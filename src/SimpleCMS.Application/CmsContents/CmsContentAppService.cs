using Microsoft.IdentityModel.Tokens;
using SimpleCMS.Authors;
using SimpleCMS.Books;
using SimpleCMS.CmsContents.Dtos;
using SimpleCMS.Permissions;
using SimpleCMS.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace SimpleCMS.CmsContents;

public class CmsContentAppService : CrudAppService<
    CmsContent,
    CmsContentDto,
    Guid,
    PagedAndSortedResultRequestDto,
    CreateUpdateCmsContentDto>,
    ICmsContentAppService
{
    private readonly IRepository<CmsContent> _repository;
    private readonly IAuthorRepository _authorRepository;

    public CmsContentAppService (
        IRepository<CmsContent, Guid> repository,
        IAuthorRepository authorRepository
        )
        : base(repository)
    {
        _repository = repository;
        _authorRepository = authorRepository;

        #region Administrative Policies
        GetPolicyName = SimpleCMSPermissions.CmsContentsAdminPolicies.Default;
        GetListPolicyName = SimpleCMSPermissions.CmsContentsAdminPolicies.Default;
        CreatePolicyName = SimpleCMSPermissions.CmsContentsAdminPolicies.Create;
        UpdatePolicyName = SimpleCMSPermissions.CmsContentsAdminPolicies.Edit;
        DeletePolicyName = SimpleCMSPermissions.CmsContentsAdminPolicies.Delete;
        #endregion
    }

    public override async Task <CmsContentDto> GetAsync(Guid id)
    {
        try
        {
            var article = await _repository.GetAsync(article => article.Id == id);
            var articleAuthor = await _authorRepository.GetAsync(author => author.Id == article.AuthorId);

            if(article is null || articleAuthor is null)
            {
                return new CmsContentDto();
            }

            return ArticleAuthorMerger(article, articleAuthor);
        }
        catch (Exception ex)
        {
            ExceptionHandler(ex);
            return new CmsContentDto();
        }
    }

    public async Task<CreateUpdateCmsContentDto> GetCmsContentAsync(Guid id)
    {
        try
        {
            var query = await GetAsync(id);
            return MapInputForUpdating(query);
        }
        catch(Exception ex)
        {
            ExceptionHandler(ex);
            return new CreateUpdateCmsContentDto();
        }
    }
    public async Task<GetAllCmsContentDetailsDto> GetAllCmsContentDetailsAsync(PagedAndSortedResultRequestDto input)
    {
        try
        {
            var cmsContentList = await _repository.GetQueryableAsync();

            if (cmsContentList.IsNullOrEmpty())
                return new GetAllCmsContentDetailsDto(0, new List<CmsContentDetailDto>());

            var joinQuery = from cmsContent in cmsContentList
                            join author in await _authorRepository.GetQueryableAsync() on cmsContent.AuthorId equals author.Id
                            select new { CmsContent = cmsContent, Author = author };

            var query = joinQuery
                .OrderBy(NormalizeSorting(input.Sorting))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var result = await AsyncExecuter.ToListAsync(query);

            var cmsContentDetailsList = result.Select(x => new CmsContentDetailDto
            {
                Id = x.CmsContent.Id,
                AuthorId = x.Author.Id,
                AuthorName = x.Author.Name,
                Title = x.CmsContent.Title,
                Subtitle = x.CmsContent.Subtitle,
                PublishDate = x.CmsContent.PublishDate,
                IsFeatured = x.CmsContent.IsFeatured,
                FeaturedImage = x.CmsContent.FeaturedImage
            }).ToList();

            return new GetAllCmsContentDetailsDto(cmsContentDetailsList.Count, cmsContentDetailsList);
        }
        catch (Exception ex)
        {
            ExceptionHandler(ex);
            return new GetAllCmsContentDetailsDto(0, new List<CmsContentDetailDto>());
        }
    }

    public async Task InsertOrUpdateCmsContentAsync(CreateUpdateCmsContentDto input)
    {
        try
        {
            if (input.Id == Guid.Empty)
            {
                await InsertNewCmsContentAsync(input);
            }
            else
            {
                await UpdateExistingCmsContentAsync(input);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler(ex);
        }
    }

    public async Task<ListResultDto<AuthorLookupDto>> GetAuthorLookupAsync()
    {
        var authors = await _authorRepository.GetListAsync();

        return new ListResultDto<AuthorLookupDto>(
            ObjectMapper.Map<List<Author>, List<AuthorLookupDto>>(authors)
        );
    }
    private async Task InsertNewCmsContentAsync(CreateUpdateCmsContentDto input)
    {
        if(input.IsFeatured)
            _ = UnsetAllFeaturedCmsContentAsync();

        CmsContent newCmsContent = MapToCmsContent(input);
        await _repository.InsertAsync(newCmsContent, false);
    }

    private async Task UpdateExistingCmsContentAsync(CreateUpdateCmsContentDto input)
    {
        if (input.IsFeatured)
            _ = UnsetAllFeaturedCmsContentAsync();

        var query = await _repository.WithDetailsAsync();

        var updateCmsContent = query.Where(x => x.Id == input.Id)
            .FirstOrDefault();

        MapInputToCmsContent(input, updateCmsContent);

        await _repository.UpdateAsync(updateCmsContent);
    }

    private CreateUpdateCmsContentDto MapInputForUpdating(CmsContentDto input)
    {
        return ObjectMapper.Map<CmsContentDto, CreateUpdateCmsContentDto>(input);
    }
    private CmsContent MapToCmsContent(CreateUpdateCmsContentDto input)
    {
        return new CmsContent
        {
            Title = input.Title,
            Subtitle = input.Subtitle,
            AuthorId = input.AuthorId,
            Content = input.Content,
            PublishDate = input.PublishDate,
            FeaturedImage = input.FeaturedImage,
            IsFeatured = input.IsFeatured
        };
    }

    private void MapInputToCmsContent(CreateUpdateCmsContentDto input, CmsContent cmsContent)
    {
        cmsContent.AuthorId = input.AuthorId;
        cmsContent.Title = input.Title;
        cmsContent.Subtitle = input.Subtitle;
        cmsContent.Content = input.Content;
        cmsContent.PublishDate = input.PublishDate;
        cmsContent.FeaturedImage = input.FeaturedImage;
        cmsContent.IsFeatured = input.IsFeatured;
    }

    private async Task UnsetAllFeaturedCmsContentAsync()
    {
        var query = await _repository.WithDetailsAsync();

        var unsetFeatured = query
            .Where(x => x.IsFeatured == true)
            .ToList();

        unsetFeatured = unsetFeatured
            .Select(article => 
            { 
                article.IsFeatured = false; 
                return article; 
            })
            .ToList();

        await _repository.UpdateManyAsync(unsetFeatured);
    }

    private void ExceptionHandler(Exception ex)
    {
        string innerException = ex.InnerException != null ? 
            $"\nInnerException:{ex.InnerException.Message}" : string.Empty;

        throw new UserFriendlyException($"{innerException}");
    }

    private CmsContentDto ArticleAuthorMerger(CmsContent cmsContentQuery, Author authorContentQuery)
    {
        return new CmsContentDto()
        {
            Id = cmsContentQuery.Id,
            Title = cmsContentQuery.Title,
            Subtitle = cmsContentQuery.Subtitle,
            AuthorId = authorContentQuery.Id,
            AuthorName = authorContentQuery.Name,
            PublishDate = cmsContentQuery.PublishDate,
            FeaturedImage = cmsContentQuery.FeaturedImage,
            IsFeatured = cmsContentQuery.IsFeatured,
            Content = cmsContentQuery.Content
        };
    }
    private static string NormalizeSorting(string sorting)
    {
        if (sorting.IsNullOrEmpty())
        {
            return $"cmsContent.{nameof(CmsContent.Title)}";
        }

        if (sorting.Contains("authorName", StringComparison.OrdinalIgnoreCase))
        {
            return sorting.Replace(
                "authorName",
                "author.Name",
                StringComparison.OrdinalIgnoreCase
            );
        }

        return $"cmsContent.{sorting}";
    }
}
