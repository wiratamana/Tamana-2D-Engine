using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Input;

namespace TamanaEngine.Core
{
    delegate OpenTK.Vector2 MousePosition();
}

namespace TamanaEngine
{
    public static class Input
    {
        private static Core.MousePosition _mousePosition;

        public static bool GetKey(Key key)
        {
            var state = Keyboard.GetState();
            return state.IsKeyDown(key);
        }

        public static OpenTK.Vector2 mousePosition
        {
            get
            {
                var mouse = Mouse.GetCursorState();
                return new OpenTK.Vector2(mouse.X, mouse.Y);
            }
        }
    }
}
