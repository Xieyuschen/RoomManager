using MongoDB.Bson.Serialization.Attributes;

namespace Rmanager.Models
{
    public interface IOrderAble
    {
        [BsonIgnore]
        int Power { get; set; }
    }
}
