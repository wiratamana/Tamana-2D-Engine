using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamanaEngine
{
    public class Text : ComponentUI
    {
        private Sprite graphicString;

        private Core.GetModelMatrix model;
        private Core.UploadMatrixMVP uploadMatrixMVP;
        private Core.BufferNewData bufferNewData;
        private Core.Shader shader;

        private string _text;
        public string text
        {
            get { return _text; }
            set
            {
                _text = value;
                if (string.IsNullOrEmpty(value))
                    return;

                var chars = new System.Drawing.Bitmap[value.Length];
                for (int i = 0; i < value.Length; i ++)
                    chars[i] = Core.DefaultFont.GetVertexDataFromChar(value[i]);

                var bigBitmap = new System.Drawing.Bitmap(chars.Length * chars[0].Width, chars[0].Height);
                var lockBitmap = new Core.LockBitmap(bigBitmap);
                lockBitmap.LockBits();
                var offsetX = 0;
                for (int i = 0; i < value.Length; i ++)
                {
                    var lockBitmapChar = new Core.LockBitmap(chars[i]);
                    lockBitmapChar.LockBits();

                    for (int x = 0; x < chars[0].Width; x++)
                        for(int y = 0; y < chars[0].Height; y++)
                        {
                            lockBitmap.SetPixel(offsetX + x, y, lockBitmapChar.GetPixel(x, y));
                        }

                    lockBitmapChar.UnlockBits();
                    offsetX += chars[0].Width;
                }

                lockBitmap.UnlockBits();
                bigBitmap.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

                graphicString?.Destroy();
                graphicString = new Sprite(bigBitmap);
                GetBufferNewDataDelegate();

                var sizeX = bigBitmap.Width / 2f;
                var sizeY = bigBitmap.Height / 2f;

                float[] vertices =
                {
                      sizeX,  sizeY,   1.0f, 1.0f, 1.0f,   1.0f, 1.0f, // top right
                      sizeX, -sizeY,   1.0f, 1.0f, 1.0f,   1.0f, 0.0f, // bottom right
                     -sizeX, -sizeY,   1.0f, 1.0f, 1.0f,   0.0f, 0.0f, // bottom left
                              
                      sizeX,  sizeY,   1.0f, 1.0f, 1.0f,   1.0f, 1.0f, // top right
                     -sizeX, -sizeY,   1.0f, 1.0f, 1.0f,   0.0f, 0.0f, // bottom left
                     -sizeX,  sizeY,   1.0f, 1.0f, 1.0f,   0.0f, 1.0f  // top left 
                 };

                bufferNewData.Invoke(vertices);
            }
        }

        private void Awake()
        {
            graphicString = new Sprite();
            shader = new Core.Shader("./res/SpriteRendererVertex.txt", "./res/SpriteRendererFragment.txt");

            GetModelMatrixDelegate();
            GetUploadMatrixMVPDelegate();
            GetBufferNewDataDelegate(); 
        }

        private void Render()
        {
            graphicString.BindTexture();
            
            shader.UseProgram();
            uploadMatrixMVP.Invoke(shader, model.Invoke());
            graphicString.RenderSprite();
        }

        private void GetModelMatrixDelegate()
        {
            var modelMatrix = transform.GetType().GetMethod("GetModelMatrix",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.IgnoreReturn);

            if (modelMatrix == null)
            {
                var methods = transform.GetType().GetMethods(
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.IgnoreReturn);

                string foundedMethods = string.Empty;

                foreach (var m in methods)
                    foundedMethods += m.Name + "\n";

                throw new NullReferenceException("\nCannot find method 'GetModelMatrix'. Founded methood : \n" + foundedMethods);
            }

            model = modelMatrix.CreateDelegate(typeof(Core.GetModelMatrix), transform) as Core.GetModelMatrix;
        }

        private void GetUploadMatrixMVPDelegate()
        {
            var mainCamera = GameObject.FindObjectOfType<Camera>();
            if (mainCamera == null)
                throw new NullReferenceException("Could not find Camera component.");

            var method = mainCamera.GetType().GetMethod("UploadMatrixMVP",
               System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            if (method == null)
            {
                var methods = mainCamera.GetType().GetMethods(
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                string foundedMethods = string.Empty;

                foreach (var m in methods)
                    foundedMethods += m.Name + "\n";

                throw new NullReferenceException("\nCannot find method 'GetModelMatrix'. Founded methood : \n" + foundedMethods);
            }

            uploadMatrixMVP = method.CreateDelegate(typeof(Core.UploadMatrixMVP), mainCamera) as Core.UploadMatrixMVP;
        }

        private void GetBufferNewDataDelegate()
        {
            var method = graphicString.GetType().GetMethod("BufferNewData",
               System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            if (method == null)
            {
                var methods = graphicString.GetType().GetMethods(
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                string foundedMethods = string.Empty;

                foreach (var m in methods)
                    foundedMethods += m.Name + "\n";

                throw new NullReferenceException("\nCannot find method 'BufferNewData'. Founded methood : \n" + foundedMethods);
            }

            bufferNewData = method.CreateDelegate(typeof(Core.BufferNewData), graphicString) as Core.BufferNewData;
        }
    }
}
