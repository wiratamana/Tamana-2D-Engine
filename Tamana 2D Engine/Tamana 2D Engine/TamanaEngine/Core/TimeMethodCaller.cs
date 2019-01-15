using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamanaEngine.Core
{
    public static class TimeMethodCaller
    {
        private static SetDeltaTime setDeltaTime;

        public static void InvokeSetDeltaTime(float deltaTime)
        {
            setDeltaTime.Invoke(deltaTime);
        }

        public static void GetSetDeltaTimeDelegate()
        {
            var time = Type.GetType("TamanaEngine.Time");
            if (time == null)
                throw new NullReferenceException();
            else Console.WriteLine(time.Name);

            var setDeltaTimeMethod = time.GetMethod("SetDeltaTime",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy);

            if (setDeltaTimeMethod == null)
            {
                var methods = time.GetType().GetMethods(
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

                string foundedMethods = string.Empty;

                foreach (var m in methods)
                    foundedMethods += m.Name + "\n";

                throw new NullReferenceException("\nCannot find method 'SetDeltaTime'. Founded methood : \n" + foundedMethods);
            }

            setDeltaTime = setDeltaTimeMethod.CreateDelegate(typeof(SetDeltaTime)) as SetDeltaTime;
        }
    }
}
