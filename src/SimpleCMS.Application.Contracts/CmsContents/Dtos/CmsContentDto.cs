using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SimpleCMS.CmsContents.Dtos;

public class CmsContentDto : CmsContentDetailDto
{
    public string Content { get; set; }
}
