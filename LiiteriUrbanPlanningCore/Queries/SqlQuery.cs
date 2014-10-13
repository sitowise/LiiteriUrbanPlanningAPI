using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiiteriUrbanPlanningCore.Queries
{
    public interface ISqlQuery
    {
        IEnumerable<KeyValuePair<string, object>> Parameters { get; }
        string GetQueryString();
    }

    public abstract class SqlQuery : ISqlQuery
    {
        public Dictionary<string, object> Parameters {get; set;}

        IEnumerable<KeyValuePair<string, object>> ISqlQuery.Parameters
        {
            get { return Parameters;  }
        }

        public SqlQuery()
        {
            this.Parameters = new Dictionary<string, object>();
        }

        public void AddParameter(string key, object value)
        {
            this.Parameters[key] = value;
        }

        public object GetParameter(string key)
        {
            if (!this.Parameters.ContainsKey(key)) {
                return null;
            }
            return this.Parameters[key];
        }

        public string[] PushParameters(string prefix, int[] parameters)
        {
            string[] paramNames = parameters.Select(
                    (s, i) => "@" + prefix + "_" + i.ToString()
                ).ToArray();
            for (int i = 0; i < paramNames.Length; i++) {
                this.AddParameter(paramNames[i], parameters[i]);
            }

            return paramNames;
        }

        public string[] PushParameters(string prefix, string[] parameters)
        {
            string[] paramNames = parameters.Select(
                    (s, i) => "@" + prefix + "_" + i.ToString()
                ).ToArray();
            for (int i = 0; i < paramNames.Length; i++) {
                this.AddParameter(paramNames[i], parameters[i]);
            }

            return paramNames;
        }

        public abstract string GetQueryString();
    }
}