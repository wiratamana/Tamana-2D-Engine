using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamanaEngine.Core
{
    public static class Util
    {
        public static T CreateDelegate<T>(string methodName, object classObj, System.Reflection.BindingFlags flags) where T : Delegate
        {
            var method = classObj.GetType().GetMethod(methodName, flags);

            if (method == null)
            {
                var methods = classObj.GetType().GetMethods(flags);

                string foundedMethods = string.Empty;

                foreach (var m in methods)
                    foundedMethods += m.Name + "\n";

                throw new NullReferenceException(string.Format("\nCannot find method {0}. Founded methood : \n", methodName) + foundedMethods);
            }

            return method.CreateDelegate(typeof(T), classObj) as T;
        }
    }
}
