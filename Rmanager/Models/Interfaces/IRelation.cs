using System;
namespace Rmanager.Models
{
    interface IRelation
    {
        Guid RelationId { get; set; }
        Guid UserId { get; set; }
    }
}
