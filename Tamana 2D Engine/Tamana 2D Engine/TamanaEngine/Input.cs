using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Input;

namespace TamanaEngine
{
    public static class Input
    {
        public static bool GetKey(Key key)
        {
            var state = Keyboard.GetState();
            return state.IsKeyDown(key);
        }
    }
}
