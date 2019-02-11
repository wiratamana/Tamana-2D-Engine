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
            Bitmap spriteData = new Bitmap(spriteFile);
            spriteData.RotateFlip(RotateFlipType.Rotate180FlipNone);
            texture = new Texture2D(spriteData.Width, spriteData.Height);
            rect = new RectangleF(0, 0, texture.width, texture.height);

            for (int x = 0; x < texture.width; x++)
            {
                for(int y = 0; y < texture.height; y++)
                {
                    texture.SetPixel(x, y, spriteData.GetPixel(x, y));
                }
            }

            texture.Apply();

            spriteData.Dispose();
        }

        public void Dispose()
        {
            texture.Dispose();
            texture = null;
        }
    }
}
