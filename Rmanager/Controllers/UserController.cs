using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rmanager.Services;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using Rmanager.Dto;
using Rmanager.Models;
using MongoDB.Driver;
using Rmanager.Exceptions;
namespace Rmanager.Controllers
{
    //2.9问题？
    //1. User，HttpContext这些东西在Microsoft.AspNet.Mvc,这些东西要代表了什么东西？
    //
    //关于登陆状态的想法：
    //只要进入这个网页就默认是匿名登陆，这个登录不需要用户操作，可以做匿名操作的所有事情。查看个人信息直接报错，进入到登陆界面去
    //然后登录的时候全部按照用户登陆模式，检查用户名密码进行登录
    //这样的话就要改一下设计了
    
    //现在的问题是：
    //如何判断用户的登陆状态？应该是可以用User.Idetity.Name访问cookie来查看
    //这样就可以进行设计了，那就需要把api改一下
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        
        readonly RmanagerService _rmanagerService;
        readonly IMapper _mapper;
        IHostEnvironment env;
        public UserController(RmanagerService rmanagerService,IMapper mapper,IHostEnvironment env)
        {
            _rmanagerService = rmanagerService;
            _mapper = mapper;
            this.env = env;
        }
        
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async ValueTask<UserDetailDto> GetUser(string id=null)
        {
            var Id = Guid.Parse(id);
            var item=await _rmanagerService.UserService.GetUserByIdAsync(Id);
            var user = _mapper.Map<UserDetailDto>(item);
            return user;
        }

        [HttpPost]
        [Route("newUser")]
        [Authorize(Roles = Roles.Admin)]
        public async ValueTask<UserDetailDto> Register(AddUserDto addUserDto)
        {

            
                if(addUserDto.Name.ToUpper()== "ANNOYMOUS")
                {
                    throw new Exception("Illegal Name!");
                }
                var user = new RmanagerUser
                {
                    Name = addUserDto.Name,
                    Email = addUserDto.Email,
                    PassWordHash = addUserDto.Password,
                    Roles = new List<string>() { "User" },
                };
                //AddUserAsync方法中已经有对Email是否注册过的检查
                var t =await _rmanagerService.UserService.AddUserAsync(user);
                return _mapper.Map<UserDetailDto>(t);
        }

        //修改用户信息
        [HttpPost]
        [Route("EditUser")]
        [Authorize(Roles = Roles.User)]
        public async ValueTask<UserDetailDto> EditUser(UserEditInfoDto userEditInfoDto)
        {
            //这里userid是直接获取还是Dto提供？
            //测试可以改别人的信息，要重新写一下。
            _rmanagerService.CheckUserAuth(HttpContext.User);
            if (userEditInfoDto.id !=null)
            {
                var user = await _rmanagerService.UserService.GetUserByIdAsync(userEditInfoDto.id);
                user.AvatarUrl = userEditInfoDto.AvatarUrl;
                user.Name = userEditInfoDto.Name;
                user.Describe = userEditInfoDto.Describe;
                await _rmanagerService.UserService.EditUserInfo(user);
                return _mapper.Map<UserDetailDto>(user);
            }
            else
            {
                throw new _401Exception("Haven't the authorization!");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("LogIn")]
        public async ValueTask<UserDetailDto> LogInAsync(LogInDto logInDto)
        {
            var user = _mapper.Map<RmanagerUser>(logInDto);
            if (!User.Identity.IsAuthenticated && string.IsNullOrEmpty(logInDto.Email))
            {
                //匿名方式登录，返回一个匿名user
                var u=await _rmanagerService.UserService.SignInAnonymously(HttpContext);
                return _mapper.Map<UserDetailDto>(u);
            }
            if (HttpContext.User.Identity.IsAuthenticated && string.IsNullOrEmpty(logInDto.Email))
            {
                return _mapper.Map<UserDetailDto>(await _rmanagerService.UserService.GetUserByEmailAsync(logInDto.Email)); 
            }
            user.PassWordHash = logInDto.PassWord;
            await _rmanagerService.UserService.SignInAsync(user, HttpContext, logInDto.RememberMe);
            var t = await _rmanagerService.UserService.GetUserByEmailAsync(logInDto.Email);
            return _mapper.Map<UserDetailDto>(t);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AddGrillen")]
        public async ValueTask<UserDetailDto> AddGrillen()
        {
            var user = new RmanagerUser
            {
                Name = "Grillen",
                Email = "2016231075@qq.com",
                PassWordHash ="sonnigundwarm2020,#",
                Roles = new List<string>() { "Admin","Grillen","User" }
            };
            //AddUserAsync方法中已经有对Email是否注册过的检查
            var t = await _rmanagerService.UserService.AddUserAsync(user);
            return _mapper.Map<UserDetailDto>(t);
        }

        /// <summary>
        /// Authorize a new admin by Grillen,
        /// if this email is already exist,we will treat it as admin and ignore info left in the addUserDto
        /// </summary>
        /// <param name="addUserDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddAdmin")]
        [Authorize(Roles=Roles.Grillen)]
        public async ValueTask<UserDetailDto> AddAdmin(AddUserDto addUserDto)
        {
            if (addUserDto.Name.ToUpper() == "ANNOYMOUS")
            {
                throw new Exception("Illegal Name!");
            }
            var temp = await _rmanagerService.UserService.GetUserByEmailAsync(addUserDto.Email);
            if (temp==null)
            {
                var user = new RmanagerUser
                {
                    Name = addUserDto.Name,
                    Email = addUserDto.Email,
                    PassWordHash = addUserDto.Password,
                    Roles = new List<string>() { Roles.User, Roles.Admin },
                };
                //AddUserAsync方法中已经有对Email是否注册过的检查
                var t = await _rmanagerService.UserService.AddUserAsync(user);
                return _mapper.Map<UserDetailDto>(t);
            }
            else
            {
                List<string> ro=new List<string>() { Roles.Admin,Roles.User};
                temp=await _rmanagerService.UserService.EditUserRole(temp, ro);
                return _mapper.Map<UserDetailDto>(temp);
            }
            
        }
        [HttpPost]
        [Route("LogOut")]
        public async ValueTask<UserDetailDto> LogOutAsync()
        {
            _rmanagerService.CheckUserAuth(User);
            return _mapper.Map<UserDetailDto>(await _rmanagerService.UserService.SignOutAsync(HttpContext));
        }
    }
}
