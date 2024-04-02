using System;
using Volo.Abp.Application.Dtos;

namespace SimpleCMS.CmsContents.Dtos;

public class CmsContentDetailDto : AuditedEntityDto<Guid>
{
    public string Title { get; set; }
    public string? Subtitle { get; set; }
    public DateTime PublishDate { get; set; }
    public string? FeaturedImage { get; set; }
    public bool IsFeatured { get; set; }
}
