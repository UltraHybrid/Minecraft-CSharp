using System.Drawing.Drawing2D;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace tmp
{
    public class SkyBoxShader
    {
        private readonly int shaderProgram;
        private int vao, vbo, ebo;
        private readonly int vPMatrixLocation;
        private static int[] Indices => Cube.GetSideIndices();

        public SkyBoxShader()
        {
            shaderProgram = Shaders.GetSkyBoxShader();
            GenBuffers();
            InstallAttributes();
            vPMatrixLocation = GL.GetUniformLocation(shaderProgram, "projection");
        }

        public void Use()
        {
            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(vao);
        }

        public void SetVPMatrix(Matrix4 vPMatrix) => GL.UniformMatrix4(vPMatrixLocation, false, ref vPMatrix);

        public void SendData()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(int), Indices,
                BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * SkyBoxVertices.Length, SkyBoxVertices,
                BufferUsageHint.StaticDraw);
        }

        private void GenBuffers()
        {
            var n = 1;
            GL.GenVertexArrays(n, out vao);
            GL.GenBuffers(n, out ebo);
            GL.GenBuffers(n, out vbo);
        }

        private void InstallAttributes()
        {
            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);
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