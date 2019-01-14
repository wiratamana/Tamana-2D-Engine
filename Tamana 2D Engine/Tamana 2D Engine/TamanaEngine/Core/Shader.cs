using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using OpenTK.Graphics.OpenGL;

namespace TamanaEngine.Core
{
    public class Shader
    {
        private int vertexShader;
        private int fragmentShader;

        private int program;

        public Shader(string pathVert, string pathFrag)
        {
            if (!File.Exists(pathVert) && !File.Exists(pathFrag))
                throw new FileNotFoundException("Shader couldn't be found.");

            string vert = File.ReadAllText(pathVert);
            string frag = File.ReadAllText(pathFrag);

            vertexShader = GL.CreateShader(ShaderType.VertexShader);
            fragmentShader = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(vertexShader, vert);
            GL.ShaderSource(fragmentShader, frag);

            GL.CompileShader(vertexShader);
            GL.CompileShader(fragmentShader);

            if (!string.IsNullOrEmpty(GL.GetShaderInfoLog(vertexShader)))
                throw new Exception("Failed to compile vertex shader : \n" + GL.GetShaderInfoLog(vertexShader));
            if (!string.IsNullOrEmpty(GL.GetShaderInfoLog(fragmentShader)))
                throw new Exception("Failed to compile fragment shader");

            program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);

            GL.LinkProgram(program);
            if (!string.IsNullOrEmpty(GL.GetProgramInfoLog(program)))
                throw new Exception("Failed to link program");

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public void UseProgram()
        {
            GL.UseProgram(program);
        }
    }
}
