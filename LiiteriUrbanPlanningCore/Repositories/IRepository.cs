using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace LiiteriUrbanPlanningCore.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        IEnumerable<T> FindAll(Queries.ISqlQuery query);

        T Single(Queries.ISqlQuery query);

        T First(Queries.ISqlQuery query);
    } 
}