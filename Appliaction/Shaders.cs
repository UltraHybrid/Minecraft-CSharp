using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace tmp
{
    public static class Shaders
    {
        private const string VertSrc = 
            @"#version 410 core

            layout (location = 0) in vec3 vert;
            layout (location = 1) in vec3 texCord;
            layout (location = 2) in vec3 position;
            layout (location = 3) in vec3 cubeTex1;
            layout (location = 4) in vec3 cubeTex2;

            uniform mat4 viewProjection;

            out vec3 TexCord;

            void main(void)
            {
                float layer;
                int id;
                id = int(texCord.p);

                if (id > 2)
                {
                    layer = float(cubeTex2[id - 3]);
                }
                else
                {
                    layer = float(cubeTex1[id]);
                }

                gl_Position = viewProjection * vec4(position + vert, 1.0);
                TexCord = vec3(texCord.st, layer);
            }";

        private const string FragSrc = 
            @"#version 410 core

            in vec3 TexCord;
            out vec4 outColor;

            uniform sampler2DArray tarr;

            void main(void)
            {
                vec4 tmp = texture(tarr, TexCord);
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