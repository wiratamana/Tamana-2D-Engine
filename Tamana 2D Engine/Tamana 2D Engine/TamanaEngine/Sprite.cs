using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

using OpenTK.Graphics.OpenGL;

namespace TamanaEngine.Core
{
    delegate void BufferNewData(ref float[] data);
}

namespace TamanaEngine
{
     public class Sprite : IDisposable
    {
        public RectangleF rect { private set; get; }
        public Texture2D texture { private set; get; }

        public Sprite(Texture2D texture2D)
        {
            texture = texture2D;
            rect = new RectangleF(0, 0, texture.width, texture.height);
        }

        public Sprite(string spriteFile)
        {
            texture = new Texture2D(100, 100);
            rect = new RectangleF(0, 0, texture.width, texture.height);
        }

        public void Dispose()
        {
            texture.Dispose();
            texture = null;
        }
    }
}
