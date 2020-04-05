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
        private int shaderProgram, skyBoxShaderProgram, sideShaderProgram;

        //buffers
        private int vao, vbo, ebo, textureBuffer, position;

        private int[] chunkVao, chunkPosition, chunkTexId;

        private int vaoS, vboS, texture, texId, arrayTex;

        private int projectionMatrixAttributeLocation;

        private int viewMatrixAttributeLocationS;
        private int projectionMatrixAttributeLocationS;

        private Matrix4 viewMatrix;
        private Matrix4 projectionMatrix, viewProjectionMatrix;

        private readonly List<Vector2> sideTexId = new List<Vector2>();
        private readonly List<Vector3> vertex = new List<Vector3>();
        private readonly List<Vector3> positions = new List<Vector3>();
        private readonly List<int> indices = new List<int>();
        private readonly List<Vector2> texCords = new List<Vector2>();

        private readonly IVisualizer<Chunk, IEnumerable<VisualizerData>> visualizer;
        private readonly World world;
        private readonly Camera camera;

        #endregion

        public Render(Camera camera, IVisualizer<Chunk, IEnumerable<VisualizerData>> visualizer, World world)
        {
            this.camera = camera;
            this.visualizer = visualizer;
            this.world = world;
        }

        public void RenderFrame()
        {
            ClearBackground(Color4.White);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.CullFace);

            RenderWorld();
            RenderSkyBox();
        }

        private void RenderWorld()
        {
            GL.UseProgram(sideShaderProgram);

            viewMatrix = camera.GetViewMatrix();
            viewProjectionMatrix = viewMatrix * projectionMatrix;
            GL.UniformMatrix4(projectionMatrixAttributeLocation, false, ref viewProjectionMatrix);

            GL.BindTexture(TextureTarget.Texture2DArray, arrayTex);
            GL.BindVertexArray(vao);
            GL.DrawElementsInstanced(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero,
                sidesCount);
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
            sideShaderProgram = Shaders.GetSideShader();
            shaderProgram = Shaders.GetDefaultShader();
            skyBoxShaderProgram = Shaders.GetSkyBoxShader();
            InitShaderAttributes();
            Resize(width, height);
            skyBoxShaderProgram = Shaders.GetSkyBoxShader();
            texture = Texture.GetCubeMap(Directory.GetFiles(Path.Combine("Textures", "skybox"), "*.png").ToList());
            arrayTex = Texture.InitArray(Directory.GetFiles(Path.Combine("Textures"), "*.png").ToList());
            InitCubes();
            GenBuffers();
            GetDataToBuffer();
            InstallAttributes();
        }

        private static void ClearBackground(Color4 backgroundColor)
        {
            GL.ClearColor(backgroundColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        private void GenBuffers()
        {
            GL.GenVertexArrays(1, out vao);
            GL.GenBuffers(1, out vbo);
            GL.GenBuffers(1, out textureBuffer);
            GL.GenBuffers(1, out position);
            GL.GenBuffers(1, out texId);
            GL.GenBuffers(1, out ebo);

            GL.GenVertexArrays(1, out vaoS);
            GL.GenBuffers(1, out ebo);
            GL.GenBuffers(1, out vboS);
        }


        private void GetDataToBuffer()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, position);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * positions.Count,
                positions.ToArray(), BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, texId);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector2.SizeInBytes * sideTexId.Count, sideTexId.ToArray(),
                BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * vertex.Count, vertex.ToArray(),
                BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, textureBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector2.SizeInBytes * texCords.Count, texCords.ToArray(),
                BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(int), indices.ToArray(),
                BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboS);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * SkyBoxVertices.Length, SkyBoxVertices,
                BufferUsageHint.StaticDraw);
        }

        private void InstallAttributes()
        {
            GL.BindVertexArray(vao);

            //0
            GL.BindBuffer(BufferTarget.ArrayBuffer, position);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            //1
            GL.BindBuffer(BufferTarget.ArrayBuffer, texId);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            //2-7
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            for (var index = 2; index <= 7; index++)
            {
                GL.VertexAttribPointer(index, 3, VertexAttribPointerType.Float, false, 0,
                    sizeof(float) * (index - 2) * 12);
                GL.EnableVertexAttribArray(index);
            }

            //8-13
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureBuffer);
            for (var index = 8; index <= 13; index++)
            {
                GL.VertexAttribPointer(index, 2, VertexAttribPointerType.Float, false, 0,
                    sizeof(float) * (index - 8) * 8);
                GL.EnableVertexAttribArray(index);
            }

            GL.VertexAttribDivisor(0, 1);
            GL.VertexAttribDivisor(1, 1);


            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BindVertexArray(0);


            //sky

            GL.BindVertexArray(vaoS);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboS);
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
            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, width / (float) height, 0.1f, 1575);
        }

        private int sidesCount;

        private void InitCubes()
        {
            indices.AddRange(Cube.GetSideIndices());
            vertex.AddRange(Cube.GetVertexes());
            texCords.AddRange(Cube.GetTextureCoords().Select(nn => nn.Xy));
            foreach (var data in world.SelectMany(x => visualizer.GetVisibleFaces(x)))
            {
                foreach (var (name, number) in data.TextureNumber)
                {
                    positions.Add(data.Position.Convert());
                    sideTexId.Add(new Vector2(number, Texture.textures[name]));
                    sidesCount++;
                }
            }
        }

        private static readonly float[] SkyBoxVertices =
        {
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