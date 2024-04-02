using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace SimpleCMS.CmsContents.Dtos;

public class InsertOrUpdateCmsContentDto : AuditedEntityDto<Guid>
{
    [Required]
    [MaxLength(128)]
    public string Title { get; set; }

    [MaxLength(128)]
    public string? Subtitle { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime PublishDate { get; set; } = DateTime.Now;
    public string? FeaturedImage { get; set; }

    [Required]
    [MaxLength(int.MaxValue)]
    public string Content { get; set; }

    public bool IsFeatured { get; set; } = false;
}
