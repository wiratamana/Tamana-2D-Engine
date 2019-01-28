using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

using TamanaEngine.Core;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace TamanaEngine.Core
{
    public delegate void BindTexture();
}

namespace TamanaEngine
{
    public class Texture2D : IDisposable
    {
        private int _textureID;
        public int width { private set; get; }
        public int height { private set; get; }

        private int colorDepth;
        private int colorStep;
        private BitmapData bmData;
        private IntPtr pointer;

        private byte[] colorsByte;

        private bool wasDisposed;

        public Texture2D(int width, int height)
        {
            this.width = width;
            this.height = height;

            GenerateTexture();
        }

        ~Texture2D()
        {
            Console.WriteLine("This is just a destructor!");
            if (!wasDisposed) Dispose();
        }

        public Color GetPixel(int x, int y)
        {
            Color clr = Color.Empty;

            // Get start index of the specified pixel
            int i = ((y * width) + x) * colorStep;

            if (i > colorsByte.Length - colorStep)
                throw new IndexOutOfRangeException();

            if (colorDepth == 32) // For 32 bpp get Red, Green, Blue and Alpha
            {
                byte b = colorsByte[i];
                byte g = colorsByte[i + 1];
                byte r = colorsByte[i + 2];
                byte a = colorsByte[i + 3]; // a
                clr = Color.FromArgb(a, r, g, b);
            }
            if (colorDepth == 24) // For 24 bpp get Red, Green and Blue
            {
                byte b = colorsByte[i];
                byte g = colorsByte[i + 1];
                byte r = colorsByte[i + 2];
                clr = Color.FromArgb(r, g, b);
            }
            if (colorDepth == 8)
            // For 8 bpp get color value (Red, Green and Blue values are the same)
            {
                byte c = colorsByte[i];
                clr = Color.FromArgb(c, c, c);
            }
            return clr;
        }
        public void SetPixel(int x, int y, Color color)
        {
            // Get start index of the specified pixel
            int i = ((y * width) + x) * colorStep;

            if (colorDepth == 32) // For 32 bpp set Red, Green, Blue and Alpha
            {
                colorsByte[i] = color.B;
                colorsByte[i + 1] = color.G;
                colorsByte[i + 2] = color.R;
                colorsByte[i + 3] = color.A;
            }
            else if (colorDepth == 24) // For 24 bpp set Red, Green and Blue
            {
                colorsByte[i] = color.B;
                colorsByte[i + 1] = color.G;
                colorsByte[i + 2] = color.R;
            }
            else if (colorDepth == 8)
            // For 8 bpp set color value (Red, Green and Blue values are the same)
            {
                colorsByte[i] = color.B;
            }
        }
        public void Apply()
        {
            Marshal.Copy(colorsByte, 0, pointer, colorsByte.Length);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, pointer);
        }

        private void BindTexture()
        {
            GL.BindTexture(TextureTarget.Texture2D, _textureID);
        }

        private void GenerateTexture()
        {
            GL.GenTextures(1, out _textureID);
            GL.BindTexture(TextureTarget.Texture2D, _textureID);

            var bmp = new Bitmap(width, height);
            bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);

            try
            {
                int PixelCount = width * height;

                Rectangle rect = new Rectangle(0, 0, width, height);

                colorDepth = Bitmap.GetPixelFormatSize(bmp.PixelFormat);

                if (colorDepth != 8 && colorDepth != 24 && colorDepth != 32)
                {
                    throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
                }

                bmData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

                colorStep = colorDepth / 8;
                colorsByte = new byte[PixelCount * colorStep];
                pointer = bmData.Scan0;

                Marshal.Copy(pointer, colorsByte, 0, colorsByte.Length);
            }
            catch (Exception ex) { throw ex; }

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    SetPixel(x, y, Color.White);
                }

            Apply();         

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        }

        public void Dispose()
        {
            GL.DeleteTexture(_textureID);
            bmData = null;
            pointer = IntPtr.Zero;
            colorsByte = null;

            wasDisposed = true;
        }
    }
}
