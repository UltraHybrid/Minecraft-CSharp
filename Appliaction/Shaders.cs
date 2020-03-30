using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace tmp
{
    public static class Shaders
    {
        private const string VertSrc = 
            @"#version 410 core

            layout (location = 0) in vec3 vert;
            layout (location = 1) in vec2 texCord;
            layout (location = 2) in vec3 position;

            uniform mat4 model;
            uniform mat4 view;
            uniform mat4 projection;

            out vec2 TexCord;

            void main(void)
            {
                gl_Position = projection * view * model * vec4(position + vert, 1.0);
                TexCord = texCord;
            }";

        private const string FragSrc = 
            @"#version 410 core

            in vec2 TexCord;
            out vec4 outColor;

            uniform sampler2DArray texture_array;

            void main(void)
            {
                vec4 tmp = texture(texture_array, vec3(TexCord, 1));
                if(tmp.a < 0.1)
                    discard;
                outColor = tmp;
            }";

        private const string VertSkyBox = 
            @"#version 410 core

            layout (location = 0) in vec3 aPos;
            out vec3 TexCoords;

            uniform mat4 projection;
            uniform mat4 view;

            void main()
            {
                TexCoords = aPos;
                vec4 pos = projection * view * vec4(aPos, 1.0);
                gl_Position = pos.xyww;
            }";

        private const string FragSkyBox =
            @"#version 410 core
            out vec4 FragColor;

            in vec3 TexCoords;

            uniform samplerCube skybox;

            void main()
            {    
                FragColor = texture(skybox, TexCoords);
            }";

        public static int GetDefaultShader() => InitShaders(VertSrc, FragSrc);

        public static int GetSkyBoxShader() => InitShaders(VertSkyBox, FragSkyBox);
        public static int InitShaders(string vert, string frag)
        {
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vert);
            GL.CompileShader(vertexShader);

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, frag);
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