using SimpleCMS.Authors;
using SimpleCMS.CmsContents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace SimpleCMS.DataSeederContributors;

public class CmsContentDataSeederContributor 
    : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<CmsContent, Guid> _repository;
    private readonly IRepository<Author, Guid> _authorRepository;
    private readonly AuthorManager _authorManager;

    public CmsContentDataSeederContributor(
        IRepository<CmsContent, Guid> repository, 
        IRepository<Author, Guid> authorRepository, 
        AuthorManager authorManager)
    {
        _repository = repository;
        _authorRepository = authorRepository;
        _authorManager = authorManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        string loremIpsum = "Lorem Ipsum is simply dummy text of the printing " +
                "and typesetting industry. Lorem Ipsum has been the industry's " +
                "standard dummy text ever since the 1500s, when an unknown printer " +
                "took a galley of type and scrambled it to make a type specimen book. " +
                "It has survived not only five centuries, but also the leap into electronic " +
                "typesetting, remaining essentially unchanged. It was popularised in the 1960s " +
                "with the release of Letraset sheets containing Lorem Ipsum passages, and more " +
                "recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";

        if (await _repository.GetCountAsync() > 0)
        {
            return;
        }

        var query = await _authorRepository.GetListAsync();
        var firstAuthor = query.First();

        List<CmsContent> cmsContentsSeed = [
            new()
                {
                    AuthorId = firstAuthor.Id,
                    Title = "Le Petit Prince by Antoine de Saint-Exupery",
                    Subtitle = "A Critical Approach",
                    PublishDate = DateTime.Now,
                    IsFeatured = true,
                    FeaturedImage = "https://placeholder.co/500",
                    Content = loremIpsum
                },
            new()
            {
                AuthorId = firstAuthor.Id,
                Title = "The Notebook by Nicholas Sparks",
                Subtitle = "A Book Review",
                PublishDate = DateTime.Now,
                IsFeatured = false,
                FeaturedImage = "https://placeholder.co/500",
                Content = loremIpsum
            },
            new()
            {
                AuthorId = firstAuthor.Id,
                Title = "The Fault In Our Stars by John Green",
                Subtitle = "A Book Review",
                PublishDate = DateTime.Now,
                IsFeatured = false,
                FeaturedImage = "https://placeholder.co/500",
                Content = loremIpsum
            }];

        await _repository.InsertManyAsync(cmsContentsSeed);

    }
}
