using SimpleCMS.CmsContents.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public CmsContentAppService (IRepository<CmsContent, Guid> repository)
        : base(repository)
    {
        _repository = repository;
    }

    public async Task<GetAllCmsContentDetailsDto> GetAllCmsContentDetailsAsync()
    {
        try
        {
            var query = await _repository.GetQueryableAsync();

            var cmsContentDetailsList = query.Select(item => 
                new CmsContentDetailDto
                {
                    Id = item.Id,
                    Title = item.Title,
                    Subtitle = item.Subtitle,
                    PublishDate = item.PublishDate,
                    FeaturedImage = item.FeaturedImage,
                    IsFeatured = item.IsFeatured
                }).ToList();

            return new GetAllCmsContentDetailsDto(cmsContentDetailsList.Count, cmsContentDetailsList); ;
        }
        catch(Exception ex)
        {
            ExceptionHandler(ex);
            return new GetAllCmsContentDetailsDto(0, new List<CmsContentDetailDto>());
        }
        finally
        {
            Console.WriteLine("An error occurred while retrieving content/s.");
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
        finally
        {
            Console.WriteLine("An error occurred while saving changes.");
        }
    }
    private async Task InsertNewCmsContentAsync(CreateUpdateCmsContentDto input)
    {
        if(input.IsFeatured)
            _ = UnsetAllFeaturedCmsContentAsync();

        CmsContent newCmsContent = MapToCmsContent(input);

        await _repository.InsertAsync(newCmsContent, autoSave: true);
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

    private CmsContent MapToCmsContent(CreateUpdateCmsContentDto input)
    {
        return new CmsContent
        {
            Title = input.Title,
            Subtitle = input.Subtitle,
            Content = input.Content,
            PublishDate = input.PublishDate,
            FeaturedImage = input.FeaturedImage,
            IsFeatured = input.IsFeatured
        };
    }

    private void MapInputToCmsContent(CreateUpdateCmsContentDto input, CmsContent cmsContent)
    {
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
}
