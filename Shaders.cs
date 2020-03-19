using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace tmp
{
    public static class Shaders
    {
        private const string VertSrc = 
            @"#version 410 core

            layout (location = 0) in vec3 position;
            layout (location = 1) in vec2 texCoord;

            uniform mat4 model;
            uniform mat4 view;
            uniform mat4 projection;

            out vec2 TexCoord;

            void main(void)
            {
                gl_Position = projection * view * model * vec4(position, 1.0);
                TexCoord = texCoord;
            }";

        private const string FragSrc = 
            @"#version 410 core
            
            in vec2 TexCoord;
            out vec4 outColor;

            uniform sampler2D ourTexture;

            void main(void)
            {
                vec4 tmp = texture(ourTexture, TexCoord);  
                if(tmp.a < 0.1)
                    discard;
                outColor = tmp;
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