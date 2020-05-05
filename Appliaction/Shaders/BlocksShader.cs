using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace tmp
{
    public class BlocksShader
    {
        private int shaderProgram, vao, vbo, ebo, position, texId, vPMatrixLocation, textureBuffer;

        
        public BlocksShader()
        {
            shaderProgram = Shaders.GetSideShader();
            GenBuffers();
            InstallAttributes();
            vPMatrixLocation = GL.GetUniformLocation(shaderProgram, "viewProjection");
        }

        public void Use()
        {
            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(vao);
        }

        public void SetVPMatrix(Matrix4 vPMatrix) => GL.UniformMatrix4(vPMatrixLocation, false, ref vPMatrix);

        public void SendData(int[] indices, List<Vector3> positions, List<Vector2> sideTexId, List<Vector3> vertex, List<Vector2> texCords)
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
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices,
                BufferUsageHint.StaticDraw);
        }

        private void GenBuffers(int n=1)
        {
            GL.GenVertexArrays(n, out vao);
            GL.GenBuffers(1, out vbo);
            GL.GenBuffers(n, out position);
            GL.GenBuffers(n, out texId);
            GL.GenBuffers(1, out ebo);
            GL.GenBuffers(1, out textureBuffer);
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
        }
    }
}