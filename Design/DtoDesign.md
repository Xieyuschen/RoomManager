# Dto 设计
## UserDetailDto
1. AddUserByAdminDto
```csharp
public class AddUserByAdminDto
    {
        //这里不允许管理员添加admin|superadmin，所以说只能添加User
        //emmmm，
        public enum Role { User = 0}
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Role> Roles { get; set; }
    }
```
2. UserEditInfoDto
```csharp
public class UserEditInfoDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public string Affiliation { get; set; }
    }
```
3. AddUserToOrgDto
```csharp
    public class  AddUserToOrgDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
```



&emsp;
&emsp;
## RoomDetailsDto
1. AddRoomByAdminDto
```csharp
public class AddRoomByAdminDto
    {
        public string Number { get; set; }
        public string Description { get; set; }
    }
```

2. EditRoomInfoDto
```csharp
public class EditRoomInfoDto
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public string CoverUrl { get; set; }
    }
```
## Organization
1. AddOrgByAdminDto
```csharp
    public class AddOrgByAdminDto
    {
        public string Name { get; set; }
    }
```
2. EditOrgInfoDto
```csharp
    public class EditOrgInfoDto
    {
        public string Name { get; set; }
        public List<Guid> MemberIds { get; set; }
        public string CoverUrl { get; set; }
        public List<Guid> RoomIds { get; set; }
    }
```

## EditThingInOrgDto
1. EditUserInOrgDto
```csharp
//在Organization中删除与加入User都只需要名字（暂时认为没有重名）。
public class EditUserInOrgDto
{
    public string Username { get; set; }
    public string Orgname { get; set; }
}
```
2. EidtRoomInOrgDto
```csharp
public class EidtRoomInOrgDto
{
    public string Roomnumber { get; set; }
    public string Orgname { get; set; }
}
```