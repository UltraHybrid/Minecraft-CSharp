using OpenTK.Graphics.OpenGL4;

namespace MinecraftSharp.Shaders
{
    public static class Shaders
    {
        private const string VertSrcCube =
            @"#version 410 core

            layout (location = 0) in vec3 vert;
            layout (location = 1) in vec3 texCord;
            layout (location = 2) in vec3 position;
            layout (location = 3) in vec3 cubeTex[2];

            uniform mat4 viewProjection;

            out vec3 TexCord;

            void main(void)
            {
                float layer;
                int id;
                id = int(texCord.p);

                if (id > 2)
                {
                    layer = float(cubeTex[1][id - 3]);
                }
                if (id < 3)
                {
                    layer = float(cubeTex[0][id]);
                }

                gl_Position = viewProjection * vec4(position + vert, 1.0);
                TexCord = vec3(texCord.st, layer);
            }";

        private const string FragSrcCube =
            @"#version 410 core

            in vec3 TexCord;
            out vec4 outColor;

            uniform sampler2DArray tarr;

            void main(void)
            {
                vec4 MinecraftSharp = texture(tarr, TexCord);
                if(MinecraftSharp.a < 0.1)
                    discard;
                
                outColor = MinecraftSharp;
            }";

        private const string VertSrcSide =
            @"#version 410 core

            layout (location = 0) in vec3 position;
            layout (location = 1) in vec3 sideTex;
            layout (location = 2) in vec3 vert[6];
            layout (location = 8) in vec2 texCord[6];

            uniform mat4 viewProjection;

            out vec3 color;
            out float light;

            void main(void)
            {
                int sideID = int(sideTex.x);
                gl_Position = viewProjection * vec4(position + vert[sideID], 1.0);
                color = vec3(texCord[sideID], sideTex.y);
                light = sideTex.z;
            }";

        private const string FragSrcSide =
            @"#version 410 core

            in vec3 color;
            in float light;

            uniform sampler2DArray tarr;

            void main(void)
            {
                vec4 MinecraftSharp = texture(tarr, color);
                if(MinecraftSharp.a < 0.1)
                    discard;
                
                gl_FragColor = MinecraftSharp * light;
            }";

        private const string VertSkyBox =
            @"#version 410 core

            layout (location = 0) in vec3 aPos;
            out vec3 TexCoords;

            uniform mat4 projection;

            void main()
            {
                TexCoords = aPos;
                vec4 pos = projection * vec4(aPos, 1.0);
                gl_Position = pos.xyww;
            }";

        private const string FragSkyBox =
            @"#version 410 core

            in vec3 TexCoords;

            uniform samplerCube skybox;

            void main()
            {    
                gl_FragColor = texture(skybox, TexCoords);
            }";
        
        private const string VertLine =
            @"#version 410 core

            layout (location = 0) in vec3 position;
            layout (location = 1) in vec3 color;

            out vec4 Color;

            uniform mat4 viewProjection;

            void main()
            {
                Color = vec4(color, 1.0);
                vec4 pos = viewProjection * vec4(position, 1.0);
                gl_Position = pos;
            }";

        private const string FragLine =
            @"#version 410 core

            in vec4 Color;

            void main()
            {    
                gl_FragColor = Color;
            }";
        
        private const string VertDefault =
            @"#version 410 core
            layout (location = 0) in vec3 vertex;
            layout (location = 1) in vec2 tex;
            layout (location = 2) in mat4 transform;

            uniform mat4 viewProjection;
            
            out vec2 Tex;

            void main()
            {
                Tex = tex;
                gl_Position = viewProjection * transform * vec4(vertex, 1.0);
            }";

        private const string FragDefault =
            @"#version 410 core

            in vec2 Tex;
            uniform sampler2D tarr;

            void main()
            {    
                gl_FragColor = texture(tarr, Tex);
            }";

        public static int GetLineShader() => InitShaders(VertLine, FragLine);
        
        public static int GetDefaultShader() => InitShaders(VertDefault, FragDefault);

        public static int GetSkyBoxShader() => InitShaders(VertSkyBox, FragSkyBox);

        public static int GetSideShader() => InitShaders(VertSrcSide, FragSrcSide);

        private static int InitShaders(string vert, string frag)
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