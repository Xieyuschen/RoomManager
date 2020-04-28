using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rmanager.Dto
{
    //public class Room
    //{
    //    public string Number { get; set; }
    //    public string Description { get; set; }
    //    public string CoverUrl { get; set; }
    //    public List<(DateTime _begintime, DateTime _endtime, Guid _id)> BookingInfo { get; set; }
    //    public string SearchAbleString { get; set; }
    //}
    public class RoomDetailDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
        public string CoverUrl { get; set; }
        //public List<BookingRoomDto> BookingInfo { get; set; }

    }
    public class AddRoomDto
    {
        public string Number { get; set; }
        public string Description { get; set; }
    }
    public class EditRoomInfoDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
        public string CoverUrl { get; set; }
    }
    public class BookingRoomDto
    {
        //这里名字不一样Dto可以通过mapper转换过去嘛
        public Guid roomId { get; set; }
        public DateTime beginTime { get; set; }
        public DateTime endTime { get; set; }
    }
}
