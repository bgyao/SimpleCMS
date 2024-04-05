using SimpleCMS.Authors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCMS.EntityFrameworkCore.Applications.Authors;

[Collection(SimpleCMSTestConsts.CollectionDefinitionName)]
public class EfCoreAuthorAppService_Tests : AuthorAppService_Tests<SimpleCMSEntityFrameworkCoreTestModule>
{

}
