using Rmanager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rmanager.Exceptions;
using MongoDB.Driver;
using EzPasswordValidator.Validators;
using Rmanager.Dto;
using Microsoft.AspNetCore.Http;
using HashLibrary;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Rmanager.Services
{
    
    public class UserService : DBQueryService<RmanagerUser>
    {
        public UserService(IDatabaseSettings settings)
            : base(settings, settings.UserCollectionName)
        {           //var client = new MongoClient(settings.ConnectionString);

        }
        /// <summary>
        /// 返回的user中PasswordHash是存在数据库中的密码值，已经被hash过，并不是用户输入的密码
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async ValueTask<RmanagerUser> AddUserAsync(RmanagerUser user)
        {
            var validater = new PasswordValidator();
            validater.SetLengthBounds(8, 20);
            validater.AddCheck(EzPasswordValidator.Checks.CheckTypes.Letters);
            validater.AddCheck(EzPasswordValidator.Checks.CheckTypes.Numbers);
            if (!validater.Validate(user.PassWordHash))
            {
                throw new _401Exception("Password is not strong enough!");
            }
            
            if(string.IsNullOrEmpty(user.Email)||
               string.IsNullOrEmpty(user.Name)||
               string.IsNullOrEmpty(user.PassWordHash))
            {
                throw new _401Exception("register data should not be null!");
            }
            
            await CheckEmailAndNameAsync(user);
            user.IsEmailConfirmed = false;
            user.AvatarUrl = "";
            
            //这里，添加之后返回的user密码已经被hash过
            var a = HashLibrary.HashedPassword.New(user.PassWordHash);
            user.PassWordHash = a.Hash + a.Salt;
            await AddAsync(user);
            return user;
        }
        private async ValueTask CheckEmailAndNameAsync(RmanagerUser user)
        {
            if (!collection.Find(t=>true).Any())
            {
                return;
            }
            var a =await collection.FindAsync(f => f.Email == user.Email);
            var s =await a.FirstOrDefaultAsync();
            if (s != null)
            {
                throw new _401Exception("This email has already registered!");
            }

        }
        public async ValueTask<Guid> GetUserIdByEmailAsync(string email)
        {
            return (await (await collection.FindAsync(t => t.Email == email)).FirstOrDefaultAsync()).Id;
        }
        /// <summary>
        /// Find User by email,return User it finds or return null as not found 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async ValueTask<RmanagerUser> GetUserByEmailAsync(string email)
        {
            try
            {
                return (await (await collection.FindAsync(t => t.Email == email)).FirstAsync());
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Find User by id,return User it finds or return null as not found 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async ValueTask<RmanagerUser> GetUserByIdAsync(Guid id)
        {
            try
            {
                return (await (await collection.FindAsync(p => p.Id == id)).FirstOrDefaultAsync());
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async ValueTask<RmanagerUser> EditUserInfo(RmanagerUser user)
        {
            var a =await (await collection.FindAsync(t => t.Id == user.Id)).FirstOrDefaultAsync();
            a.AvatarUrl = user.AvatarUrl;
            a.Name = user.Name;
            await collection.UpdateOneAsync(Builders<RmanagerUser>.Filter.Where(t => t.Id == user.Id),
                Builders<RmanagerUser>.Update.Set<string>(u => u.AvatarUrl, user.AvatarUrl)
                .Set(u => u.ExtraInformation, user.ExtraInformation)
                .Set(u=>u.Name,user.Name)
                );
            return user;
        }
        public async ValueTask<RmanagerUser> EditUserRole(RmanagerUser user,List<string> t)
        {
            user.Roles = t;
            await collection.UpdateOneAsync(Builders<RmanagerUser>.Filter.Where(t => t.Id == user.Id),
                Builders<RmanagerUser>.Update.Set(u => u.Roles, t));
            return user;
        }
        //public async ValueTask<RmanagerUser> CheckEmailAndPassword(LogInDto logInDto)
        //{
        //    var user =await GetUserByEmailAsync(logInDto.Email);
        //    if (user.PassWordHash != logInDto.PassWord)
        //    {
        //        throw new _401Exception("Email or password are wrong,please input again!");
        //    }
        //    else
        //    {
        //        return user;
        //    }
        //}

        public async ValueTask<RmanagerUser> SignInAnonymously(HttpContext httpContext)
        {
            var user = new RmanagerUser
            {
                Name = "anonymous",
                Id = Guid.NewGuid(),
                Email = Guid.NewGuid().ToString(),
                Roles = new List<string>() { "anonymous" },
                PassWordHash = "12345678a"
            };
            
            await AddUserAsync(user);

            //这里，很重要。否则会导致哈希之前的密码已经和数据库中的相同
            user.PassWordHash = "12345678a";
            await SignInAsync(user, httpContext);
            return user;
        }
        public async ValueTask<bool> SignInAsync(RmanagerUser user, HttpContext httpContext, bool rememberMe = true, bool validPassword = true)
        {
            var u = await GetUserByEmailAsync(user.Email);
            if (u == null)
            {
                throw new _400Exception("Cannot find the Email!");
            }
            bool auth = true;
            if (validPassword)
            {
                var hash = u.PassWordHash.Substring(0, 32);
                var salt = u.PassWordHash.Substring(32);
                var h = new HashedPassword(hash, salt);
                auth = h.Check(user.PassWordHash);
            }
            if (auth)
            {
                var authProperties = new AuthenticationProperties
                {
                    //there are many properties in class AuthenticationProperties
                    IsPersistent = rememberMe
                };

                //这一块是干嘛的？？
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, u.Email),
                    new Claim(ClaimTypes.Name, u.Id.ToString()),
                };
                for (int i = 0; i < u.Roles.Count; i++)
                {
                    claims.Add(new Claim(ClaimTypes.Role, u.Roles[i]));
                }
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var c = new ClaimsPrincipal();
                
                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);
                return true;
            }
            else
            {

                throw new _401Exception("Password and email do not match!");
            }
        }
        public async ValueTask<RmanagerUser> SignOutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return await SignInAnonymously(httpContext);
        }
    }
}


