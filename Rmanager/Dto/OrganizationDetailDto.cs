using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rmanager.Models;
namespace Rmanager.Dto
{
    public class OrganizationDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CoverUrl { get; set; }
        public string Introduction { get; set; }
        public List<Guid> users { get; set; }
        public List<Guid> rooms { get; set; }
    }
    public class AddOrgDto
    {
        //name必填
        public string Name { get; set; }
        public string CoverUrl { get; set; }
        public string Introduction { get; set; }
    }
    public class EditOrgInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CoverUrl { get; set; }
        public string Introduction { get; set; }
    }
    public class AddBelongDto
    {
        public Guid id { get; set; }
        public Guid orgId { get; set; }
    }
}
