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

        public static OpenTK.Vector2 mousePosition
        {
            get
            {
                var state = Mouse.GetCursorState();
                return new OpenTK.Vector2(state.X, state.Y);
            }
        }

        public static OpenTK.Vector2 mousePositionWindow
        {
            get
            {
                var state = Mouse.GetCursorState();
                return new OpenTK.Vector2(state.X - ((1920 / 2) - (Screen.width / 2)), state.Y - ((1080 / 2) - (Screen.height / 2)));
            }
        }

        public static OpenTK.Vector2 mousePositionGL
        {
            get
            {
                var state = Mouse.GetCursorState();
                return new OpenTK.Vector2(state.X -  (1920 / 2), state.Y -  (1080 / 2));
            }
        }
    }
}
