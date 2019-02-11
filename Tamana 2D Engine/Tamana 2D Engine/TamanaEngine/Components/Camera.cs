using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace TamanaEngine
{
    public class Camera : Component
    {
        private static Matrix4 projectionOrtho
        { get { return Matrix4.CreateOrthographic(Screen.width, Screen.height, .1f, 60f); } }
        private static Matrix4 projectionPerspective
        { get { return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60f), 16f / 9f, .1f, 100f); } }

        private static Camera _camera;
        public static Camera camera
        {
            get
            {
                if (_camera == null)
                    _camera = GameObject.FindObjectOfType<Camera>();

                return _camera;
            }
        }

        private readonly Matrix4 projectionUI = Matrix4.LookAt(new Vector3(0, 0, -10), new Vector3(0,0,-9), new Vector3(0, 1, 0));

        private Matrix4 view;

        private void UploadMatrixMVP(Core.Shader shader, Matrix4 modelMatrix)
        {
            view = Matrix4.LookAt(transform.position, transform.position + transform.forward, transform.up);          

            shader.SetMatrix4("matrixMVP", modelMatrix * view * projectionOrtho);
        }

        private void UploadMatrixUI(Core.Shader shader, Matrix4 modelMatrix)
        {
            shader.SetMatrix4("matrixMVP", modelMatrix * projectionUI * projectionOrtho);
        }

        protected override void DestroyComponent()
        {
            
        }
    }
}
