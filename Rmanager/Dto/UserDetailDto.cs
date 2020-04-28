using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rmanager.Models;
namespace Rmanager.Dto
{
    //2.12
    //在dto和user中加上该用户所属的组织
    public class UserDetailDto
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public List<string> Roles { get; set; }
        public List<Guid> BelongOrg { get; set; }
    }
    public class AddUserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password {get;set;}
    }
    public class LogInDto
    {
        public string Email { get; set; }
        public string PassWord { get; set; }
        public bool RememberMe { get; set; }
    }
    public class UserEditInfoDto
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public string Describe { get; set; }
    }

}
