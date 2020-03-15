using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using ClearBufferMask = OpenTK.Graphics.OpenGL4.ClearBufferMask;
using GL = OpenTK.Graphics.OpenGL4.GL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using OpenTKUtilities = OpenTK.Platform.Utilities;

namespace tmp
{
    internal sealed class Window : GameWindow
    {
        public Window() : base(
            1280, 720,
            GraphicsMode.Default,
            "Minecraft OpenGL 4.1",
            GameWindowFlags.Default,
            DisplayDevice.Default,
            4, 1,
            GraphicsContextFlags.Default)
        {
            VSync = VSyncMode.Off;
            keys = new Dictionary<Key, bool>();
            foreach (Key key in Enum.GetValues(typeof(Key)))
            {
                keys[key] = false;
            }
            camera = new Camera(keys, new Vector3(0f, 0f, 0));
        }
        
        #region Variables

        private Dictionary<Key, bool> keys;
        
        private int shaderProgram;

        private int vao;

        private int vbo;

        private int ebo;

        private int textureBuffer;

        private int text;

        private List<Cube> cubes = new List<Cube>();

        private readonly Camera camera;
        private float lastXPos;
        private float lastYPos;

        private int modelMatrixAttributeLocation;
        private int viewMatrixAttributeLocation;
        private int projectionMatrixAttributeLocation;
        
        private Matrix4 modelMatrix;
        private Matrix4 viewMatrix;
        private Matrix4 projectionMatrix;
        
        #endregion


        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width/(float)Height, 0.1f, 500);
        }

        protected override void OnLoad(EventArgs e)
        {
            Mouse.SetPosition(Width / 2f, Height / 2f);
            CursorVisible = false;
            shaderProgram = Shaders.InitShaders();
            cubes.Add(new Cube(new Vector3(0, 0, 0)));
            InitBuffers();
            InitShaderAttributes();
            InitUniformMatrix();
            text = new Texture().GetTexture();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            camera.Move((float)e.Time);
        }

        protected override void OnClosed(EventArgs e)
        {
            GL.DeleteVertexArrays(1, ref vao);
            GL.DeleteProgram(shaderProgram);
            
            base.OnClosed(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Title = $"(VSync: {VSync}) FPS: {1f / e.Time}";
            GL.Enable(EnableCap.DepthTest);
            ClearBackground(Color4.Aqua);   
            GL.UseProgram(shaderProgram);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Nearest);
            viewMatrix = camera.GetViewMatrix();
            GL.UniformMatrix4(modelMatrixAttributeLocation, false, ref modelMatrix);
            GL.UniformMatrix4(viewMatrixAttributeLocation, false, ref viewMatrix);
            GL.UniformMatrix4(projectionMatrixAttributeLocation, false, ref projectionMatrix);
            
            GL.BindTexture(TextureTarget.Texture2D ,text);
            GL.BindVertexArray(vao);
            GL.DrawElements(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
            
            
            
            SwapBuffers();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            keys[e.Key] = true;
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            keys[e.Key] = false;
        }
        
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            Mouse.SetPosition(Width, Height);
            camera.MouseMove();
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
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * cubes[0].GetVertexes().Count, cubes[0].GetVertexes().ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            GL.GenBuffers(1, out textureBuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, Vector2.SizeInBytes * cubes[0].GetVertexes().Count, cubes[0].GetTextureCoords().ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);
            
            GL.GenBuffers(1, out ebo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, cubes[0].GetIndices().Length * sizeof(int), cubes[0].GetIndices(), BufferUsageHint.StaticDraw);
            
            
            GL.BindVertexArray(0);
        }

        private void InitShaderAttributes()
        {
            modelMatrixAttributeLocation = GL.GetUniformLocation(shaderProgram, "model");
            viewMatrixAttributeLocation = GL.GetUniformLocation(shaderProgram, "view");
            projectionMatrixAttributeLocation = GL.GetUniformLocation(shaderProgram, "projection");
        }

        private void InitUniformMatrix()
        {
            modelMatrix = Matrix4.Identity;
            viewMatrix = Matrix4.Identity;
            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, Width/(float)Height, 0.1f, 500);
        }
    }
}