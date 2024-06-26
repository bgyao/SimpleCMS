﻿using Shouldly;
using SimpleCMS.Authors.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Xunit;

namespace SimpleCMS.Authors;

public abstract class AuthorAppService_Tests<TStartupModule> : SimpleCMSApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IAuthorAppService _authorAppService;

    protected AuthorAppService_Tests()
    {
        _authorAppService = GetRequiredService<IAuthorAppService>();
    }

    [Fact]
    public async Task Should_Get_All_Authors_Without_Any_Filter()
    {
        var result = await _authorAppService.GetListAsync(new GetAuthorListDto());

        result.TotalCount.ShouldBeGreaterThanOrEqualTo(2);
        result.Items.ShouldContain(author => author.Name == "George Orwell");
        result.Items.ShouldContain(author => author.Name == "Douglas Adams");
    }

    [Fact]
    public async Task Should_Get_Filtered_Authors()
    {
        var result = await _authorAppService.GetListAsync(
            new GetAuthorListDto { Filter = "George" });

        result.TotalCount.ShouldBeGreaterThanOrEqualTo(1);
        result.Items.ShouldContain(author => author.Name == "George Orwell");
        result.Items.ShouldNotContain(author => author.Name == "Douglas Adams");
    }

    [Fact]
    public async Task Should_Create_A_New_Author()
    {
        var authorDto = await _authorAppService.CreateAsync(
            new CreateAuthorDto
            {
                Name = "Edward Bellamy",
                BirthDate = new DateTime(1850, 05, 22),
                ShortBio = "Edward Bellamy was an American author..."
            }
        );

        authorDto.Id.ShouldNotBe(Guid.Empty);
        authorDto.Name.ShouldBe("Edward Bellamy");
    }

    [Fact]
    public async Task Should_Not_Allow_To_Create_Duplicate_Author()
    {
        await Assert.ThrowsAsync<AuthorAlreadyExistsException>(async () =>
        {
            await _authorAppService.CreateAsync(
                new CreateAuthorDto
                {
                    Name = "Douglas Adams",
                    BirthDate = DateTime.Now,
                    ShortBio = "..."
                }
            );
        });
    }
    [Fact]
    public async Task Should_Update_Existing_Author()
    {
        var query = await _authorAppService.GetListAsync(
            new GetAuthorListDto { Filter = "Douglas" });

        var result = query.Items.FirstOrDefault();

        await _authorAppService.UpdateAsync(
            result.Id,
            new UpdateAuthorDto
            {
                Name = "Douglas Smith",
                BirthDate = result.BirthDate,
                ShortBio = "Douglas Smith was an American author who ..."
            });

        var checkQuery = await _authorAppService.GetListAsync(
            new GetAuthorListDto { Filter = "Smith" });

        var updatedAuthor = checkQuery.Items.FirstOrDefault();

        updatedAuthor.ShouldNotBeNull();
        updatedAuthor.Name.ShouldBe("Douglas Smith");
    }

    [Fact]
    public async Task Should_Not_Allow_Updating_Author_If_Duplicate()
    {
        await Assert.ThrowsAsync<AuthorAlreadyExistsException>( async() => 
        {
            var query = await _authorAppService.GetListAsync(
            new GetAuthorListDto { Filter = "George" });

            var result = query.Items.FirstOrDefault();

            await _authorAppService.UpdateAsync(
                result.Id,
                new UpdateAuthorDto
                {
                    Name = "Douglas Adams",
                    BirthDate = result.BirthDate,
                    ShortBio = result.ShortBio
                }
            );
        });
    }

    [Fact]
    public async Task Should_Delete_Existing_Author()
    {
        // Note:
        //      Step 1: Retrieve a valid author
        //      Step 2: Get the Author.Id
        //      Step 3: DeleteAsync(Author.Id)
        //      Step 4: Use the same Author.Id in GetAsync()
        //      RESULT: EntityNotFoundException
        // Reason for step 4: DeleteAsync will always return 204 Undocumented regardless 
        // if the Guid passed to it exists or not.

        //Act
        var query = await _authorAppService.GetListAsync(
            new GetAuthorListDto { Filter = "Douglas" });

        var result = query.Items.FirstOrDefault();

        await _authorAppService.DeleteAsync(result.Id);

        //Assert
        var ex = await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            var checkDeletedThroughGetById = await _authorAppService.GetAsync(result.Id);
        });

        ex.Id.ShouldBe(result.Id);
    }
}