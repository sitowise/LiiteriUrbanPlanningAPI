using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Diagnostics;

using System.Web.Http.ModelBinding;
using System.Web.Http.Controllers;

namespace LiiteriUrbanPlanningAPI.Binders
{
    public class StringArrayModelBinder : IModelBinder
    {
        public StringArrayModelBinder()
        {
        }

        public bool BindModel(
            HttpActionContext actionContext,
            ModelBindingContext bindingContext)
        {
            string key = bindingContext.ModelName;
            var val = bindingContext.ValueProvider.GetValue(key);
            if (val == null) {
                return false;
            }
            string s = val.AttemptedValue as string;
            if (s == null) {
                return false;
            }

            string[] vals = val.AttemptedValue.Split(',');
            bindingContext.Model = vals;

            return true;
        }
    }
}
