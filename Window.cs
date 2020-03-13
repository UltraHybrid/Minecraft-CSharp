using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using ClearBufferMask = OpenTK.Graphics.OpenGL4.ClearBufferMask;
using GL = OpenTK.Graphics.OpenGL4.GL;
using OpenTK.Graphics.OpenGL4;
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
        }
        
        #region Variables
        
        private int shaderProgram;

        private int vao;

        private int vbo;

        private int ebo;

        private List<Vector3> t = new List<Vector3>()
        {
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 0, 0)
        };

        private int[] i = new[]
        {
            0, 1, 2,
            2, 3, 0

        };
        
        #endregion


        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnLoad(EventArgs e)
        {
            CursorVisible = false;
            shaderProgram = Shaders.InitShaders();
            InitBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
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
            ClearBackground(Color4.Aqua);   
            GL.UseProgram(shaderProgram);
            
            GL.BindVertexArray(vao);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
            
            
            
            SwapBuffers();
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
            GL.BufferData(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * t.Count, t.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            GL.GenBuffers(1, out ebo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, i.Length * sizeof(int), i, BufferUsageHint.StaticDraw);
            
            
            GL.BindVertexArray(0);
        }
        
        private void UpdateFrame()
        {
            
        }
    }
}