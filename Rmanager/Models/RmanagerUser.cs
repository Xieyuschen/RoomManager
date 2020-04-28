using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rmanager.Models
{
    
    public class RmanagerUser : Entity,ISearchAble
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public List<string> Roles { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string PassWordHash { get; set; }
        public string Describe { get; set; }
        public string SearchAbleString { get; set; }

    }
}


