using Rmanager.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rmanager.Services
{
    /// <summary>
    /// Class whu=ich provide basic function for database query
    /// can be inherit by sepecific database service class.
    /// </summary>
    /// <typeparam name="T">Database Class</typeparam>
    public class DBQueryService<T> : LimFx.Business.Services.DBQueryServices<T> where T : Entity, ISearchAble
    {
        public DBQueryService(IDatabaseSettings settings, string collectionName)
            : base(settings.ConnectionString,settings.DatabaseName,collectionName) { }
        //如果想自定义，在下方重载重载继承的方法
    }
}
