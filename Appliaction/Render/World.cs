using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using tmp.Logic;

namespace tmp
{
    public class World : IRender
    {
        private int ebo;
        private readonly int vPMatrixLocation, shaderProgram;
        private int textureBuffer;

        private readonly int[] vao, vbo, texturesId;
        private readonly List<int> arrayTex = new List<int>();

        private readonly List<PointI> chunksCords = new List<PointI>();
        private readonly List<int> chunkSidesCount = new List<int>();
        private readonly VisualManager3 visualManager;
        private readonly IMover viewer;
        private int Size { get; }

        public World(VisualManager3 visualManager, IMover viewer)
        {
            this.viewer = viewer;
            this.visualManager = visualManager;
            foreach (var e in Directory.GetFiles(Path.Combine("Textures"), "*.png").ToList())
            {
                arrayTex.Add(Texture.GetTexture(e));
            }
            var size = visualManager.World.Size;
            Size = size * size;
            vao = new int[Size];
            vbo = new int[Size];
            texturesId = new int[Size];
            shaderProgram = Shaders.GetSideShader();
            GenBuffers();
            for (var i = 0; i < Size; i++)
                InstallAttributes(i);
            vPMatrixLocation = GL.GetUniformLocation(shaderProgram, "viewProjection");
        }

        public void Render(Matrix4 viewProjectionMatrix)
        {
            //GL.Enable(EnableCap.CullFace);
            GL.UseProgram(shaderProgram);
            //GL.BindTexture(TextureTarget.Texture2DArray, arrayTex);

            GL.UniformMatrix4(vPMatrixLocation, false, ref viewProjectionMatrix);
            
            for (var i = 0; i < chunksCords.Count; i++)
            {
                //if (Vector2.Dot(viewer.Front.Convert().Xz,
                  //  chunksCords[i].Convert().Xz * 16 - viewer.Position.Convert().Xz) >= 0)
                {
                    GL.BindVertexArray(vao[i]);
                    GL.DrawElements(PrimitiveType.Triangles, chunkSidesCount[i] * 6, DrawElementsType.UnsignedInt, 0);
                }
            }

            GL.BindVertexArray(0);
            //GL.Disable(EnableCap.CullFace);
        }

        public void Update()
        {
            if (visualManager.Ready.Count != 0)
            {
                var (newChunk, chunkForDelete) = visualManager.Ready.Dequeue();
                var chunk = visualManager.World[newChunk];
                if (chunk != null)
                {
                    var data = chunk.SimpleData;
                    //Console.WriteLine(data==null);
                    int index;
                    var sidesCount = data.TexturesData.Count;
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
                    SendData(index, data.Positions, data.TexturesData);
                }
            }
        }

        private int indCount;

        private void SendData(int n, List<Vector3> vertex, List<uint> indexData)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo[n]);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * vertex.Count,
                vertex.ToArray(), BufferUsageHint.StaticDraw);
/*
            GL.BindBuffer(BufferTarget.ArrayBuffer, texturesId[n]);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector2.SizeInBytes * sideTexId.Count, sideTexId.ToArray(),
                BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * Vertexes.Length, Vertexes,
                BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, textureBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector2.SizeInBytes * TextureCords.Length, TextureCords,
                BufferUsageHint.StaticDraw);
*/

            if (indCount < indexData.Count)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
                GL.BufferData(BufferTarget.ElementArrayBuffer, indexData.Count * sizeof(int), indexData.ToArray(),
                    BufferUsageHint.StaticDraw);
            }
        }

        private void GenBuffers()
        {
            GL.GenVertexArrays(Size, vao);
            GL.GenBuffers(Size, vbo);
            GL.GenBuffers(1, out ebo);
        }

        private void InstallAttributes(int n)
        {
            GL.BindVertexArray(vao[n]);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo[n]);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);


            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BindVertexArray(0);
        }

        private uint[] Indices => Cube.GetSideIndices();
        private Vector2[] TextureCords => Cube.GetTextureCoords().Select(nn => nn.Xy).ToArray();
    }
}