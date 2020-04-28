# SuperAdmin
```csharp
//SuperAdmin对象可以添加admin成员
//用户名、密码与邮箱
//成功添加返回admin对象，失败时throw一个错误返回一个空对象。
admin Addadmin(string username,string password,string email);
```
### 导入Excel
```csharp
//接受一个组织名称org |要导入Excel的路径 excelpath
bool ImportExcel(string org,string excelpath);
//将在 person的拥有的组织列表中查找org，如果找到的话就把excel里面的成员添加到org类中的list<user>
//若没有这个组织，那么就创建新组织将excel中的数据导入。
```

# Admin
### 添加user
```csharp
//创建成功则返回一个user对象，失败则返回空对象并throw异常。
user Adduser(string username,string password,string email);
void Deleteuser(string UserId,stirng email)
```
### 管理组织Org
- 暂时只需要创建和编辑组织的信息，不允许删除组织
```csharp
//返回一个Org对象，封面采用默认白图片，之后修改
Org AddOrg(string orgname);
//修改Org的名称
Org EditOrgName(string OrgId,string neworgname);
//修改Org的封面
Org EditOrgCover(string OrgId,string CoverUrl);
```
### 管理房间Room
```csharp
//创建房间的时候只需要输入房间号（关键信息）
Room AddRoom(stirng RoomNumber);
//
Room EditRoom(string RoomId,
              stirng CoverUrl,
              string description);
```

### 对组织中成员管理
```csharp
User AddUserToOrg(string OrgId,string UserId);
User DeleteUserFromOrg(string OrgId,UserId);
```
### 对组织中房间的管理
```csharp
Room AddRoomToOrg(string OrgId,string UserId);
Room DeleteUserFromOrg(string OrgId,stirng UserId);
```




# User
User 只需要订购房间
```Csharp
//开始不允许用户修改定房间的信息，如果想要改就取消再重新来
Room BookRoom(string RoomId,string UserId,string begin,string Endtiem);
Room CancelBook(string RoomId,string UserId);
```