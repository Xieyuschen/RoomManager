using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rmanager.Models;
using Rmanager.Exceptions;
using Rmanager.Dto;
using Rmanager.Services;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace Rmanager.Controllers
{
    //add in 2.9
    //在改信息的时候是采取一项一项赋值的方法，效率极低。而且之后不易修改，比如
    //说我加了几个属性那么所有的都必须修改，不能接受。
    //需要新方法：首先根据主键在数据库中找到这个对象，然后通过某种手段将
    //dto属性覆盖对象中的对应属性，无法覆盖的保留对象的原有值。如id等
    //
    //AutoMapper应该可以解决这个问题，明天再去看一下学习一下好方法。
    [Route("api/[controller]")]
    public class RoomController : Controller
    {
        public RmanagerService _rmanagerService;
        public IMapper _mapper;
        public IHostEnvironment env;
        public RoomController(RmanagerService rmanagerService,IMapper mapper,IHostEnvironment hostEnvironment)
        {
            _rmanagerService = rmanagerService;
            _mapper = mapper;
            env = hostEnvironment;
        }

        [HttpGet("{id}")]
        public async ValueTask<RoomDetailDto> GetRoom(Guid id)
        {
            var room = await _rmanagerService.RoomService.GetRoomByIdAsync(id);
            if (room == null)
            {
                throw new _401Exception("Cannot find this room!");
            }
            return _mapper.Map<RoomDetailDto>(room);
        }

        [HttpPost]
        [Route("NewRoom")]
        [Authorize(Roles = Roles.Admin)]
        public async ValueTask<RoomDetailDto> NewRoom(AddRoomDto addRoomDto)
        {
            var room = new Room
            {
                Id = Guid.NewGuid(),
                Number = addRoomDto.Number,
                Description = addRoomDto.Description,
            };
            await _rmanagerService.RoomService.AddRoom(room);
            return _mapper.Map<RoomDetailDto>(room);
        }

        /// <summary>
        /// return a RoomDetailDto with lastest Info
        /// </summary>
        /// <param name="editRoomInfoDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EditRoom")]
        [Authorize(Roles = Roles.Admin)]
        public async ValueTask<RoomDetailDto> EditRoom(EditRoomInfoDto editRoomInfoDto)
        {
            var room=await _rmanagerService.RoomService.GetRoomByIdAsync(editRoomInfoDto.Id);
            room.CoverUrl = editRoomInfoDto.CoverUrl;
            room.Description = editRoomInfoDto.Description;
            room.Number = editRoomInfoDto.Number;
            await _rmanagerService.RoomService.EditRoomInfoAsync(room);
            return _mapper.Map<RoomDetailDto>(room);
        }

        [HttpPost]
        [Route("Booking")]
        [Authorize(Roles = Roles.User)]
        public async ValueTask<BookingDetailsDto> Booking(BookingRoomDto bookingRoomDto)
        {
            if (bookingRoomDto.beginTime < bookingRoomDto.endTime && bookingRoomDto.beginTime > DateTime.Now)
            {
                //在登陆的时候 claim(Claimtype.Name,user.Id)如此设置，将Name设置成用户的id
                Guid userid = Guid.Parse(User.Identity.Name);
                var u = await _rmanagerService.BookingRoomService.BookingRoomAsync(userid, bookingRoomDto.roomId
                           , bookingRoomDto.beginTime, bookingRoomDto.endTime);
                return _mapper.Map<BookingDetailsDto>(u);
            }
            else
            {
                //开始时间在结束时间之后时报错，能否直接禁止用户输入这种不合法数据？
                throw new _401Exception("The time is not suitable, please check it again!");
            }
        }    
    }
}
