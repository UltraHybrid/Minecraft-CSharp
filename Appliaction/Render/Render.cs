using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace tmp
{
    public class Render
    {
        #region Variables

        private int shaderProgram, skyBoxShaderProgram;

        private int vao, vbo, ebo, textureBuffer, position;

        private int vaoS, vboS, texture;

        private readonly Dictionary<string, int> textures = new Dictionary<string, int>();

        private int arrayTex;


        private int modelMatrixAttributeLocation;
        private int viewMatrixAttributeLocation;
        private int projectionMatrixAttributeLocation;

        private int viewMatrixAttributeLocationS;
        private int projectionMatrixAttributeLocationS;

        private Matrix4 modelMatrix;
        private Matrix4 viewMatrix;
        private Matrix4 projectionMatrix;


        private readonly List<Cube> cubes = new List<Cube>();
        private readonly List<Vector3> vertex = new List<Vector3>();
        private readonly List<Vector3> positions = new List<Vector3>();
        private List<int> indices = new List<int>();
        private List<Vector2> texcoords = new List<Vector2>();

        private World world;
        private readonly Camera camera;

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

            viewMatrix = camera.GetViewMatrix();
            GL.UniformMatrix4(modelMatrixAttributeLocation, false, ref modelMatrix);
            GL.UniformMatrix4(projectionMatrixAttributeLocation, false, ref projectionMatrix);
            GL.UniformMatrix4(viewMatrixAttributeLocation, false, ref viewMatrix);

            GL.BindTexture(TextureTarget.Texture2DArray, arrayTex);
            GL.BindVertexArray(vao);
            GL.DrawElementsInstanced(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedInt, IntPtr.Zero, t);
            GL.BindVertexArray(0);

            //sky

            GL.DepthFunc(DepthFunction.Lequal);
            GL.UseProgram(skyBoxShaderProgram);

            viewMatrix = new Matrix4(new Matrix3(viewMatrix));
            GL.UniformMatrix4(projectionMatrixAttributeLocationS, false, ref projectionMatrix);
            GL.UniformMatrix4(viewMatrixAttributeLocationS, false, ref viewMatrix);

            GL.BindVertexArray(vaoS);
            GL.BindTexture(TextureTarget.TextureCubeMap, texture);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            GL.BindVertexArray(0);
            GL.DepthFunc(DepthFunction.Less);
        }

        public void UpdateFrame()
        {
        }

        public void Initialise(int width, int height)
        {
            shaderProgram = Shaders.GetDefaultShader();
            skyBoxShaderProgram = Shaders.GetSkyBoxShader();
            InitShaderAttributes();
            Resize(width, height);
            skyBoxShaderProgram = Shaders.GetSkyBoxShader();
            texture = Texture.GetCubeMap(Directory.GetFiles(Path.Combine("Textures", "skybox"), "*.png")
                .ToList());
            arrayTex = Texture.InitArray(Directory.GetFiles(Path.Combine("Textures"), "*.png").ToList());
            InitCubes();
            InitBuffers();
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

            vertex.Clear();

            GL.GenBuffers(1, out textureBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector2.SizeInBytes * texcoords.Count,
                texcoords.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            GL.GenBuffers(1, out position);
            GL.BindBuffer(BufferTarget.ArrayBuffer, position);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * positions.Count,
                positions.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribDivisor(2, 1);

            texcoords.Clear();

            GL.GenBuffers(1, out ebo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(int),
                indices.ToArray(), BufferUsageHint.StaticDraw);

            indices.Clear();
            GL.BindVertexArray(0);


            //sky
            GL.GenVertexArrays(1, out vaoS);
            GL.BindVertexArray(vaoS);

            GL.GenBuffers(1, out vboS);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboS);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * skyboxVertices.Length, skyboxVertices,
                BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);
        }

        private void InitShaderAttributes()
        {
            modelMatrixAttributeLocation = GL.GetUniformLocation(shaderProgram, "model");
            viewMatrixAttributeLocation = GL.GetUniformLocation(shaderProgram, "view");
            projectionMatrixAttributeLocation = GL.GetUniformLocation(shaderProgram, "projection");

            viewMatrixAttributeLocationS = GL.GetUniformLocation(skyBoxShaderProgram, "view");
            projectionMatrixAttributeLocationS = GL.GetUniformLocation(skyBoxShaderProgram, "projection");
        }

        private void InitTextures(string textureStorage)
        {
            foreach (var a in BaseBlocks.AllBlocks.ToDictionary(x => x.Name, y => y.TextureName))
            {
                
                
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
            var a = new Cube();
            indices.AddRange(a.GetIndices());
            vertex.AddRange(a.GetVertexesWithoutOffset());
            var tm = a.GetTextureCoords().Select(v3 => v3.Xy);
            texcoords.AddRange(tm);
            foreach (var blocks in world.GetVisibleBlock(0, 0))
            {
                var tmp = blocks.Key.Select(te => Texture.textures[te]).ToList();
                foreach (var blockCord in blocks.Value)
                {
                    positions.Add(blockCord);
                    t++;
                }
            }
        }

        public float[] skyboxVertices =
        {
            // positions          
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