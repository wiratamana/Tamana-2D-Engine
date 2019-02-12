using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Imaging;

namespace TamanaEngine.Core
{
    public static class Util
    {
        public static T CreateDelegate<T>(string methodName, object classObj, System.Reflection.BindingFlags flags) where T : Delegate
        {
            var method = classObj.GetType().GetMethod(methodName, flags);

            if (method == null)
            {
                var methods = classObj.GetType().GetMethods(flags);

                string foundedMethods = string.Empty;

                foreach (var m in methods)
                    foundedMethods += m.Name + "\n";

                throw new NullReferenceException(string.Format("\nCannot find method {0}. Founded methood : \n", methodName) + foundedMethods);
            }

            return method.CreateDelegate(typeof(T), classObj) as T;
        }

        public static Sprite[] GifToSpriteArray(string gifFile)
        {
            System.Drawing.Image gif = System.Drawing.Image.FromFile(gifFile);
            FrameDimension dimension = new FrameDimension(gif.FrameDimensionsList[0]);
            int frames = gif.GetFrameCount(dimension);
            Sprite[] sprites = new Sprite[frames];

            for (int i = 0; i < frames; i++)
            {
                Bitmap resultingImage = new Bitmap(gif.Width, gif.Height);

                gif.SelectActiveFrame(dimension, i);

                Rectangle destRegion = new Rectangle(0, 0, gif.Width, gif.Height);
                Rectangle srcRegion = new Rectangle(0, 0, gif.Width, gif.Height);

                using (Graphics grD = Graphics.FromImage(resultingImage))
                {
                    grD.DrawImage(gif, destRegion, srcRegion, GraphicsUnit.Pixel);
                }

                resultingImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                var tex2D = new Texture2D(gif.Width, gif.Height);
                for (int x = 0; x < tex2D.width; x++)
                    for (int y = 0; y < tex2D.height; y++)
                    {
                        tex2D.SetPixel(x, y, resultingImage.GetPixel(x, y));
                    }
                tex2D.Apply();
                sprites[i] = new Sprite(tex2D);
                resultingImage.Dispose();
            }

            return sprites;
        }
    }
}
