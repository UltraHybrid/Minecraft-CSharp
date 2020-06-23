using System.IO;
using System.Linq;
using MinecraftSharp.Loaders;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Shader = MinecraftSharp.Shaders.Shaders;

namespace MinecraftSharp.Rendering
{
    public class SkyBox : IRender
    {
        private readonly int shaderProgram, vPMatrixLocation, texture;
        private int vao, vbo, ebo;
        private static int[] Indices => Cube.GetSideIndices();

        public SkyBox()
        {
            shaderProgram = Shaders.Shaders.GetSkyBoxShader();
            GenBuffers();
            InstallAttributes();
            vPMatrixLocation = GL.GetUniformLocation(shaderProgram, "projection");
            texture = Texture.GetCubeMap(Directory.GetFiles(Path.Combine("Textures", "skybox"), "*.png").ToList());
            SendData();
        }

        public void Render(Matrix4 viewProjectionMatrix)
        {
            GL.DepthFunc(DepthFunction.Lequal);
            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(vao);

            GL.UniformMatrix4(vPMatrixLocation, false, ref viewProjectionMatrix);

            GL.BindTexture(TextureTarget.TextureCubeMap, texture);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            GL.DepthFunc(DepthFunction.Less);
        }

        public void Update()
        {
        }

        private void SendData()
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
            GL.GenVertexArrays(1, out vao);
            GL.GenBuffers(1, out ebo);
            GL.GenBuffers(1, out vbo);
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