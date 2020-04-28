using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rmanager.Models;
using Rmanager.Dto;
using MongoDB.Driver;
using Rmanager.Exceptions;
using Rmanager.Services;
namespace Rmanager.Services
{
    public class BookingRoomService:DBQueryService<RoomBookingRecord>
    {
        public BookingRoomService(IDatabaseSettings settings)
            : base(settings, settings.BookingRoomCollectionName) { }
        
        /// <summary>
        /// Return a List of RoomBookingRecord booked by a certain user.
        /// Attention that in the method treat the userId as a exist Id in default.
        /// Don't forget check it before using this method.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async ValueTask<List<RoomBookingRecord>> GetUserBookingRecords(Guid userId)
        {
            var t = (await collection.FindAsync(t => t.UserId == userId)).ToList();
            return t;
        }
        /// <summary>
        /// Treat all id are vaild and starttime<endtime,Please check before using this method.
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roomId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async ValueTask<RoomBookingRecord> BookingRoomAsync(Guid userId,Guid roomId,DateTime startTime,DateTime endTime)
        {
            RoomBookingRecord roomBookingRecord = new RoomBookingRecord()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                RoomId = roomId,
                StartTime = startTime,
                EndTime = endTime
            };
            var t = await CheckTimeAvailable(roomId, startTime, endTime);
            if (t)
            {
                await AddAsync(roomBookingRecord);
                return roomBookingRecord;
            }
            else
            {
                throw new _400Exception("The time you choose is not available!");
            }
        }


        /// <summary>
        /// Check if the time in spcified room is available.
        /// Return true when time is available.
        /// </summary>
        /// <param name="roomid"></param>
        /// <param name="begintime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public async ValueTask<bool> CheckTimeAvailable(Guid roomid, DateTime begintime, DateTime endtime)
        {
            var t =(await collection.FindAsync(t => t.RoomId==roomid)).ToList();
            //添加一个感叹号，因为xxx.Any()在有东西的时候返回true
            if (!t.Any())
            {
                return true;
            }
            foreach (var item in t)
            {
                if ((item.StartTime< begintime && item.StartTime > begintime && item.EndTime > endtime) ||
                   (item.StartTime < begintime && item.EndTime < endtime && item.EndTime > begintime) ||
                   (item.StartTime < begintime && item.EndTime > endtime))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
