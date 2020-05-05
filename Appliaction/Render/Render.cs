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
        private int texture, arrayTex;

        private Matrix4 viewMatrix;
        private Matrix4 projectionMatrix, viewProjectionMatrix;
        
        private readonly SkyBoxShader skyBoxShader = new SkyBoxShader();
        private readonly BlocksShader blocksShader = new BlocksShader();

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
            blocksShader.Use();

            viewMatrix = camera.GetViewMatrix();
            viewProjectionMatrix = viewMatrix * projectionMatrix;
            blocksShader.SetVPMatrix(viewProjectionMatrix);
            GL.BindTexture(TextureTarget.Texture2DArray, arrayTex);
            GL.DrawElementsInstanced(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero,
                sidesCount);
            GL.BindVertexArray(0);
        }

        private void RenderSkyBox()
        {
            GL.DepthFunc(DepthFunction.Lequal);
            skyBoxShader.Use();

            viewMatrix = new Matrix4(new Matrix3(viewMatrix)) * projectionMatrix;
            skyBoxShader.SetVPMatrix(viewMatrix);

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
            Resize(width, height);
            texture = Texture.GetCubeMap(Directory.GetFiles(Path.Combine("Textures", "skybox"), "*.png").ToList());
            arrayTex = Texture.InitArray(Directory.GetFiles(Path.Combine("Textures"), "*.png").ToList());
            InitCubes();
            UpdateBuffersData();
        }

        private static void ClearBackground(Color4 backgroundColor)
        {
            GL.ClearColor(backgroundColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        private void UpdateBuffersData()
        {
            blocksShader.SendData(indices.ToArray(), positions, sideTexId, vertex, texCords);
            skyBoxShader.SendData(indices.ToArray());
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
    }
}