using Shouldly;
using SimpleCMS.Authors;
using SimpleCMS.Authors.Dtos;
using SimpleCMS.CmsContents.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Xunit;

namespace SimpleCMS.CmsContents;

public abstract class CmsContentAppService_Tests<TStartupModule> :
    SimpleCMSApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ICmsContentAppService _contentAppService;
    private readonly IAuthorAppService _authorAppService;

    protected CmsContentAppService_Tests()
    {
        _contentAppService = GetRequiredService<ICmsContentAppService>();
        _authorAppService = GetRequiredService<IAuthorAppService>();
    }

    [Fact]
    public async Task Should_Get_List_of_CmsContent_Details()
    {
        //Act
        var result = await _contentAppService.GetAllCmsContentDetailsAsync(
            new PagedAndSortedResultRequestDto() );

        //Assert
        result.ShouldNotBeNull();
        result.TotalCount.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get_CmsContent_By_Id()
    {
        //Act
        var listQuery = await _contentAppService.GetAllCmsContentDetailsAsync(
            new PagedAndSortedResultRequestDto() );
        var lastArticle = listQuery.Items.Last();

        var result = await _contentAppService.GetCmsContentAsync(lastArticle.Id);

        result.ShouldNotBeNull();
        result.Id.ShouldBe( lastArticle.Id );
        result.Title.ShouldBe(lastArticle.Title);
    }

    [Fact]
    public async Task Should_Not_Get_CmsContent_With_Invalid_Id()
    {
        //Act
        var guid = new Guid();

        //Assert
        var ex = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            var result = await _contentAppService.GetCmsContentAsync(guid);
        });

        ex.ShouldNotBeNull();
        ex.Message.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Create_A_Valid_CmsContent()
    {
        var authors = await _authorAppService.GetListAsync(new GetAuthorListDto());
        var firstAuthor = authors.Items.First();

        //Act
       
        await _contentAppService.InsertOrUpdateCmsContentAsync(
        new CreateUpdateCmsContentDto()
        {
            AuthorId = authors.Items[0].Id,
            Title = "New test article",
            Subtitle = "Subtitle",
            IsFeatured = true,
            FeaturedImage = null,
            Content = "Test content",
            PublishDate = DateTime.Now
        });

        var query = await _contentAppService.GetAllCmsContentDetailsAsync(
            new PagedAndSortedResultRequestDto());

        var result = query.Items.Where(x => x.Title == "New test article")
            .FirstOrDefault();

        result.ShouldNotBeNull();
        result.Title.ShouldBe("New test article");
        
    }

    [Fact]
    public async Task Should_Not_Create_A_CmsContent_Without_A_Title()
    {
        var authors = await _authorAppService.GetListAsync(new GetAuthorListDto());
        var firstAuthor = authors.Items.First();

        //Assert
        var ex = await Assert.ThrowsAsync<AbpValidationException>( async() =>
        {
            await _contentAppService.InsertOrUpdateCmsContentAsync(
                new CreateUpdateCmsContentDto()
                {
                    AuthorId = authors.Items[0].Id,
                    Title = "",
                    Subtitle = "Subtitle",
                    IsFeatured = true,
                    FeaturedImage = null,
                    Content = "Test content",
                    PublishDate = DateTime.Now
                });
        });

        ex.ValidationErrors
            .ShouldContain(err => err.MemberNames.Any(mem => mem == "Title"));

    }
    [Fact]
    public async Task Should_Not_Create_A_CmsContent_If_Title_Exceeds_MaxLength()
    {
        var authors = await _authorAppService.GetListAsync(new GetAuthorListDto());
        var firstAuthor = authors.Items.First();

        //Assert
        var ex = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _contentAppService.InsertOrUpdateCmsContentAsync(
                new CreateUpdateCmsContentDto()
                {
                    AuthorId = authors.Items[0].Id,
                    Title = "The quick brown fox jumps over the lazy dog because the dog is too lazy to move and the turtle beats the rabbit in a mockup race!",
                    Subtitle = "Subtitle goes here",
                    IsFeatured = true,
                    FeaturedImage = null,
                    Content = "Test content",
                    PublishDate = DateTime.Now
                });
        });

        ex.ValidationErrors
            .ShouldContain(err => err.MemberNames.Any(mem => mem == "Title"));

    }


    [Fact]
    public async Task Should_Not_Create_A_CmsContent_If_Subtitle_Exceeds_MaxLength()
    {
        var authors = await _authorAppService.GetListAsync(new GetAuthorListDto());
        var firstAuthor = authors.Items.First();

        //Assert
        var ex = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _contentAppService.InsertOrUpdateCmsContentAsync(
                new CreateUpdateCmsContentDto()
                {
                    AuthorId = authors.Items[0].Id,
                    Title = "New Title Article",
                    Subtitle = "The quick brown fox jumps over the lazy dog because the dog is too lazy to move and the turtle beats the rabbit in a mockup race!",
                    IsFeatured = true,
                    FeaturedImage = null,
                    Content = "Test content",
                    PublishDate = DateTime.Now
                });
        });

        ex.ValidationErrors
            .ShouldContain(err => err.MemberNames.Any(mem => mem == "Subtitle"));

    }

    [Fact]
    public async Task Should_Not_Create_A_CmsContent_If_Content_Is_Null()
    {
        var authors = await _authorAppService.GetListAsync(new GetAuthorListDto());
        var firstAuthor = authors.Items.First();

        //Assert
        var ex = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _contentAppService.InsertOrUpdateCmsContentAsync(
                new CreateUpdateCmsContentDto()
                {
                    AuthorId = authors.Items[0].Id,
                    Title = "New Title Article",
                    Subtitle = "Subtitle goes here",
                    IsFeatured = true,
                    FeaturedImage = null,
                    Content = "",
                    PublishDate = DateTime.Now
                });
        });

        ex.ValidationErrors
            .ShouldContain(err => err.MemberNames.Any(mem => mem == "Content"));

    }

    [Fact]
    public async Task Should_Update_A_Valid_CmsContent()
    {
        //Act
        var authors = await _authorAppService.GetListAsync(new GetAuthorListDto());
        var contents = await _contentAppService.GetAllCmsContentDetailsAsync(
            new PagedAndSortedResultRequestDto() );
        var article = await _contentAppService.GetCmsContentAsync(contents.Items[0].Id);

        await _contentAppService.InsertOrUpdateCmsContentAsync(
            new CreateUpdateCmsContentDto()
            {
                Id = article.Id,
                AuthorId = authors.Items[0].Id,
                Title = "New test article",
                Subtitle = "Subtitle",
                IsFeatured = true,
                FeaturedImage = null,
                Content = "Test content",
                PublishDate = DateTime.Now
            });

        var query = await _contentAppService.GetAllCmsContentDetailsAsync(
            new PagedAndSortedResultRequestDto());

        var result = query.Items.Where(x => x.Title == "New test article")
            .FirstOrDefault();

        //Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(article.Id);
        result.AuthorId.ShouldBe(authors.Items[0].Id);
        result.Title.ShouldBe("New test article");

    }

    [Fact]
    public async Task Should_Not_Update_A_CmsContent_Without_A_Title()
    {
        //Act
        var authors = await _authorAppService.GetListAsync(new GetAuthorListDto());
        var contents = await _contentAppService.GetAllCmsContentDetailsAsync(
            new PagedAndSortedResultRequestDto());
        var article = await _contentAppService.GetCmsContentAsync(contents.Items[0].Id);

        //Assert
        var ex = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _contentAppService.InsertOrUpdateCmsContentAsync(
                new CreateUpdateCmsContentDto()
                {
                    Id = article.Id,
                    AuthorId = authors.Items[0].Id,
                    Title = "",
                    Subtitle = "Subtitle",
                    IsFeatured = true,
                    FeaturedImage = null,
                    Content = "Test content",
                    PublishDate = DateTime.Now
                });
        });

        ex.ValidationErrors
            .ShouldContain(err => err.MemberNames.Any(mem => mem == "Title"));

    }
    [Fact]
    public async Task Should_Not_Update_A_CmsContent_If_Title_Exceeds_MaxLength()
    {
        //Act
        var authors = await _authorAppService.GetListAsync(new GetAuthorListDto());
        var contents = await _contentAppService.GetAllCmsContentDetailsAsync(
            new PagedAndSortedResultRequestDto());
        var article = await _contentAppService.GetCmsContentAsync(contents.Items[0].Id);

        //Assert
        var ex = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _contentAppService.InsertOrUpdateCmsContentAsync(
                new CreateUpdateCmsContentDto()
                {
                    Id = article.Id,
                    AuthorId = authors.Items[0].Id,
                    Title = "The quick brown fox jumps over the lazy dog because the dog is too lazy to move and the turtle beats the rabbit in a mockup race!",
                    Subtitle = "Subtitle goes here",
                    IsFeatured = true,
                    FeaturedImage = null,
                    Content = "Test content",
                    PublishDate = DateTime.Now
                });
        });

        ex.ValidationErrors
            .ShouldContain(err => err.MemberNames.Any(mem => mem == "Title"));

    }


    [Fact]
    public async Task Should_Not_Update_A_CmsContent_If_Subtitle_Exceeds_MaxLength()
    {
        //Act
        var authors = await _authorAppService.GetListAsync(new GetAuthorListDto());
        var contents = await _contentAppService.GetAllCmsContentDetailsAsync(
            new PagedAndSortedResultRequestDto());
        var article = await _contentAppService.GetCmsContentAsync(contents.Items[0].Id);

        //Assert
        var ex = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _contentAppService.InsertOrUpdateCmsContentAsync(
                new CreateUpdateCmsContentDto()
                {
                    Id = article.Id,
                    AuthorId = authors.Items[0].Id,
                    Title = "New Title Article",
                    Subtitle = "The quick brown fox jumps over the lazy dog because the dog is too lazy to move and the turtle beats the rabbit in a mockup race!",
                    IsFeatured = true,
                    FeaturedImage = null,
                    Content = "Test content",
                    PublishDate = DateTime.Now
                });
        });

        ex.ValidationErrors
            .ShouldContain(err => err.MemberNames.Any(mem => mem == "Subtitle"));

    }

    [Fact]
    public async Task Should_Not_Update_A_CmsContent_If_Content_Is_Null()
    {
        //Act
        var authors = await _authorAppService.GetListAsync(new GetAuthorListDto());
        var contents = await _contentAppService.GetAllCmsContentDetailsAsync(
            new PagedAndSortedResultRequestDto());
        var article = await _contentAppService.GetCmsContentAsync(contents.Items[0].Id);

        //Assert
        var ex = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _contentAppService.InsertOrUpdateCmsContentAsync(
                new CreateUpdateCmsContentDto()
                {
                    Id = article.Id,
                    AuthorId = authors.Items[0].Id,
                    Title = "New Title Article",
                    Subtitle = "Subtitle goes here",
                    IsFeatured = true,
                    FeaturedImage = null,
                    Content = "",
                    PublishDate = DateTime.Now
                });
        });

        ex.ValidationErrors
            .ShouldContain(err => err.MemberNames.Any(mem => mem == "Content"));

    }

    [Fact]
    public async Task Should_Delete_Valid_CmsContent()
    {
        // Note:
        //      Step 1: Retrieve a valid CmsContent
        //      Step 2: Get the Id
        //      Step 3: DeleteAsync(CmsContent.Id)
        //      Step 4: Use the same Id in GetAsync()
        //      RESULT: UserFriendlyException
        // Reason for step 4: DeleteAsync will always return 204 Undocumented regardless 
        // if the Guid passed to it exists or not.

        //Act
        var query = await _contentAppService.GetAllCmsContentDetailsAsync(
            new PagedAndSortedResultRequestDto());

        await _contentAppService.DeleteAsync(query.Items[0].Id);

        //Assert
        var ex = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
        {
            var checkIfDeleted = await _contentAppService.GetCmsContentAsync(query.Items[0].Id);
        });

        ex.ShouldNotBeNull();
        ex.Message.ShouldNotBeNull();
    }
}
