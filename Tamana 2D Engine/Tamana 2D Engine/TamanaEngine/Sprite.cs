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
    delegate void BufferNewData(float[] data);
}

namespace TamanaEngine
{
     public class Sprite
    {
        public RectangleF rect { private set; get; }

        private int VAO;
        private int VBO;

        private int id;

        private static readonly float[] vertices =
        {
             0.5f,  0.5f,   1.0f, 1.0f, 1.0f,   1.0f, 1.0f, // top right
             0.5f, -0.5f,   1.0f, 1.0f, 1.0f,   1.0f, 0.0f, // bottom right
            -0.5f, -0.5f,   1.0f, 1.0f, 1.0f,   0.0f, 0.0f, // bottom left

             0.5f,  0.5f,   1.0f, 1.0f, 1.0f,   1.0f, 1.0f, // top right
            -0.5f, -0.5f,   1.0f, 1.0f, 1.0f,   0.0f, 0.0f, // bottom left
            -0.5f,  0.5f,   1.0f, 1.0f, 1.0f,   0.0f, 1.0f  // top left 
        };

        private float[] myVertices;

        public Sprite()
        {
            myVertices = vertices;
            GenerateObject();
            LoadTexture("");
        }

        public Sprite(string path)
        {
            myVertices = vertices;
            GenerateObject();
            LoadTexture(path);
        }

        public Sprite(Bitmap bitmap)
        {
            myVertices = vertices;
            GenerateObject();

            rect = new RectangleF(0, 0, bitmap.Width, bitmap.Height);

            GL.GenTextures(1, out id);
            GL.BindTexture(TextureTarget.Texture2D, id);

            BitmapData data = bitmap.LockBits(
                 new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                 ImageLockMode.ReadOnly,
                 System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        }          

        public void BindTexture()
        {
            GL.BindTexture(TextureTarget.Texture2D, id);
        }

        public void RenderSprite()
        {
            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
        }

        private void GenerateObject()
        {
            GL.GenVertexArrays(1, out VAO);
            GL.GenBuffers(1, out VBO);

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * myVertices.Length, myVertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(float) * 7, 0 * sizeof(float));
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(float) * 7, 2 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, sizeof(float) * 7, 5 * sizeof(float));
            GL.EnableVertexAttribArray(2);
        }

        private void BufferNewData(float[] data)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * data.Length, data, BufferUsageHint.StaticDraw);
        }

        private void LoadTexture(string path)
        {
            GL.GenTextures(1, out id);
            GL.BindTexture(TextureTarget.Texture2D, id);

            Bitmap bmp = null;
            if (path == string.Empty)
                bmp = CreateBitmap();
            else
            {
                if (!File.Exists(path))
                    bmp = CreateBitmap();
                else bmp = LoadBitmap(path);
            }

            rect = new RectangleF(0, 0, bmp.Width, bmp.Height);

            BitmapData data = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bmp.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        }

        private Bitmap LoadBitmap(string path)
        {
            var bmp = new Bitmap(path);
            bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
            return bmp;
        }

        private Bitmap CreateBitmap()
        {
            Bitmap bmp = new Bitmap(100, 100);            
            Core.LockBitmap lockBitmap = new Core.LockBitmap(bmp);            
            lockBitmap.LockBits();
            for(int x = 0; x < bmp.Width; x++)
                for(int y = 0; y < bmp.Height; y++)
                {
                    lockBitmap.SetPixel(x, y, Color.White);
                }

            lockBitmap.UnlockBits();

            bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);

            return bmp;
        }

        public void Destroy()
        {
            GL.DeleteBuffer(VBO);
            GL.DeleteTexture(id);
            GL.DeleteVertexArray(VAO);

            myVertices = null;
        }
    }
}
