using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace tmp
{
    public static class Shaders
    {
        private const string VertSrc = 
            @"#version 410 core

            void main(void)
            {
                gl_Position = vec4(-0.25, 0.25, 0.5, 1.0);
            }";

        private const string FragSrc = 
            @"#version 410 core

            out vec4 outColor;

            void main(void)
            {
                outColor = vec4(1.0, 1.0, 0.0, 1.0);
            }";

        public static int InitShaders()
        {
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, VertSrc);
            GL.CompileShader(vertexShader);

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, FragSrc);
            GL.CompileShader(fragmentShader);

            var shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);
            GL.LinkProgram(shaderProgram);

            GL.DetachShader(shaderProgram, vertexShader);
            GL.DetachShader(shaderProgram, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            return shaderProgram;
        }
    }
}