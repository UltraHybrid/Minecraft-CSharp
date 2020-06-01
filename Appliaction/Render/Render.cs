using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using tmp.Logic;

namespace tmp
{
    public class Render
    {
        #region Variables

        private int texture, arrayTex;
        private readonly List<PointI> chunksCords = new List<PointI>();
        private readonly List<int> chunkSidesCount = new List<int>();

        private Matrix4 viewMatrix;
        private Matrix4 projectionMatrix, viewProjectionMatrix;

        private readonly SkyBoxShader skyBoxShader;
        private readonly BlocksShader blocksShader;
        private readonly DefaultShader defaultShader;
        private readonly LineShader lineShader;

        private readonly VisualManager visualManager;
        private readonly Camera camera;

        private readonly Mesh mesh;
        
        private Stopwatch sw = new Stopwatch();

        #endregion

        public Render(Camera camera, VisualManager visualManager)
        {
            this.camera = camera;
            this.visualManager = visualManager;
            skyBoxShader = new SkyBoxShader();
            blocksShader = new BlocksShader(visualManager.World.Size);
            lineShader = new LineShader();
            defaultShader = new DefaultShader();
            mesh = new Mesh("./models/walk-robot/source/Animation_END.fbx", defaultShader);
            sw.Start();
        }

        public void RenderFrame()
        {
            ClearBackground(Color4.White);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.DepthTest);

            UpdateMatrix();
            //RenderLines();
            RenderWorld();
            mesh.Render(viewProjectionMatrix);
            RenderSkyBox();
        }

        private void RenderLines()
        {
            lineShader.Use();
            var aa = Matrix4.CreateTranslation(new Vector3(-0.8f, 0.5f, 0f));
            var position = camera.viewer.Position.Convert();
            lineShader.SetVPMatrix(Matrix4.CreateTranslation(position + new Vector3(0, 1.8f, 0)) * viewMatrix *
                                   projectionMatrix);
            GL.DrawArrays(PrimitiveType.Lines, 0, 6);
        }

        private void UpdateMatrix()
        {
            viewMatrix = camera.GetViewMatrix();
            viewProjectionMatrix = viewMatrix * projectionMatrix;
        }

        private void RenderWorld()
        {
            GL.Enable(EnableCap.CullFace);
            blocksShader.Use();

            blocksShader.SetVPMatrix(viewProjectionMatrix);

            GL.BindTexture(TextureTarget.Texture2DArray, arrayTex);

            for (var i = 0; i < chunksCords.Count; i++)
            {
                if (Vector3.Dot(camera.viewer.Front.Convert(),
                chunksCords[i].Convert() * 16 - camera.viewer.Position.Convert()) >= 0)
                {
                    blocksShader.BindVao(i);
                    GL.DrawElementsInstanced(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero,
                        chunkSidesCount[i]);
                }
            }

            GL.BindVertexArray(0);
            GL.Disable(EnableCap.CullFace);
        }

        private void RenderSkyBox()
        {
            GL.DepthFunc(DepthFunction.Lequal);
            skyBoxShader.Use();

            skyBoxShader.SetVPMatrix(new Matrix4(new Matrix3(viewMatrix)) * projectionMatrix);

            GL.BindTexture(TextureTarget.TextureCubeMap, texture);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            GL.DepthFunc(DepthFunction.Less);
        }

        public void UpdateFrame()
        {
            if (visualManager.Ready.Count != 0)
            {
                var (chunkForRender, chunkForSwap) = visualManager.Ready.Dequeue();
                var chunk = visualManager.World[chunkForRender];
                if (chunk != null)
                {
                    var data = chunk.SimpleData;
                    int index;
                    var sidesCount = data.TexturesData.Count;
                    if (Equals(chunkForSwap, chunkForRender))
                    {
                        chunksCords.Add(chunkForRender);
                        chunkSidesCount.Add(sidesCount);
                        index = chunksCords.Count - 1;
                    }
                    else
                    {
                        index = chunksCords.IndexOf(chunkForSwap);
                        chunksCords[index] = chunkForRender;
                        chunkSidesCount[index] = sidesCount;
                    }

                    blocksShader.SendData(index, data.Positions, data.TexturesData);
                }

                timer++;
                if (timer == 2500 || timer == 1600)
                {
                    Console.WriteLine(timer + $": {sw.ElapsedMilliseconds}");
                }
                    
            }
        }

        public void Initialise(int width, int height)
        {
            Resize(width, height);
            texture = Texture.GetCubeMap(Directory.GetFiles(Path.Combine("Textures", "skybox"), "*.png").ToList());
            arrayTex = Texture.InitArray(Directory.GetFiles(Path.Combine("Textures"), "*.png").ToList());
            UpdateBuffersData();
        }

        private int timer = 0;
        
        
        private static void ClearBackground(Color4 backgroundColor)
        {
            GL.ClearColor(backgroundColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        private void UpdateBuffersData() => skyBoxShader.SendData();

        public void Resize(int width, int height)
        {
            GL.Viewport(0, 0, width, height);
            viewMatrix = Matrix4.Identity;
            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, width / (float) height, 0.1f, 1575);
        }
    }
}