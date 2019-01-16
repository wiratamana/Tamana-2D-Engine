using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

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

                if (_sprite != null)
                    _sprite.Destroy();

                _sprite = null;

                _sprite = value;
                GetEverythig();
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

                 float[] vertices =
                 {
                      value.X,  value.Y,   1.0f, 1.0f, 1.0f,   1.0f, 1.0f, // top right
                      value.X, -value.Y,   1.0f, 1.0f, 1.0f,   1.0f, 0.0f, // bottom right
                     -value.X, -value.Y,   1.0f, 1.0f, 1.0f,   0.0f, 0.0f, // bottom left
                      
                      value.X,  value.Y,   1.0f, 1.0f, 1.0f,   1.0f, 1.0f, // top right
                     -value.X, -value.Y,   1.0f, 1.0f, 1.0f,   0.0f, 0.0f, // bottom left
                     -value.X,  value.Y,   1.0f, 1.0f, 1.0f,   0.0f, 1.0f  // top left 
                 };

                bufferNewData.Invoke(vertices);
            }
        }

        private Core.GetModelMatrix model;
        private Core.UploadMatrixMVP uploadMatrixMVP;
        private Core.BufferNewData bufferNewData;
        private Core.Shader shader;

        public SpriteRenderer()
        {
            shader = new Core.Shader("./res/SpriteRendererVertex.txt", "./res/SpriteRendererFragment.txt");
            _sprite = new Sprite("./res/sprite.png");
        }

        private void Awake()
        {
            GetEverythig();
        }

        private void Render()
        {
            sprite.BindTexture();

            shader.UseProgram();
            uploadMatrixMVP.Invoke(shader, model.Invoke());
            sprite.RenderSprite();
        }

        private void GetEverythig()
        {
            GetModelMatrixDelegate();
            GetUploadMatrixMVPDelegate();
            GetBufferNewDataDelegate();

            size = new Vector2(sprite.rect.Width, sprite.rect.Height);
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
            var method = sprite.GetType().GetMethod("BufferNewData",
               System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            if (method == null)
            {
                var methods = method.GetType().GetMethods(
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                string foundedMethods = string.Empty;

                foreach (var m in methods)
                    foundedMethods += m.Name + "\n";

                throw new NullReferenceException("\nCannot find method 'BufferNewData'. Founded methood : \n" + foundedMethods);
            }

            bufferNewData = method.CreateDelegate(typeof(Core.BufferNewData), sprite) as Core.BufferNewData;
        }
    }
}
