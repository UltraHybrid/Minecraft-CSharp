using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using tmp.TrialVersion;

namespace tmp
{
    public class Render
    {
        #region Variables

        private int shaderProgram;

        private int vao;
        private int vbo;
        private int ebo;
        private int textureBuffer;

        private readonly Dictionary<string, int> textures = new Dictionary<string, int>();


        private readonly Camera camera;

        private int modelMatrixAttributeLocation;
        private int viewMatrixAttributeLocation;
        private int projectionMatrixAttributeLocation;

        private Matrix4 modelMatrix;
        private Matrix4 viewMatrix;
        private Matrix4 projectionMatrix;


        private readonly List<Cube> cubes = new List<Cube>();
        private readonly List<Vector3> vertex = new List<Vector3>();
        private List<int> indices = new List<int>();
        private List<Vector2> texcoords = new List<Vector2>();

        private World world;

        #endregion

        public Render(Camera camera, World world)
        {
            this.world = world;
            this.camera = camera;
        }

        public void RenderFrame()
        {
            ClearBackground(Color4.White);
            GL.UseProgram(shaderProgram);

            GL.BindTexture(TextureTarget.Texture2D, textures["cobblestone"]);
            GL.BindVertexArray(vao);
            GL.DrawElements(PrimitiveType.Triangles, 36 * cubes.Count, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);


        }
        public void UpdateFrame()
        {
            viewMatrix = camera.GetViewMatrix();
            GL.UniformMatrix4(modelMatrixAttributeLocation, false, ref modelMatrix);
            GL.UniformMatrix4(projectionMatrixAttributeLocation, false, ref projectionMatrix);
            GL.UniformMatrix4(viewMatrixAttributeLocation, false, ref viewMatrix);
        }

        public void Initialise(int width, int height)
        {
            shaderProgram = Shaders.InitShaders();
            InitCubes();
            InitBuffers();
            InitShaderAttributes();
            Resize(width, height);
            InitTextures("Textures");
        }
        
        private static void ClearBackground(Color4 backgroundColor)
        {
            GL.ClearColor(backgroundColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        private void InitBuffers()
        {
            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

            GL.GenBuffers(1, out vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * vertex.Count,
                vertex.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            GL.GenBuffers(1, out textureBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector2.SizeInBytes * texcoords.Count,
                texcoords.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            GL.GenBuffers(1, out ebo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(int),
                indices.ToArray(), BufferUsageHint.StaticDraw);

            GL.BindVertexArray(0);
        }

        private void InitShaderAttributes()
        {
            modelMatrixAttributeLocation = GL.GetUniformLocation(shaderProgram, "model");
            viewMatrixAttributeLocation = GL.GetUniformLocation(shaderProgram, "view");
            projectionMatrixAttributeLocation = GL.GetUniformLocation(shaderProgram, "projection");
        }

        private void InitTextures(string textureStorage)
        {
            foreach (var textureFile in Directory.GetFiles(textureStorage, "*.png"))
            {
                var texture = Texture.GetTexture(textureFile);
                var name = Path.GetFileNameWithoutExtension(textureFile);
                textures.Add(name, texture);
            }
        }

        public void Resize(int width, int height)
        {
            GL.Viewport(0, 0, width, height);
            modelMatrix = Matrix4.Identity;
            viewMatrix = Matrix4.Identity;
            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, width / (float) height, 0.1f, 500);
        }

        private int t;

        private void InitCubes()
        {
            foreach (var blocks in world.GetVisibleBlock(0, 0))
            {
                foreach (var blockCord in blocks.Value)
                {
                    Console.WriteLine(blockCord);
                    var a = new Cube(blockCord);
                    cubes.Add(a);
                    indices.AddRange(a.GetIndices(t * 24));
                    vertex.AddRange(a.GetVertexes());
                    texcoords.AddRange(a.GetTextureCoords());
                    t++;
                }
            }
        }
    }
}