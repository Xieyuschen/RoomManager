using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rmanager.Models;
using Rmanager.Dto;
using MongoDB.Driver;
using Rmanager.Exceptions;

namespace Rmanager.Services
{
    public class RoomInOrgService:DBQueryService<RoomInOrg>
    {
        public RoomInOrgService(IDatabaseSettings settings)
            : base(settings, settings.RoomInOrgCollectionName) { }
        public async ValueTask<List<Guid>> GetRoomsInOrgAsync(Guid orgId)
        {
            var Set = (await collection.FindAsync(t => t.orgId == orgId)).ToList();
            List<Guid> roomlist = new List<Guid>();
            foreach (var item in Set)
            {
                roomlist.Add(item.roomId);
            }
            return roomlist;
        }
        public async ValueTask<RoomInOrg> AddRoomInOrgAsync(Guid roomid,Guid orgid)
        {
            //所有查询都要在数据库内完成，取出来再检查就很蠢
            var t = (await collection.FindAsync(t => t.roomId == roomid && t.orgId == orgid)).Any();
            if (t)
            {
                throw new _401Exception("You cannot add a room which already in this org!");
            }
            else
            {
                RoomInOrg roomInOrg = new RoomInOrg() {Id=Guid.NewGuid(),orgId = orgid, roomId = roomid };
                await AddAsync(roomInOrg);
                return roomInOrg;
            }
        }
    }
}
