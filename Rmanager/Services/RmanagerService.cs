using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Rmanager.Models;
using Rmanager.Services;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using Rmanager.Exceptions;
using System.Security.Claims;

namespace Rmanager.Services
{
    public class RmanagerService
    {
        public UserInOrgService UserInOrgService{ get; }
        public RoomInOrgService RoomInOrgService { get; }
        public OrganizationService OrganizationService { get; }
        public RoomService RoomService { get; }
        public UserService UserService { get; }
        public BookingRoomService BookingRoomService { get; }
        public List<string> AllowedEmail { get; set; }
        IHostEnvironment env { get; }
        public RmanagerService(IDatabaseSettings settings, IHostEnvironment env, IMapper mapper)
        {
            
            BsonSerializer.RegisterIdGenerator(typeof(Guid), GuidGenerator.Instance);
            try
            {
                OrganizationService = new OrganizationService(settings);
                RoomService = new RoomService(settings);
                UserService = new UserService(settings);
                UserInOrgService = new UserInOrgService(settings);
                RoomInOrgService = new RoomInOrgService(settings);
                BookingRoomService = new BookingRoomService(settings);
                AllowedEmail = new List<string>();
                this.env = env;
            }
            catch(Exception e)
            {
                throw new ServiceStartUpException(e.Message);
            }
        }
        public void CheckUserAuth(ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated && !env.IsDevelopment())
            {
                throw new _403Exception();
            }
        }
        public void CheckAdminAutu(ClaimsPrincipal user)
        {
            if (!env.IsDevelopment() && !user.IsInRole(Roles.Admin))
            {
                throw new _403Exception();
            }
        }
        public void CheckGrillenAuth(ClaimsPrincipal user)
        {
            if (!env.IsDevelopment() && !user.IsInRole(Roles.Grillen))
            {
                throw new _403Exception();
            }
        }

    }
}
