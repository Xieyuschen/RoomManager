using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace Rmanager.Models
{
    //2.12+
    //添加了public List<Guid> BelongOrg { get; set; }
    //该用户所在的组织
    //2.12 end
    //2.13+
    //昨天的设计简直蠢到极点，
    //        public List<Guid> BelongOrg { get; set; }
    //        public List<Booking> BookingInfo { get; set; }
    //这两条放到这里，数据库更新查找的时候那酸爽，简直不敢想象，溜了溜了
    public class Room : Entity,ISearchAble
    {
        
        public string Number { get; set; }
        public string Description { get; set; }
        public string CoverUrl { get; set; }
        public string SearchAbleString { get; set; }

    }
    public class Booking
    {
        public  DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid UserId { get; set; }
    }
}
