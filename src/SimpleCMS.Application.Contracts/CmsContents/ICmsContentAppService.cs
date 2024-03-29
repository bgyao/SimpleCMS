﻿using SimpleCMS.CmsContents.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SimpleCMS.CmsContents;

public interface ICmsContentAppService : ICrudAppService<
    CmsContentDto,
    Guid,
    PagedAndSortedResultRequestDto,
    InsertOrUpdateCmsContentDto> //should be separate by SOLID Principles
{
    Task<GetAllCmsContentDetailsDto> GetAllCmsContentDetailsAsync();
    Task InsertOrUpdateCmsContent(CmsContentDto input);
}