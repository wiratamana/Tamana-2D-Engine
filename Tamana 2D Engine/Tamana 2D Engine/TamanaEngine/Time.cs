using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TamanaEngine.Core
{
    delegate void SetDeltaTime(float deltaTime);
}

namespace TamanaEngine
{
    public static class Time
    {
        private static float _deltaTime;
        public static float deltaTime { get { return _deltaTime; } }

        private static float _time;
        public static float time { get { return _time; } }

        private static void SetDeltaTime(float deltaTime)
        {
            _deltaTime = deltaTime;
            _time += deltaTime;
        }
    }
}
