using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data.Common;

namespace LiiteriUrbanPlanningCore.Repositories
{
    public abstract class SqlRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbConnection dbConnection;

        public SqlRepository(DbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public IEnumerable<TEntity> FindAll(
            Queries.ISqlQuery query,
            Factories.IFactory factory)
        {
            var entityList = new List<TEntity>();

            using (DbCommand cmd = this.dbConnection.CreateCommand()) {
                cmd.CommandText = query.GetQueryString();

                foreach (KeyValuePair<string, object> param in
                        query.Parameters) {
                    cmd.Parameters.Add(
                        new SqlParameter(param.Key, param.Value));
                }

                using (DbDataReader rdr = cmd.ExecuteReader()) {
                    while (rdr.Read()) {
                        TEntity p = (TEntity) factory.Create(rdr);
                        entityList.Add(p);
                    }
                }
            }

            return entityList;
        }

        public abstract IEnumerable<TEntity> GetAll();

        public abstract IEnumerable<TEntity> FindAll(Queries.ISqlQuery query);

        public abstract TEntity Single(Queries.ISqlQuery query);

        public abstract TEntity First(Queries.ISqlQuery query);
    }
}