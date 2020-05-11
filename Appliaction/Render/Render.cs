﻿using System;
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
        private readonly List<PointI> chunksCords = new List<PointI>();
        private readonly List<int> chunkSidesCount = new List<int>();

        private Matrix4 viewMatrix;
        private Matrix4 projectionMatrix, viewProjectionMatrix;

        private readonly SkyBoxShader skyBoxShader;
        private readonly BlocksShader blocksShader;

        private readonly VisualMap visualMap;
        private readonly World2 world;
        private readonly Camera camera;

        #endregion

        public Render(Camera camera, VisualMap visualMap, World2 world)
        {
            this.camera = camera;
            this.visualMap = visualMap;
            this.world = world;
            skyBoxShader = new SkyBoxShader();
            blocksShader = new BlocksShader(world.Size);
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
            
            var n = 0;
            for (var i = 0; i < chunksCords.Count; i++)
            {
                if (Vector2.Dot(camera.viewer.Front.Convert().Xz,
                    chunksCords[i].Convert().Xz * 16 - camera.viewer.Position.Convert().Xz) >= 0)
                {
                    blocksShader.BindVao(i);
                    GL.DrawElementsInstanced(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero,
                        chunkSidesCount[i]);
                }
                else n++;
            }
            //Console.WriteLine(n);
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
            while (!visualMap.Ready.IsEmpty)
            {
                if (visualMap.Ready.TryDequeue(out var point))
                {
                    int index;
                    var sidesCount = visualMap.Data[point.Item1].Count;
                    if (Equals(point.Item2, point.Item1))
                    {
                        chunksCords.Add(point.Item1);
                        chunkSidesCount.Add(sidesCount);
                        index = chunksCords.Count - 1;
                    }
                    else
                    {
                        index = chunksCords.IndexOf(point.Item2);
                        chunksCords[index] = point.Item1;
                        chunkSidesCount[index] = sidesCount;
                    }

                    var data = visualMap.Data[point.Item1];
                    var positions = new List<Vector3>();
                    var texId = new List<Vector2>();
                    foreach (var d in data)
                    {
                        foreach (var f in d.Faces)
                        {
                            positions.Add(d.Position.Convert());
                            texId.Add(new Vector2(f.Number, Texture.textures[f.Name]));
                        }
                    }

                    //blocksShader.SendData(index, visualMap.Data[point.Item1].positions.ToArray(), visualMap.Data[point.Item1].texId.ToArray());
                    blocksShader.SendData(index, positions, texId);
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