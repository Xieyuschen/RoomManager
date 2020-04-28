using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rmanager.Models
{
    public class UserInOrg:Entity,ISearchAble
    {
        public Guid userId { get; set; }
        public Guid orgId { get; set; }
        public string SearchAbleString { get; set; }
    }
}
