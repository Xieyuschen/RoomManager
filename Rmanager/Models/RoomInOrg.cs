using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rmanager.Models
{
    //用来存储房间与组织的隶属关系
    public class RoomInOrg:Entity,ISearchAble
    {
        public Guid roomId { get; set; }
        public Guid orgId { get; set; }
        public string SearchAbleString { get; set; }

    }
}
