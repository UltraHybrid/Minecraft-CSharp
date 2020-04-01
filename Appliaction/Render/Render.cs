using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace tmp
{
    public class Render
    {
        #region Variables

        //shaders
        private int shaderProgram, skyBoxShaderProgram;

        //buffers
        private int vao, vbo, ebo, textureBuffer, position;

        private int vaoS, vboS, texture, texId, arrayTex;

        private int projectionMatrixAttributeLocation;

        private int viewMatrixAttributeLocationS;
        private int projectionMatrixAttributeLocationS;

        private Matrix4 viewMatrix;
        private Matrix4 projectionMatrix, viewProjectionMatrix;

        private readonly List<int> texturesId = new List<int>();
        private readonly List<Vector3> vertex = new List<Vector3>();
        private readonly List<Vector3> positions = new List<Vector3>();
        private readonly List<int> indices = new List<int>();
        private readonly List<Vector3> texCords = new List<Vector3>();

        private readonly World world;
        private readonly Camera camera;

        #endregion

        public Render(Camera camera, World world)
        {
            this.world = world;
            this.camera = camera;
        }

        public void RenderFrame()
        {
            ClearBackground(Color4.White);

            GL.Enable(EnableCap.PolygonSmooth);
            GL.Enable(EnableCap.CullFace);

            RenderWorld();
            RenderSkyBox();
        }

        private void RenderWorld()
        {
            GL.UseProgram(shaderProgram);

            viewMatrix = camera.GetViewMatrix();
            viewProjectionMatrix = viewMatrix * projectionMatrix;
            GL.UniformMatrix4(projectionMatrixAttributeLocation, false, ref viewProjectionMatrix);

            GL.BindTexture(TextureTarget.Texture2DArray, arrayTex);
            GL.BindVertexArray(vao);
            GL.DrawElementsInstanced(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedInt, IntPtr.Zero,
                cubesCount);
            GL.BindVertexArray(0);
        }

        private void RenderSkyBox()
        {
            GL.DepthFunc(DepthFunction.Lequal);
            GL.UseProgram(skyBoxShaderProgram);

            viewMatrix = new Matrix4(new Matrix3(viewMatrix));
            GL.UniformMatrix4(projectionMatrixAttributeLocationS, false, ref projectionMatrix);
            GL.UniformMatrix4(viewMatrixAttributeLocationS, false, ref viewMatrix);

            GL.BindVertexArray(vaoS);
            GL.BindTexture(TextureTarget.TextureCubeMap, texture);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            GL.BindVertexArray(0);
            GL.DepthFunc(DepthFunction.Less);
        }

        public void UpdateFrame()
        {
        }

        public void Initialise(int width, int height)
        {
            shaderProgram = Shaders.GetDefaultShader();
            skyBoxShaderProgram = Shaders.GetSkyBoxShader();
            InitShaderAttributes();
            Resize(width, height);
            skyBoxShaderProgram = Shaders.GetSkyBoxShader();
            texture = Texture.GetCubeMap(Directory.GetFiles(Path.Combine("Textures", "skybox"), "*.png").ToList());
            arrayTex = Texture.InitArray(Directory.GetFiles(Path.Combine("Textures"), "*.png").ToList());
            InitCubes();
            InitBuffers();
        }

        private static void ClearBackground(Color4 backgroundColor)
        {
            GL.ClearColor(backgroundColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        private void InitBuffers()
        {
            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

            GL.GenBuffers(1, out vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * vertex.Count, vertex.ToArray(),
                BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);


            GL.GenBuffers(1, out textureBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * texCords.Count, texCords.ToArray(),
                BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            GL.GenBuffers(1, out position);
            GL.BindBuffer(BufferTarget.ArrayBuffer, position);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * positions.Count,
                positions.ToArray(), BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.GenBuffers(1, out texId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, texId);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(int) * texturesId.Count, texturesId.ToArray(),
                BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Int, false, 6 * sizeof(int), 0);
            GL.EnableVertexAttribArray(4);
            GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Int, false, 6 * sizeof(int), 3 * sizeof(int));


            GL.VertexAttribDivisor(2, 1);
            GL.VertexAttribDivisor(3, 1);
            GL.VertexAttribDivisor(4, 1);


            GL.GenBuffers(1, out ebo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(int), indices.ToArray(),
                BufferUsageHint.StaticDraw);

            GL.BindVertexArray(0);


            //sky
            GL.GenVertexArrays(1, out vaoS);
            GL.BindVertexArray(vaoS);

            GL.GenBuffers(1, out ebo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(int),
                indices.ToArray(), BufferUsageHint.StaticDraw);

            GL.GenBuffers(1, out vboS);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboS);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * SkyBoxVertices.Length, SkyBoxVertices,
                BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);
        }

        private void InitShaderAttributes()
        {
            //modelMatrixAttributeLocation = GL.GetUniformLocation(shaderProgram, "model");
            //viewMatrixAttributeLocation = GL.GetUniformLocation(shaderProgram, "view");
            projectionMatrixAttributeLocation = GL.GetUniformLocation(shaderProgram, "viewProjection");

            viewMatrixAttributeLocationS = GL.GetUniformLocation(skyBoxShaderProgram, "view");
            projectionMatrixAttributeLocationS = GL.GetUniformLocation(skyBoxShaderProgram, "projection");
        }


        public void Resize(int width, int height)
        {
            GL.Viewport(0, 0, width, height);
            viewMatrix = Matrix4.Identity;
            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, width / (float) height, 0.1f, 1500);
        }

        private int cubesCount;

        private void InitCubes()
        {
            indices.AddRange(Cube.GetIndices());
            vertex.AddRange(Cube.GetVertexes());
            texCords.AddRange(Cube.GetTextureCoords());
            foreach (var magic in world.SelectMany(x=>world.GetVisibleBlock(x)))
            {
                foreach (var (yes, pos) in magic)
                {
                    var tmp = yes
                        .Zip(magic.Key, (b, s) => (b, s))
                        .Select(t => t.b ? Texture.textures[t.s] : -1);
                    positions.Add(pos.Convert());
                    texturesId.AddRange(tmp);
                    cubesCount++;
                }
            }
        }

        //private List<int> 

        private static readonly float[] SkyBoxVertices =
        {
            // positions          
            -1.0f, 1.0f, -1.0f,
            -1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,
            1.0f, 1.0f, -1.0f,
            -1.0f, 1.0f, -1.0f,

            -1.0f, -1.0f, 1.0f,
            -1.0f, -1.0f, -1.0f,
            -1.0f, 1.0f, -1.0f,
            -1.0f, 1.0f, -1.0f,
            -1.0f, 1.0f, 1.0f,
            -1.0f, -1.0f, 1.0f,

            1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, 1.0f,
            1.0f, 1.0f, 1.0f,
            1.0f, 1.0f, 1.0f,
            1.0f, 1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,

            -1.0f, -1.0f, 1.0f,
            -1.0f, 1.0f, 1.0f,
            1.0f, 1.0f, 1.0f,
            1.0f, 1.0f, 1.0f,
            1.0f, -1.0f, 1.0f,
            -1.0f, -1.0f, 1.0f,

            -1.0f, 1.0f, -1.0f,
            1.0f, 1.0f, -1.0f,
            1.0f, 1.0f, 1.0f,
            1.0f, 1.0f, 1.0f,
            -1.0f, 1.0f, 1.0f,
            -1.0f, 1.0f, -1.0f,

            -1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f, 1.0f,
            1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f, 1.0f,
            1.0f, -1.0f, 1.0f
        };
    }
}