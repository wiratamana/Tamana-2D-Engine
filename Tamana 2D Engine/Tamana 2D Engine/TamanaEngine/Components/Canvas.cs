using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace TamanaEngine
{
    public class Canvas : Component
    {
        private Rectangle _rect;
        public Rectangle rect { get { return _rect; } }

        protected override void DestroyComponent()
        {
            throw new NotImplementedException();
        }

        private void Awake()
        {
            _rect = new Rectangle(0, 0, Screen.width, Screen.height);
        }
    }
}
