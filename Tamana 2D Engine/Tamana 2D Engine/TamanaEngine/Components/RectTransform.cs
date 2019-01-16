using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace TamanaEngine
{
    public class RectTransform : Component
    {
        private Vector2 _sizeDelta;       
        private Vector2 sizeDelta
        {
            get { return _sizeDelta; }
            set
            {
                _sizeDelta = value;
            }
        }
    }
}
