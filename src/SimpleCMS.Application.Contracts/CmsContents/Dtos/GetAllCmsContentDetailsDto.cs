﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCMS.CmsContents.Dtos;

public class GetAllCmsContentDetailsDto
{
    public int TotalCount { get; set; } = 0;
    public List<CmsContentDetailDto> Items { get; set; }

    public GetAllCmsContentDetailsDto(int totalCount, List<CmsContentDetailDto> items)
    {
        TotalCount = totalCount;
        Items = items;
    }
}
