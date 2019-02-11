using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace TamanaEngine
{
    public class Image : ComponentUI
    {
        private Core.GetModelMatrix model;
        private Core.UploadMatrixMVP uploadMatrixMVP;
        private Core.BindTexture bindTexture;

        private Core.Shader shader;

        private int VBO;
        private int VAO;

        private Sprite _sprite;
        public Sprite sprite
        {
            get { return _sprite; }
            set
            {
                _sprite = value;
                size = new Vector2(_sprite.rect.Width, _sprite.rect.Height);
                GetBindTextureDelegate();
                UploadNewVerticesToGPU();
            }
        }

        private Vector2 size;
        public bool raycastTarget { get; set; }
        public bool isMouseOverlap
        {
            get
            {
                if (!raycastTarget)
                    return false;

                var mousePosition = Input.mousePositionGL;
                var pos = transform.position;
                var halfSize = size / 2f;

                if (mousePosition.X < pos.X + halfSize.X && mousePosition.X > pos.X - halfSize.X &&
                    mousePosition.Y < pos.Y + halfSize.Y && mousePosition.Y > pos.Y - halfSize.Y)
                    return true;

                return false;
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

        private float[] vertices =
            {
                      .5f,  .5f,  1.0f, 1.0f, // top right
                      .5f, -.5f,  1.0f, 0.0f, // bottom right
                     -.5f, -.5f,  0.0f, 0.0f, // bottom left
                      
                      .5f,  .5f,  1.0f, 1.0f, // top right
                     -.5f, -.5f,  0.0f, 0.0f, // bottom left
                     -.5f,  .5f,  0.0f, 1.0f  // top left 
            };

        private void Awake()
        {
            shader = new Core.Shader("./res/TextRendererVertex.txt", "./res/TextRendererFragment.txt");
            sprite = new Sprite(new Texture2D(100, 100));
            size = new Vector2(100, 100);
            color = System.Drawing.Color.White;

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
                      size.X / 2f,  size.Y / 2f, 1.0f, 1.0f, // top right
                      size.X / 2f, -size.Y / 2f, 1.0f, 0.0f, // bottom right
                     -size.X / 2f, -size.Y / 2f, 0.0f, 0.0f, // bottom left
                                               
                      size.X / 2f,  size.Y / 2f, 1.0f, 1.0f, // top right
                     -size.X / 2f, -size.Y / 2f, 0.0f, 0.0f, // bottom left
                     -size.X / 2f,  size.Y / 2f, 0.0f, 1.0f  // top left 
                 };

            vertices = newVertices;

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);
        }

        private void GetModelMatrixDelegate()
        {
            model = Core.Util.CreateDelegate<Core.GetModelMatrix>("GetModelMatrix", transform,
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        }

        private void GetUploadMatrixMVPDelegate()
        {
            uploadMatrixMVP = Core.Util.CreateDelegate<Core.UploadMatrixMVP>("UploadMatrixUI", Camera.camera,
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        }

        private void GetBindTextureDelegate()
        {
            bindTexture = Core.Util.CreateDelegate<Core.BindTexture>("BindTexture", _sprite.texture,
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        }

        private void GenerateBuffer()
        {
            GL.GenBuffers(1, out VBO);
            GL.GenVertexArrays(1, out VAO);

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

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
