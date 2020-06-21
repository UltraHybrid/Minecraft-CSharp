using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using Assimp;
using OpenTK.Graphics.OpenGL4;
using tmp.Domain;
using PrimitiveType = OpenTK.Graphics.OpenGL4.PrimitiveType;
using Shader = tmp.Shaders.Shaders;

namespace tmp.Rendering
{
    public class Entity
    {
        private AssimpContext assimpContext = new AssimpContext();
        private const PostProcessSteps Flags = PostProcessSteps.FlipUVs | PostProcessSteps.Triangulate;

        private readonly int vPMatrixLocation, shaderProgram;
        private int vbo, vao, ebo, tbo, transform;

        private readonly List<Vector3> vertexList = new List<Vector3>();
        private readonly List<uint> indexList = new List<uint>();
        private readonly List<Vector2> texCords = new List<Vector2>();
        private List<Matrix4> matri = new List<Matrix4>();
        private readonly int texture;
        private readonly Game game;

        public Entity(Game game)
        {
            this.game = game;

            shaderProgram = Shader.GetDefaultShader();
            GenBuffer();
            InstallAttributes();
            var scene = assimpContext.ImportFile("./Models/source/cow.fbx", Flags);

            foreach (var mesh in scene.Meshes)
            {
                var size = (uint) vertexList.Count;
                texCords.AddRange(mesh.TextureCoordinateChannels[0].Select(f => new Vector2(f.X, f.Y)));
                vertexList.AddRange(mesh.Vertices.Select(c => new Vector3(c.X, c.Y, c.Z)));
                indexList.AddRange(mesh.GetUnsignedIndices().Select(e => e + size));
            }

            texture = Texture.InitTextureFromFile("./Models/textures/cow.png", true);
            vPMatrixLocation = GL.GetUniformLocation(shaderProgram, "viewProjection");
            SendData(vertexList.ToArray(), texCords.ToArray(), indexList.ToArray());
        }

        public void Render(Matrix4 vpm)
        {
            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(vao);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            SetVPM(vpm);
            GL.DrawElementsInstanced(PrimitiveType.Triangles, indexList.Count, DrawElementsType.UnsignedInt,
                IntPtr.Zero, matri.Count);
        }

        public void Update()
        {
            matri = new List<Matrix4>();
            foreach (var animal in game.Animals)
            {
                var angle = Vector3.CalculateAngle(animal.Mover.Front.Convert(), new Vector3(1, 0, 0));
                var position = animal.Mover.Position.Convert();
                var tmpMatr =
                    Matrix4.CreateScale(0.05f) *
                    Matrix4.CreateRotationX(-(float) Math.PI / 2) *
                    Matrix4.CreateRotationY(angle + (float)Math.PI / 2 + 10e-3f) *
                    Matrix4.CreateTranslation(position.X, position.Y + 0.6f, position.Z);
                matri.Add(tmpMatr);
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, transform);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector4.SizeInBytes * matri.Count * 4, matri.ToArray(),
                BufferUsageHint.StreamDraw);
        }

        private void SendData(Vector3[] vertex, Vector2[] texCord, uint[] indices)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * vertexList.Count, vertex,
                BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, tbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector2.SizeInBytes * texCords.Count, texCord,
                BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, transform);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector4.SizeInBytes * matri.Count * 4, matri.ToArray(),
                BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices,
                BufferUsageHint.StaticDraw);
        }

        private void GenBuffer()
        {
            GL.GenVertexArrays(1, out vao);
            GL.GenBuffers(1, out vbo);
            GL.GenBuffers(1, out tbo);
            GL.GenBuffers(1, out ebo);
            GL.GenBuffers(1, out transform);
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

            GL.BindBuffer(BufferTarget.ArrayBuffer, transform);
            for (var i = 0; i < 4; i++)
            {
                GL.VertexAttribPointer(2 + i, 4, VertexAttribPointerType.Float, false, Vector4.SizeInBytes * 4,
                    Vector4.SizeInBytes * i);
                GL.EnableVertexAttribArray(2 + i);
                GL.VertexAttribDivisor(2 + i, 1);
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

            GL.BindVertexArray(0);
        }

        private void SetVPM(Matrix4 VPM) => GL.UniformMatrix4(vPMatrixLocation, false, ref VPM);
    }
}