using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace TamanaEngine.Core
{
    public class VertexBufferObject
    {
        private int VBO;

        public VertexBufferObject()
        {
            GL.GenBuffers(1, out VBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
        }
    }
}
