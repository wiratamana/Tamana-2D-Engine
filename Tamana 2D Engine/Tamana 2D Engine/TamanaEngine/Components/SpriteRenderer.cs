using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace TamanaEngine
{
    public class SpriteRenderer : Component
    {
        private Sprite _sprite;
        public Sprite sprite
        {
            get { return _sprite; }
            set
            {
                if (value == null)
                    return;

                _sprite.texture.Dispose();
                _sprite = value;
                GetBindTextureDelegate();
            }
        }

        private Vector2 _size;
        public Vector2 size
        {
            get { return _size; }
            set
            {
                if (value.X < 0)
                    value.X = 0;

                if (value.Y < 0)
                    value.X = 0;

                value.X /= 2f;
                value.Y /= 2f;
                _size = value;


                myVertices[0] = value.X;
                myVertices[1] = value.Y;

                myVertices[7] = value.X;
                myVertices[8] = -value.Y;

                myVertices[14] = -value.X;
                myVertices[15] = -value.Y;

                myVertices[21] = value.X;
                myVertices[22] = value.Y;

                myVertices[28] = -value.X;
                myVertices[29] = -value.Y;

                myVertices[35] = -value.X;
                myVertices[36] = value.Y;

                IntPtr vertPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(myVertices.Length * sizeof(float));
                System.Runtime.InteropServices.Marshal.Copy(myVertices, 0, vertPtr, myVertices.Length);

                GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
                GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * myVertices.Length, vertPtr, BufferUsageHint.StaticDraw);

                System.Runtime.InteropServices.Marshal.FreeHGlobal(vertPtr);
            }
        }

        private Core.GetModelMatrix model;
        private Core.UploadMatrixMVP uploadMatrixMVP;
        private Core.BindTexture bindTexture;
        private Core.Shader shader;

        private int VBO;
        private int VAO;

        private static readonly float[] vertices =
        {
             100.5f,  100.5f,   1.0f, 1.0f, 1.0f,   1.0f, 1.0f, // top right
             100.5f, -100.5f,   1.0f, 1.0f, 1.0f,   1.0f, 0.0f, // bottom right
            -100.5f, -100.5f,   1.0f, 1.0f, 1.0f,   0.0f, 0.0f, // bottom left
                      
             100.5f,  100.5f,   1.0f, 1.0f, 1.0f,   1.0f, 1.0f, // top right
            -100.5f, -100.5f,   1.0f, 1.0f, 1.0f,   0.0f, 0.0f, // bottom left
            -100.5f,  100.5f,   1.0f, 1.0f, 1.0f,   0.0f, 1.0f  // top left 
        };

        private float[] myVertices;

        private void Awake()
        {
            myVertices = vertices;

            shader = new Core.Shader("./res/SpriteRendererVertex.txt", "./res/SpriteRendererFragment.txt");
            _sprite = new Sprite("./res/sprite.png");            

            GetEverything();
            GenerateBuffer();

            size = new Vector2(sprite.rect.Width, sprite.rect.Height);
        }

        private void Render()
        {
            bindTexture.Invoke();

            shader.UseProgram();
            uploadMatrixMVP.Invoke(shader, model.Invoke());

            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, myVertices.Length);
        }

        private void GetEverything()
        {
            GetModelMatrixDelegate();
            GetUploadMatrixMVPDelegate();
            GetBindTextureDelegate();
        }

        private void GenerateBuffer()
        {
            GL.GenBuffers(1, out VBO);
            GL.GenVertexArrays(1, out VAO);

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
            var method = sprite.texture.GetType().GetMethod("BindTexture",
               System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            if (method == null)
            {
                var methods = sprite.texture.GetType().GetMethods(
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                string foundedMethods = string.Empty;

                foreach (var m in methods)
                    foundedMethods += m.Name + "\n";

                throw new NullReferenceException("\nCannot find method 'BufferNewData'. Founded methood : \n" + foundedMethods);
            }

            bindTexture = method.CreateDelegate(typeof(Core.BindTexture), sprite.texture) as Core.BindTexture;
        }

        protected override void DestroyComponent()
        {
            throw new NotImplementedException();
        }
    }
}
