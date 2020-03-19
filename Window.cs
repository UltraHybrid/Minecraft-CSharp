using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using ClearBufferMask = OpenTK.Graphics.OpenGL4.ClearBufferMask;
using GL = OpenTK.Graphics.OpenGL4.GL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using OpenTKUtilities = OpenTK.Platform.Utilities;
using StringName = OpenTK.Graphics.OpenGL.StringName;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;

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

        private readonly Dictionary<Key, bool> keys;

        private int shaderProgram;

        private int vao;
        private int vbo;
        private int ebo;
        private int textureBuffer;

        private int texture;
        private int texture2;


        private readonly Camera camera;

        private int modelMatrixAttributeLocation;
        private int viewMatrixAttributeLocation;
        private int projectionMatrixAttributeLocation;

        private Matrix4 modelMatrix;
        private Matrix4 viewMatrix;
        private Matrix4 projectionMatrix;


        private readonly List<Cube> cubes = new List<Cube>();
        private List<Vector3> vertex = new List<Vector3>();
        private List<int> indices = new List<int>();
        private List<Vector2> texcoords = new List<Vector2>();

        #endregion


        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            projectionMatrix =
                Matrix4.CreatePerspectiveFieldOfView((float) Math.PI / 4, Width / (float) Height, 0.1f, 500);
        }

        protected override void OnLoad(EventArgs e)
        {
            Mouse.SetPosition(Width / 2f, Height / 2f);
            CursorVisible = false;
            shaderProgram = Shaders.InitShaders();
            InitCubes(50, -3, 50);
            InitBuffers();
            InitShaderAttributes();
            InitUniformMatrix();
            texture = new Texture().GetTexture1();
            texture2 = new Texture().GetTexture2();
            
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            camera.Move((float) e.Time);
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
            //GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactor.SrcAlphaSaturate, BlendingFactor.One);
            //GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            ClearBackground(Color4.Aqua);
            GL.UseProgram(shaderProgram);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) TextureMagFilter.Nearest);
            viewMatrix = camera.GetViewMatrix();
            GL.UniformMatrix4(modelMatrixAttributeLocation, false, ref modelMatrix);
            GL.UniformMatrix4(viewMatrixAttributeLocation, false, ref viewMatrix);
            GL.UniformMatrix4(projectionMatrixAttributeLocation, false, ref projectionMatrix);

            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.BindVertexArray(vao);
            GL.DrawElements(PrimitiveType.Triangles, 36 * cubes.Count, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);


            SwapBuffers();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            keys[e.Key] = true;
            if (e.Key == Key.Escape)
                Close();
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            keys[e.Key] = false;
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        { 
            Mouse.SetPosition(Width / 2f, Height / 2f);
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

        private void InitUniformMatrix()
        {
            modelMatrix = Matrix4.Identity;
            viewMatrix = Matrix4.Identity;
            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, Width / (float) Height, 0.1f, 500);
        }

        private int t;

        private void InitCubes(int x, int y, int z)
        {
            Console.WriteLine(x*z*4);
            Console.WriteLine(OpenTK.Graphics.OpenGL.GL.GetString(StringName.Version));
            for (var i = -x; i < x; i++)
            for (var j = -z; j < z; j++)
            {
                var a = new Cube(new Vector3(i * 2, y, j));
                cubes.Add(a);
                indices.AddRange(a.GetIndices(t * 24));
                vertex.AddRange(a.GetVertexes());
                texcoords.AddRange(a.GetTextureCoords());
                t++;
            }
        }
    }
}