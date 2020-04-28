# 数据库设计  
所有数据库内的model继承同一个基类：  
Entity  
```csharp  
public class Entity
{
    public Entity()
    {
        Id = System.Guid.NewGuid();
        ExtraInformation = new Dictionary<string, object>();
        CreateTime = DateTime.UtcNow;
        UpdateTime = CreateTime;
        IsDeleted = false;
    }
    [BsonRepresentation(MongoDB.Bson.BsonType.Binary)]
    public Guid Id { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime DeleteTime { get; set; }

    public string EntityType { get; protected set; }

    //附加信息
    public Dictionary<string, object> ExtraInformation { get; set; }

    [BsonElement("CreateTime")]
    public DateTime CreateTime { get; }

    public DateTime UpdateTime { get; private set; }

    public void Update()
    {
        UpdateTime = DateTime.UtcNow;
    }
}
```  

## User  
```csharp  
public class User : Entity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string AvatarUrl { get; set; }
    public string Affiliation { get; set; }
    public List<string> Roles { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public string PassWordHash { get; set; }
}
```  
## Org  
```csharp
public class Oranization: Entity
{
    public string Name { get; set; }
    public List<Guid> MemberIds{ get; set; }
    public string CoverUrl{get;set;}
    public List<Guid> RoomIds{get;set;}
}
```  
## Room  
```csharp
public class Room: Entity
{
    public int RoomNumber{get;set;}
    public string Description { get; set; }
    public string CoverUrl{get;set;}
}
```  
## Book  
> 注意：
* 用户只能订阅自己属于的组织的房间
* 不同用户对同一房间的订阅不能有时间重合
```csharp
public class Book: Entity
{
    public Guid UserId{get;set;}
    public Guid RoomId{get;set;}
    public DateTime BookTime{get;set;}
    public DateTime EndTime{get;set;}
}
```