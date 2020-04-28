using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace Rmanager.Models
{


    public class Organization : Entity,ISearchAble
    {
        public string Name { get; set; }
        public string CoverUrl { get; set; }
        public string Introduction { get; set; }
        public string SearchAbleString { get; set; }
    }
}
