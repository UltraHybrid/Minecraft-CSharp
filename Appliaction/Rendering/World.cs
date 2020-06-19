using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework.Constraints;
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
        private int vbo, ebo;
        private readonly int vPMatrixLocation, shaderProgram;
        private int textureBuffer;

        private readonly int[] vao, data;
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
            data = new int[Size];
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
                if (System.Numerics.Vector3.Dot(viewer.Front, 16*chunksCords[i].AsVector() - viewer.Position.AsVector()) >= 0)
                {
                    GL.BindVertexArray(vao[i]);
                    GL.DrawElementsInstanced(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero,
                        chunkSidesCount[i]);
                }
            }

            GL.BindVertexArray(0);
            GL.Disable(EnableCap.CullFace);
        }

        public void Update()
        {
            while (visualManager.Ready.Count != 0)
            {
                var updateChunk = visualManager.Ready.Dequeue();
                var chunkData = visualManager.World.GetRowData(updateChunk);
                var index = chunksCords.IndexOf(updateChunk);
                chunksCords[index] = updateChunk;
                chunkSidesCount[index] = chunkData.Length / 6;
                SendData(index, chunkData);
            }
            
            if (visualManager.Ready2.Count != 0)
            {
                var (newChunk, chunkForDelete) = visualManager.Ready2.Dequeue();
                var chunkData = visualManager.World.GetRowData(newChunk);
                int index;
                var sidesCount = chunkData.Length / 6;
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

                SendData(index, chunkData);
            }
        }

        private void SendData(int n, float[] data)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.data[n]);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * data.Length,
                data.ToArray(), BufferUsageHint.StaticDraw);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * Vertexes.Length, Vertexes,
                BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, textureBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector2.SizeInBytes * TextureCords.Length, TextureCords,
                BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(int), Indices,
                BufferUsageHint.StaticDraw);
        }

        private void GenBuffers()
        {
            GL.GenVertexArrays(Size, vao);
            GL.GenBuffers(1, out vbo);
            GL.GenBuffers(Size, data);
            GL.GenBuffers(1, out ebo);
            GL.GenBuffers(1, out textureBuffer);
        }

        private void InstallAttributes(int n)
        {
            GL.BindVertexArray(vao[n]);

            //0
            GL.BindBuffer(BufferTarget.ArrayBuffer, data[n]);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 6, 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(float) * 6, sizeof(float) * 3);
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
        }

        private int[] Indices => Cube.GetSideIndices();
        private Vector3[] Vertexes => Cube.GetVertexes();
        private Vector2[] TextureCords => Cube.GetTextureCoords().Select(nn => nn.Xy).ToArray();
    }
}