using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SimpleCMS.Books.Dtos;

public class AuthorLookupDto : EntityDto<Guid>
{
    public string Name { get; set; }
}
