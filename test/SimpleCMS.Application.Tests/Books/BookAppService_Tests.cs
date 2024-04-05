using Shouldly;
using SimpleCMS.Authors;
using SimpleCMS.Authors.Dtos;
using SimpleCMS.Books.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Xunit;
using Xunit.Sdk;

namespace SimpleCMS.Books;

public abstract class BookAppService_Tests<TStartupModule> :
    SimpleCMSApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IBookAppService _bookAppService;
    private readonly IAuthorAppService _authorAppService;

    protected BookAppService_Tests()
    {
        _bookAppService = GetRequiredService<IBookAppService>();
        _authorAppService = GetRequiredService<IAuthorAppService>();
    }

    [Fact]
    public async Task Should_Get_List_Of_Books()
    {
        //Act
        var result = await _bookAppService.GetListAsync(
            new PagedAndSortedResultRequestDto() );

        //Assert
        result.TotalCount.ShouldBeGreaterThan(0);
        result.Items.ShouldContain(b => b.Name == "1984" && b.AuthorName == "George Orwell");
    }

    [Fact]
    public async Task Should_Get_Book_By_Id()
    {
        //Act
        var query = await _bookAppService.GetListAsync(
            new PagedAndSortedResultRequestDto());
        var firstBook = query.Items.First();

        var result = await _bookAppService.GetAsync(firstBook.Id);

        //Assert
        result.Id.ShouldNotBe(Guid.Empty);
        result.Id.ShouldBe(firstBook.Id);
        result.Name.ShouldBe(firstBook.Name);
    }

    [Fact]
    public async Task Should_Not_Get_Non_Existing_Book_By_Id()
    {
        //Act
        var guid = new Guid();

        //Assert
        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            var result = await _bookAppService.GetAsync(guid);
        });

        ex.Id.ShouldBe(guid);
    }

    [Fact]
    public async Task Should_Create_A_Valid_Book()
    {
        var authors = await _authorAppService.GetListAsync( new GetAuthorListDto() );
        var firstAuthor = authors.Items.First();

        //Act
        var result = await _bookAppService.CreateAsync(
            new CreateUpdateBookDto
            {
                AuthorId = firstAuthor.Id,
                Name = "New test book 42",
                Price = 10,
                PublishDate = DateTime.Now,
                Type = BookType.ScienceFiction
            });

        //Assert
        result.Id.ShouldNotBe(Guid.Empty);
        result.Name.ShouldBe("New test book 42");
    }

    [Fact]
    public async Task Should_Not_Create_A_Book_Without_Name()
    {
        var authors = await _authorAppService.GetListAsync(new GetAuthorListDto());
        var firstAuthor = authors.Items.First();

        var ex = await Assert.ThrowsAsync<AbpValidationException>( async() =>
        {
            await _bookAppService.CreateAsync(
                new CreateUpdateBookDto
                {
                    AuthorId = firstAuthor.Id,
                    Name = "",
                    Price = 10,
                    PublishDate = DateTime.Now,
                    Type = BookType.ScienceFiction
                }
                );
        });

        ex.ValidationErrors
            .ShouldContain(err => err.MemberNames.Any(mem => mem == "Name"));
    }

    // Not included in the tutorial. My addition. Name has 129 characters
    [Fact]
    public async Task Should_Not_Create_A_Book_With_Name_More_Than_Max_Char_Limit()
    {
        var authors = await _authorAppService.GetListAsync(new GetAuthorListDto());
        var firstAuthor = authors.Items.First();

        var ex = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _bookAppService.CreateAsync(
                new CreateUpdateBookDto
                { 
                    AuthorId = firstAuthor.Id,
                    Name = "The quick brown fox jumps over the lazy dog because the dog is too lazy to move and the turtle beats the rabbit in a mockup race!",
                    Price = 10,
                    PublishDate = DateTime.Now,
                    Type = BookType.ScienceFiction
                }
                );
        });

        ex.ValidationErrors
            .ShouldContain(err => err.MemberNames.Any(mem => mem == "Name"));
    }

    [Fact]
    public async Task Should_Return_Valid_AuthorLookup_List()
    {
        //Act
        var result = await _bookAppService.GetAuthorLookupAsync();

        //Assert
        result.ShouldNotBeNull();
        result.Items.ShouldNotBeNull();
        result.Items.ShouldNotBeEmpty();
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Update_A_Valid_Book()
    {
        //Act
        var query = await _bookAppService.GetListAsync(
            new PagedAndSortedResultRequestDto());
        var firstBook = query.Items.First();

        var result = await _bookAppService.UpdateAsync(
            firstBook.Id,
            new CreateUpdateBookDto
            { 
                AuthorId = firstBook.AuthorId,
                Name = "Lord of the Rings",
                Price = 10,
                PublishDate = DateTime.Now,
                Type = BookType.Fantastic
            });

        //Assert
        result.ShouldNotBeNull();
        result.Name.ShouldBe("Lord of the Rings");
        result.Type.ShouldBe(BookType.Fantastic);

    }

    [Fact]
    public async Task Should_Not_Update_Book_With_No_Title()
    {
        //Act
        var query = await _bookAppService.GetListAsync(
            new PagedAndSortedResultRequestDto());
        var firstBook = query.Items.First();

        var ex = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _bookAppService.UpdateAsync(
                firstBook.Id,
                new CreateUpdateBookDto
                {
                    AuthorId = firstBook.AuthorId,
                    Name = "",
                    Price = 10,
                    PublishDate = DateTime.Now,
                    Type = BookType.ScienceFiction
                }
                );
        });

        ex.ValidationErrors
            .ShouldContain(err => err.MemberNames.Any(mem => mem == "Name"));
    }

    [Fact]
    public async Task Should_Delete_Valid_Book()
    {
        // Note:
        //      Step 1: Retrieve a valid book
        //      Step 2: Get the Book.Id
        //      Step 3: DeleteAsync(Book.Id)
        //      Step 4: Use the same Book.Id in GetAsync()
        //      RESULT: EntityNotFoundException
        // Reason for step 4: DeleteAsync will always return 204 Undocumented regardless 
        // if the Guid passed to it exists or not.

        //Act
        var query = await _bookAppService.GetListAsync(
            new PagedAndSortedResultRequestDto());
        var firstBook = query.Items.First();

        await _bookAppService.DeleteAsync(firstBook.Id );

        //Assert
        var ex = await Assert.ThrowsAsync<EntityNotFoundException>( async() =>
        {
            var checkIfBookIsDeleted = await _bookAppService.GetAsync(firstBook.Id);
        });

        ex.Id.ShouldBe(firstBook.Id);
    }
}