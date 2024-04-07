using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace SimpleCMS.CmsContents.Dtos;

public class CreateUpdateCmsContentDto : AuditedEntityDto<Guid>
{
    public Guid AuthorId { get; set; }

    [Required]
    [StringLength(128)]
    public string Title { get; set; } = string.Empty;

    [StringLength(128)]
    public string? Subtitle { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime PublishDate { get; set; } = DateTime.Now;
    public string? FeaturedImage { get; set; }

    [Required]
    [StringLength(int.MaxValue)]
    public string Content { get; set; } = string.Empty;

    public bool IsFeatured { get; set; } = false;
}
