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
                      .5f,  .5f,  1.0f, 1.0f, // top right
                      .5f, -.5f,  1.0f, 0.0f, // bottom right
                     -.5f, -.5f,  0.0f, 0.0f, // bottom left
                      
                      .5f,  .5f,  1.0f, 1.0f, // top right
                     -.5f, -.5f,  0.0f, 0.0f, // bottom left
                     -.5f,  .5f,  0.0f, 1.0f  // top left 
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

        private Vector4 _color;
        public System.Drawing.Color color
        {
            get { return System.Drawing.Color.FromArgb((int)_color.X * 255, (int)_color.Y * 255, (int)_color.Z * 255, (int)_color.W * 255); }
            set
            {
                _color = new Vector4(1, value.R / 255f, value.G / 255f, value.B / 255f);
            }
        }

        private int VBO;
        private int VAO;

        private void Awake()
        {
            shader = new Core.Shader("./res/TextRendererVertex.txt", "./res/TextRendererFragment.txt");

            textTexture = new Texture2D(100, 100);

            uploadMatrixMVP = CreateDelegate<Core.UploadMatrixMVP>("UploadMatrixMVP", GameObject.FindObjectOfType<Camera>(),
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
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
            shader.SetVec3("color", new Vector3(_color.Y, _color.Z, _color.W));

            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
        }

        private void UploadNewVerticesToGPU()
        {
            float[] newVertices =
                {
                      size.X,  size.Y,  1.0f, 1.0f, // top right
                      size.X, -size.Y,  1.0f, 0.0f, // bottom right
                     -size.X, -size.Y,  0.0f, 0.0f, // bottom left
                              
                      size.X,  size.Y,  1.0f, 1.0f, // top right
                     -size.X, -size.Y,  0.0f, 0.0f, // bottom left
                     -size.X,  size.Y,  0.0f, 1.0f  // top left 
                 };

            vertices = newVertices;

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.DynamicDraw);
        }

        private void GetModelMatrixDelegate()
        {
            model = CreateDelegate<Core.GetModelMatrix>("GetModelMatrix", transform,
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        }

        private void GetUploadMatrixMVPDelegate()
        {           
            uploadMatrixMVP = CreateDelegate<Core.UploadMatrixMVP>("UploadMatrixUI", GameObject.FindObjectOfType<Camera>(),
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        }

        private void GetBindTextureDelegate()
        {
            bindTexture = CreateDelegate<Core.BindTexture>("BindTexture", textTexture,
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        }

        private T CreateDelegate<T>(string methodName, object classObj, System.Reflection.BindingFlags flags) where T : Delegate
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

        private void GenerateBuffer()
        {
            GL.GenBuffers(1, out VBO);
            GL.GenVertexArrays(1, out VAO);

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0 * sizeof(float));
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 4, 2 * sizeof(float));
            GL.EnableVertexAttribArray(1);
        }

        protected override void DestroyComponent()
        {
            throw new NotImplementedException();
        }
    }
}
