using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace TamanaEngine
{
    public class Text : ComponentUI
    {
        private Core.GetModelMatrix model;
        private Core.UploadMatrixMVP uploadMatrixMVP;
        private Core.BindTexture bindTexture;
        private Core.Shader shader;

        private Texture2D textTexture;

        private float[] vertices =
            {
                      .5f,  .5f,   1.0f, 1.0f, 1.0f,   1.0f, 1.0f, // top right
                      .5f, -.5f,   1.0f, 1.0f, 1.0f,   1.0f, 0.0f, // bottom right
                     -.5f, -.5f,   1.0f, 1.0f, 1.0f,   0.0f, 0.0f, // bottom left
                      
                      .5f,  .5f,   1.0f, 1.0f, 1.0f,   1.0f, 1.0f, // top right
                     -.5f, -.5f,   1.0f, 1.0f, 1.0f,   0.0f, 0.0f, // bottom left
                     -.5f,  .5f,   1.0f, 1.0f, 1.0f,   0.0f, 1.0f  // top left 
            };
        private Vector2 size;

        private string _text;
        public string text
        {
            get { return _text; }
            set
            {
                _text = value;
                if (string.IsNullOrEmpty(value))
                    return;

                var chars = new Texture2D[value.Length];
                for (int i = 0; i < value.Length; i ++)
                    chars[i] = Core.DefaultFont.GetTexture2DFromChar(value[i]);

                if (textTexture != null)
                    textTexture.Dispose();
                textTexture = new Texture2D(chars.Length * chars[0].width, chars[0].height);

                var offsetX = 0;
                for (int i = value.Length - 1; i >= 0; i --)
                {
                    for (int x = 0; x < chars[0].width; x++)
                        for(int y = 0; y < chars[0].height; y++)
                        {
                            textTexture.SetPixel(offsetX + x, y, chars[i].GetPixel(x, y));
                        }
                    offsetX += chars[0].width;
                }

                textTexture.Apply();

                var sizeX = textTexture.width / 2f;
                var sizeY = textTexture.height / 2f;

                size = new Vector2(sizeX, sizeY);

                UploadNewVerticesToGPU();
            }
        }

        private System.Drawing.Color _color;
        public System.Drawing.Color color
        {
            get { return _color; }
            set
            {
                _color = value;
                UploadNewVerticesToGPU();
            }
        }

        private int VBO;
        private int VAO;

        private void Awake()
        {

            shader = new Core.Shader("./res/SpriteRendererVertex.txt", "./res/SpriteRendererFragment.txt");

            textTexture = new Texture2D(100, 100);


            GetModelMatrixDelegate();
            GetUploadMatrixMVPDelegate();
            GetBindTextureDelegate();
            GenerateBuffer();
        }

        private void Render()
        {
            bindTexture.Invoke();

            shader.UseProgram();
            uploadMatrixMVP.Invoke(shader, model.Invoke());

            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
        }

        private void UploadNewVerticesToGPU()
        {
            float[] newVertices =
                {
                      size.X,  size.Y,   _color.R/255f, _color.G, _color.B,   1.0f, 1.0f, // top right
                      size.X, -size.Y,   _color.R/255f, _color.G, _color.B,   1.0f, 0.0f, // bottom right
                     -size.X, -size.Y,   _color.R/255f, _color.G, _color.B,   0.0f, 0.0f, // bottom left
                              
                      size.X,  size.Y,   _color.R/255f, _color.G, _color.B,   1.0f, 1.0f, // top right
                     -size.X, -size.Y,   _color.R/255f, _color.G, _color.B,   0.0f, 0.0f, // bottom left
                     -size.X,  size.Y,   _color.R/255f, _color.G, _color.B,   0.0f, 1.0f  // top left 
                 };

            vertices = newVertices;

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);
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

        private void GetBindTextureDelegate()
        {
            var method = textTexture.GetType().GetMethod("BindTexture",
               System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            if (method == null)
            {
                var methods = textTexture.GetType().GetMethods(
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                string foundedMethods = string.Empty;

                foreach (var m in methods)
                    foundedMethods += m.Name + "\n";

                throw new NullReferenceException("\nCannot find method 'BufferNewData'. Founded methood : \n" + foundedMethods);
            }

            bindTexture = method.CreateDelegate(typeof(Core.BindTexture), textTexture) as Core.BindTexture;
        }

        private void GenerateBuffer()
        {
            GL.GenBuffers(1, out VBO);
            GL.GenVertexArrays(1, out VAO);

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(float) * 7, 0 * sizeof(float));
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(float) * 7, 2 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, sizeof(float) * 7, 5 * sizeof(float));
            GL.EnableVertexAttribArray(2);
        }

        protected override void DestroyComponent()
        {
            throw new NotImplementedException();
        }
    }
}
