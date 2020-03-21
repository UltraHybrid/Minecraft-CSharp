﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using tmp.TrialVersion;

namespace tmp
{
    public class Render
    {
        #region Variables

        private int shaderProgram, skyBoxShaderProgram;

        private int vao, vbo, ebo, textureBuffer;
        
        private int vaoS, vboS, texture;

        private readonly Dictionary<string, int> textures = new Dictionary<string, int>();


        

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

            GL.BindTexture(TextureTarget.Texture2D, textures["dirt"]);
            GL.BindVertexArray(vao);
            GL.DrawElements(PrimitiveType.Triangles, 36 * cubes.Count, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
            
            //sky
            
            GL.DepthFunc(DepthFunction.Lequal);
            GL.UseProgram(skyBoxShaderProgram);

            viewMatrix = new Matrix4(new Matrix3(viewMatrix));
            GL.UniformMatrix4(projectionMatrixAttributeLocationS, false, ref projectionMatrix);
            GL.UniformMatrix4(viewMatrixAttributeLocationS, false, ref viewMatrix);
            
            GL.BindVertexArray(vaoS);
            //GL.ActiveTexture(0);
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
            InitCubes();
            InitBuffers();
            InitShaderAttributes();
            Resize(width, height);
            InitTextures("Textures");
            var textureStorage = Path.Combine("Textures", "skybox");
            skyBoxShaderProgram = Shaders.GetSkyBoxShader();
            texture = Texture.GetCubeMap(Directory.GetFiles(textureStorage, "*.png").ToList());
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
            
            
            //sky
            GL.GenVertexArrays(1, out vaoS);
            GL.BindVertexArray(vaoS);
            
            GL.GenBuffers(1, out vboS);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboS);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * skyboxVertices.Length, skyboxVertices, BufferUsageHint.StaticDraw);
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
            foreach (var textureFile in Directory.GetFiles(textureStorage, "*.png"))
            {
                var www = Texture.GetTexture(textureFile);
                var name = Path.GetFileNameWithoutExtension(textureFile);
                textures.Add(Path.GetFileNameWithoutExtension(textureFile), www);
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
        
    public float[] skyboxVertices = {
        // positions          
        -1.0f,  1.0f, -1.0f,
        -1.0f, -1.0f, -1.0f,
         1.0f, -1.0f, -1.0f,
         1.0f, -1.0f, -1.0f,
         1.0f,  1.0f, -1.0f,
        -1.0f,  1.0f, -1.0f,

        -1.0f, -1.0f,  1.0f,
        -1.0f, -1.0f, -1.0f,
        -1.0f,  1.0f, -1.0f,
        -1.0f,  1.0f, -1.0f,
        -1.0f,  1.0f,  1.0f,
        -1.0f, -1.0f,  1.0f,

         1.0f, -1.0f, -1.0f,
         1.0f, -1.0f,  1.0f,
         1.0f,  1.0f,  1.0f,
         1.0f,  1.0f,  1.0f,
         1.0f,  1.0f, -1.0f,
         1.0f, -1.0f, -1.0f,

        -1.0f, -1.0f,  1.0f,
        -1.0f,  1.0f,  1.0f,
         1.0f,  1.0f,  1.0f,
         1.0f,  1.0f,  1.0f,
         1.0f, -1.0f,  1.0f,
        -1.0f, -1.0f,  1.0f,

        -1.0f,  1.0f, -1.0f,
         1.0f,  1.0f, -1.0f,
         1.0f,  1.0f,  1.0f,
         1.0f,  1.0f,  1.0f,
        -1.0f,  1.0f,  1.0f,
        -1.0f,  1.0f, -1.0f,

        -1.0f, -1.0f, -1.0f,
        -1.0f, -1.0f,  1.0f,
         1.0f, -1.0f, -1.0f,
         1.0f, -1.0f, -1.0f,
        -1.0f, -1.0f,  1.0f,
         1.0f, -1.0f,  1.0f
    };
    }
}