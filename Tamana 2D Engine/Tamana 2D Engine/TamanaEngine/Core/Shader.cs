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
                throw new Exception("Failed to link program. \nReason : \n" + GL.GetProgramInfoLog(program));   

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public void UseProgram()
        {
            GL.UseProgram(program);
        }

        public void SetMatrix4(string uniformName, OpenTK.Matrix4 matrix4)
        {
            GL.UniformMatrix4(GL.GetUniformLocation(program, uniformName), false, ref matrix4);
        }

        public void SetInt(string uniformName, int value)
        {
            GL.Uniform1(GL.GetUniformLocation(program, uniformName), value);
        }

        public void SetVec3(string uniformName, OpenTK.Vector3 value)
        {
            GL.Uniform3(GL.GetUniformLocation(program, uniformName), value);
        }

        public void SetVec2(string uniformName, OpenTK.Vector2 value)
        {
            GL.Uniform2(GL.GetUniformLocation(program, uniformName), value);
        }
    }
}
