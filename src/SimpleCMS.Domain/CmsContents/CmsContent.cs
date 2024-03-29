using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace SimpleCMS.CmsContents;

public class CmsContent : AuditedAggregateRoot<Guid>
{
    public virtual string Title { get; set; } //maxlength 128
    public virtual string? Subtitle { get; set; } //maxlength 128
    public virtual string Content { get; set; } //Int32.MaxValue = 2,147,483,647
    public virtual DateTime PublishDate { get; set; }
    public virtual string? FeaturedImage { get; set; }
}
