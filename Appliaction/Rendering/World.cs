using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using tmp.Domain;
using tmp.Infrastructure.SimpleMath;
using tmp.Logic;
using Shader = tmp.Shaders.Shaders;


namespace tmp.Rendering
{
    public class World : IRender
    {
        private readonly int vPMatrixLocation, shaderProgram;

        private readonly int[] vao, vbo, ebo;
        private readonly int arrayTex;

        private readonly List<PointI> chunksCords = new List<PointI>();
        private readonly List<int> chunkSidesCount = new List<int>();
        private readonly VisualManager3 visualManager;
        private readonly IMover2 viewer;
        private int Size { get; }

        public World(VisualManager3 visualManager, IMover2 viewer)
        {
            this.viewer = viewer;
            this.visualManager = visualManager;
            arrayTex = Texture.arrayTex;
            var size = visualManager.World.Size;
            Size = size * size * 16;
            vao = new int[Size];
            vbo = new int[Size];
            ebo = new int[Size];
            shaderProgram = Shader.GetSideShader();
            GenBuffers();
            for (var i = 0; i < Size; i++)
                InstallAttributes(i);
            vPMatrixLocation = GL.GetUniformLocation(shaderProgram, "viewProjection");
        }

        public void Render(Matrix4 viewProjectionMatrix)
        {
            GL.Enable(EnableCap.CullFace);
            GL.UseProgram(shaderProgram);
            GL.BindTexture(TextureTarget.Texture2DArray, arrayTex);

            GL.UniformMatrix4(vPMatrixLocation, false, ref viewProjectionMatrix);

            for (var i = 0; i < chunksCords.Count; i++)
            {
                if (System.Numerics.Vector3.Dot(viewer.Front,
                    16 * chunksCords[i].AsVector() - viewer.Position.AsVector() + 16 * viewer.Front) >= 0)
                {
                    GL.BindVertexArray(vao[i]);
                    GL.DrawElements(BeginMode.Triangles, chunkSidesCount[i], DrawElementsType.UnsignedInt, 0);
                }
            }

            GL.BindVertexArray(0);
            GL.Disable(EnableCap.CullFace);
        }

        public void Update()
        {
            while (visualManager.ReadyToUpdate.Count != 0)
            {
                var updateChunk = visualManager.ReadyToUpdate.Dequeue();
                var (vertex, indices) = visualManager.World.GetRowData(updateChunk);
                var index = chunksCords.IndexOf(updateChunk);
                chunksCords[index] = updateChunk;
                chunkSidesCount[index] = indices.Length;
                SendData(index, vertex, indices);
            }

            if (visualManager.ReadyToReplace.Count != 0)
            {
                var (newChunk, chunkForDelete) = visualManager.ReadyToReplace.Dequeue();
                var (vertex, indices) = visualManager.World.GetRowData(newChunk);
                var sidesCount = indices.Length;
                if (indices.Length != 0)
                {
                    int index;
                    if (Equals(chunkForDelete, newChunk))
                    {
                        chunksCords.Add(newChunk);
                        chunkSidesCount.Add(sidesCount);
                        index = chunksCords.Count - 1;
                    }
                    else
                    {
                        index = chunksCords.IndexOf(chunkForDelete);
                        chunksCords[index] = newChunk;
                        chunkSidesCount[index] = sidesCount;
                    }

                    SendData(index, vertex, indices);
                }
            }
        }

        private void SendData(int n, float[] vertex, int[] indices)
        {
            GL.BindVertexArray(vao[n]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo[n]);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertex.Length,
                vertex, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo[n]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int),
                indices, BufferUsageHint.StaticDraw);
        }

        private void GenBuffers()
        {
            GL.GenVertexArrays(Size, vao);
            GL.GenBuffers(Size, vbo);
            GL.GenBuffers(Size, ebo);
        }

        private void InstallAttributes(int n)
        {
            GL.BindVertexArray(vao[n]);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo[n]);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 7, 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 7, sizeof(float) * 3);
            GL.EnableVertexAttribArray(1);

            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, sizeof(float) * 7, sizeof(float) * 5);
            GL.EnableVertexAttribArray(2);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo[n]);
            GL.BindVertexArray(0);
        }
    }
}