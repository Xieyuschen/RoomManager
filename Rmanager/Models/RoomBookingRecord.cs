using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rmanager.Models
{
    //存储用户订房间的信息
    public class RoomBookingRecord:Entity,ISearchAble
    {
        public Guid UserId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string SearchAbleString { get; set; }
    }
}
