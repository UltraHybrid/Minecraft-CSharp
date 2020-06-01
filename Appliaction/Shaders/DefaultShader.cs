using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace tmp
{
    public class DefaultShader
    {
        private readonly int vPMatrixLocation, shaderProgram;
        private int vbo, vao, ebo, tbo;

        public DefaultShader()
        {
            shaderProgram = Shaders.GetDefaultShader();
            GenBuffers();
            InstallAttributes();
            vPMatrixLocation = GL.GetUniformLocation(shaderProgram, "viewProjection");
        }

        public void Use() => GL.UseProgram(shaderProgram);

        public void BindVao() => GL.BindVertexArray(vao);

        public void SetVPMatrix(Matrix4 vPMatrix) => GL.UniformMatrix4(vPMatrixLocation, false, ref vPMatrix);

        public void SendData(Vector3[] vertex, Vector2[] tex, uint[] indices)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * vertex.Length,
                vertex, BufferUsageHint.StaticDraw);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, tbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * tex.Length,
                tex, BufferUsageHint.StaticDraw);
            
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length,
                indices, BufferUsageHint.StaticDraw);
        }

        private void GenBuffers()
        {
            GL.GenVertexArrays(1, out vao);
            GL.GenBuffers(1, out vbo);
            GL.GenBuffers(1, out ebo);
            GL.GenBuffers(1, out tbo);
        }


        private void InstallAttributes()
        {
            GL.BindVertexArray(vao);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, tbo);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);
            
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

            GL.BindVertexArray(0);
        }
    }
}