using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace tmp
{
    public class BlocksShader
    {
        private int shaderProgram, vbo, ebo, vPMatrixLocation, textureBuffer;

        private int[] vao, position, texturesId;

        private int Size { get; }

        public BlocksShader(int size)
        {
            Size = size * size;
            vao = new int[Size];
            position = new int[Size];
            texturesId = new int[Size];
            shaderProgram = Shaders.GetSideShader();
            GenBuffers();
            for(var i = 0; i < Size; i++)
                InstallAttributes(i);
            vPMatrixLocation = GL.GetUniformLocation(shaderProgram, "viewProjection");
        }

        public void Use() => GL.UseProgram(shaderProgram);

        public void BindVao(int n) => GL.BindVertexArray(vao[n]);

        public void SetVPMatrix(Matrix4 vPMatrix) => GL.UniformMatrix4(vPMatrixLocation, false, ref vPMatrix);

        public void SendData(int n, List<Vector3> positions, List<Vector2> sideTexId)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, position[n]);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * positions.Count,
                positions.ToArray(), BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, texturesId[n]);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector2.SizeInBytes * sideTexId.Count, sideTexId.ToArray(),
                BufferUsageHint.StaticDraw);

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
            GL.GenBuffers(Size, position);
            GL.GenBuffers(Size, texturesId);
            GL.GenBuffers(1, out ebo);
            GL.GenBuffers(1, out textureBuffer);
        }

        private void InstallAttributes(int n)
        {
            GL.BindVertexArray(vao[n]);

            //0
            GL.BindBuffer(BufferTarget.ArrayBuffer, position[n]);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            //1
            GL.BindBuffer(BufferTarget.ArrayBuffer, texturesId[n]);
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
        }

        private int[] Indices => Cube.GetSideIndices();
        private Vector3[] Vertexes => Cube.GetVertexes();
        private Vector2[] TextureCords => Cube.GetTextureCoords().Select(nn => nn.Xy).ToArray();
    }
}