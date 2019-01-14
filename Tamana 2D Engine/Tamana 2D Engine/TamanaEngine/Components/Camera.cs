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
        { get { return Matrix4.CreateOrthographic(Core.Screen.width, Core.Screen.height, .1f, 60f); } }
        
        private Matrix4 view;

        private void Update()
        {
            view = Matrix4.LookAt(transform.position, transform.forward, transform.up);
        }

        private void UploadMatrixMVP(Core.Shader shader, Matrix4 modelMatrix)
        {
            shader.SetMatrix4("matrixMVP", modelMatrix * view * projectionOrtho);
        }
    }
}
