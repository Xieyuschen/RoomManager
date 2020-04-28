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
    public class OrganizationService:DBQueryService<Organization>
    {
        public OrganizationService(IDatabaseSettings settings)
           : base(settings, settings.OrganizationCollectionName)
        {
        }

        public async ValueTask<Guid> GetOrgIdByName(string name)
        {
            return (await (await collection.FindAsync(t => t.Name == name)).FirstOrDefaultAsync()).Id;
        }

        /// <summary>
        /// return a org with certain name or return null when the org with certain name isn't exist.
        /// Do not throw an exception here.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async ValueTask<Organization> GetOrgByName(string name)
        {
            try
            {
                return (await (await collection.FindAsync(t => t.Name == name)).FirstAsync());
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async ValueTask<Organization> AddOrganization(Organization organization)
        {
            await AddAsync(organization);
            return organization;
        }
        
        /// <summary>
        /// return the organization the guid specied or return null when the org is not exist.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async ValueTask<Organization> GetOrgByIdAsync(Guid guid)
        {
            try
            {
                return await (await collection.FindAsync(t => t.Id == guid)).FirstAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async ValueTask<Organization> EditOrgInfoAsync(Organization organization)
        {
             collection.UpdateOne(Builders<Organization>.Filter.Where(t=>t.Id==organization.Id), 
                Builders<Organization>.Update
                .Set(t => t.Name, organization.Name)
                .Set(t => t.Introduction, organization.Introduction)
                .Set(t => t.CoverUrl, organization.CoverUrl));
            return organization;
        }


    }
}

