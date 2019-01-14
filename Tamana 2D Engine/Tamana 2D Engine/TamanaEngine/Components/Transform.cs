using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace TamanaEngine.Core
{
    delegate Matrix4 GetModelMatrix();
    delegate void UploadMatrixMVP(Shader shader, Matrix4 matrixMVP);
}

namespace TamanaEngine
{
    public class Transform : Component
    {
        private Vector3 _position;
        public Vector3 position
        {
            get { return _position; }
            set
            {
                _position = value;

                model = Matrix4.CreateTranslation(value);
            }
        }

        public Vector3 forward { get { return Vector3.Normalize((position + new Vector3(0, 0, 1)) - position); } }
        public Vector3 right { get { return Vector3.Normalize((position + new Vector3(1, 0, 0)) - position); } }
        public Vector3 up { get { return Vector3.Normalize((position + new Vector3(0, 1, 0)) - position); } }

        private Matrix4 model;
        private Matrix4 GetModelMatrix()
        {
            return model;
        }

        private void Awake()
        {
            position = Vector3.Zero;
        }
    }
}