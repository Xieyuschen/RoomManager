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
    public class UserInOrgService:DBQueryService<UserInOrg>
    {
        public UserInOrgService(IDatabaseSettings settings)
            : base(settings, settings.UserInOrgCollectionName){ }

        public async ValueTask<List<Guid>> GetUsersInOrgAsync(Guid orgId)
        {
            var Set = (await collection.FindAsync(t => t.orgId == orgId)).ToList();
            List<Guid> userlist = new List<Guid>();
            foreach (var item in Set)
            {
                userlist.Add(item.userId);
            }
            return userlist;
        }
        /// <summary>
        /// Return a list contains orgs to which user belongs.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async ValueTask<List<Guid>> GetOrgUserBelongsAsync(Guid guid)
        {
            var Set = (await collection.FindAsync(t => t.userId == guid)).ToList();
            List<Guid> orglist = new List<Guid>();
            foreach (var item in Set)
            {
                orglist.Add(item.orgId);
            }
            return orglist;
        }

        /// <summary>
        /// Add a user in an org and return a UserInOrg object.
        /// Check if userid and orgid exist to avoid adding a invaild info!
        /// Have already checked if the user belongs to this org.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public async ValueTask<UserInOrg> AddUserInOrgAsync(Guid userId,Guid orgId)
        {
            var flag = (await collection.FindAsync(t => t.userId == userId && t.orgId == orgId)).Any();
            if (flag)
            {
                throw new _401Exception("This User has been in this org already!");
            }
            else
            {
                UserInOrg userInOrg = new UserInOrg() { Id=Guid.NewGuid(),userId = userId, orgId = orgId };
                await AddAsync(userInOrg);
                return userInOrg;
            }
        }

    }
}
