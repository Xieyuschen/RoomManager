using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rmanager.Models;
using Rmanager.Dto;
using Rmanager.Exceptions;
using Rmanager.Services;
using AutoMapper;
using Microsoft.Extensions.Hosting;

namespace Rmanager.Controllers
{
    [Route("api/[controller]")]
    public class OrganizationController : Controller
    {
        public RmanagerService _rmanagerService;
        public IMapper _mapper;
        IHostEnvironment env;
        public OrganizationController(RmanagerService rmanagerService, IMapper mapper,IHostEnvironment env)
        {
            _rmanagerService = rmanagerService;
            _mapper = mapper;
            this.env = env;
        }
        // GET: api/<controller>
        [AllowAnonymous]
        [HttpGet]
        public async ValueTask<OrganizationDetailDto> GetOrganiztion(string id=null)
        {
            var orgId = Guid.Parse(id);
            var org =await _rmanagerService.OrganizationService.GetOrgByIdAsync(orgId);
            if (org!=null)
            {
                var u= _mapper.Map<OrganizationDetailDto>(org);
                u.users = await GetUsersInOrg(orgId);
                u.rooms = await GetRoomsInOrg(orgId);
                return u;

                //感觉返回整个RoomDetailDto 和UserDetailDto有点太啰嗦了，代码就先注释掉吧。之后如果需要的话
                //就把OrganizationDetailDto里面users和rooms的泛型内容改一下就可以了
                //现在还是返回 User 和 Room 的 id列表

                ////下面这两句很关键，不要动不动就对着虚空操作了
                //u.users = new List<Guid>();
                //u.rooms = new List<Guid>();
                //var useridlist= await GetUsersInOrg(orgId);
                //foreach(var item in useridlist)
                //{
                //    var user= (await _rmanagerService.UserService.GetUserByIdAsync(item));
                //    var x = _mapper.Map<UserDetailDto>(user);
                //    u.users.Add(x);
                //}
                //var roomidlist = await GetRoomsInOrg(orgId);
                //foreach (var item in roomidlist)
                //{
                //    var room = (await _rmanagerService.RoomService.GetRoomByIdAsync(item));
                //    u.rooms.Add(_mapper.Map<RoomDetailDto>(room));
                //}
            }
            else
            {
                throw new _401Exception("Cannot find this organization!");
            }
        }
        /// <summary>
        /// Add a new Org and return its detail
        /// we check whether the room exists in Controller and don't need to check in services
        /// </summary>
        /// <param name="addOrgDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("NewOrg")]
        [Authorize(Roles=Roles.Admin)]
        public async ValueTask<OrganizationDetailDto> AddOrganization(AddOrgDto addOrgDto)
        {
            var t =await _rmanagerService.OrganizationService.GetOrgByName(addOrgDto.Name);
            if (t == null)
            {
                var org = new Organization
                {
                    Id = Guid.NewGuid(),
                    Name = addOrgDto.Name,
                    CoverUrl = addOrgDto.CoverUrl,
                    Introduction = addOrgDto.Introduction,
                };
                var s = await _rmanagerService.OrganizationService.AddOrganization(org);
                return _mapper.Map<OrganizationDetailDto>(s);

            }
            else
            {
                throw new _401Exception($"{addOrgDto.Name} has already existed!");
            }
        }

        [HttpPost]
        [Route("EditOrg")]
        [Authorize(Roles=Roles.Admin)]
        public async ValueTask<OrganizationDetailDto> EditOrganization(EditOrgInfoDto editOrgInfoDto)
        {
            //要设置只允许admin修改自己所属的组织。
            var u=await _rmanagerService.OrganizationService.GetOrgByIdAsync(editOrgInfoDto.Id);
            if (u != null)
            {
                u.Introduction = editOrgInfoDto.Introduction;
                u.Name = editOrgInfoDto.Name;
                u.CoverUrl = editOrgInfoDto.CoverUrl;
                await _rmanagerService.OrganizationService.EditOrgInfoAsync(u);
                return _mapper.Map<OrganizationDetailDto>(u);
            }
            else
            {
                throw new _401Exception("Cannot find the Org!");
            }
            
        }

        /// <summary>
        /// Return users the org contains or throw a exception refers that this user are already in org
        /// </summary>
        /// <param name="addUserInOrgDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddUserInOrg")]
        [Authorize(Roles=Roles.Admin)]
        public async ValueTask<AddBelongDto> AddUserInOrgAsync(AddBelongDto addBelongDto)
        {
            await _rmanagerService.UserInOrgService.AddUserInOrgAsync(addBelongDto.id, addBelongDto.orgId);
            return addBelongDto;
        }

        [HttpPost]
        [Route("AddRoomInOrg")]
        [Authorize(Roles = Roles.Admin)]
        public async ValueTask<AddBelongDto> AddRoomInOrgAsync(AddBelongDto addBelongDto)
        {
            await _rmanagerService.RoomInOrgService.AddRoomInOrgAsync(addBelongDto.id, addBelongDto.orgId);
            return addBelongDto;
        }

        [HttpGet]
        [Route("GetUsersInOrg")]
        [AllowAnonymous] 
        public async ValueTask<List<Guid>> GetUsersInOrg(Guid orgId)
        {
            //这里要检查org是否存在嘛？
            var t = await _rmanagerService.UserInOrgService.GetUsersInOrgAsync(orgId);
            return t;
        }
        [HttpGet]
        [Route("GetRoomsInOrg")]
        [AllowAnonymous]
        public async ValueTask<List<Guid>> GetRoomsInOrg(Guid orgid)
        {
            var t = await _rmanagerService.RoomInOrgService.GetRoomsInOrgAsync(orgid);
           
            return t;
        }

    }
}
