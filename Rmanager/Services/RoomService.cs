using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rmanager.Models;
using MongoDB.Driver;
using Rmanager.Exceptions;
using Rmanager.Services;
namespace Rmanager.Services
{
    //2.8日加：
    //错误处理异常捕获应该全部都放在controller中完成？？
    //——这样的好处就是只需要在controller中考虑要如何处理要出
    public class RoomService:DBQueryService<Room>
    {
        public RoomService(IDatabaseSettings settings)
            : base(settings, settings.RoomCollectionName)
        {
        }
        public async ValueTask<Room> AddRoom(Room room)
        {
            try
            {
                if (!(await collection.FindAsync(t => t.Id == room.Id)).Any())
                {
                    await AddAsync(room);
                    return room;
                }
                else throw new Exception();
            }
            catch (Exception)
            {
                throw new _401Exception("This room has already existed!");
            }
         
        }
        public async ValueTask<Room> GetRoomByNumber(string num)
        {
            try
            {
                return await (await collection.FindAsync(t => t.Number == num)).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw new _401Exception($"Cannot find the {num} room");
            }
        }
        //这个方法好像没啥用，先放这里吧之后再去掉
        public async ValueTask<Guid> GetRoomIdBynum(string num)
        {
            try
            {
                return (await collection.FindAsync(t => t.Number == num)).FirstOrDefault().Id;
            }
            catch (Exception)
            {
                throw new _400Exception($"Cannot find the {num} room!");
            }
        }

        public async ValueTask<Room> GetRoomByIdAsync(Guid roomid)
        {
            try
            {
                return (await (await collection.FindAsync(t => t.Id == roomid)).FirstAsync());
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async ValueTask<Room> EditRoomInfoAsync(Room room)
        {
            await collection.UpdateOneAsync(Builders<Room>.Filter.Where(t => t.Id == room.Id),
                Builders<Room>.Update.Set(t => t.Number, room.Number)
                .Set(t => t.Description, room.Description)
                .Set(t => t.CoverUrl, room.CoverUrl)
                );
            return room;
        }


    }
}
