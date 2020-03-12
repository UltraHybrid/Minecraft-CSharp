using System;
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

        private int vertexArray; 


        #endregion


        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnLoad(EventArgs e)
        {
            CursorVisible = false;
            shaderProgram = Shaders.InitShaders();
            
            GL.GenVertexArrays(1, out vertexArray);
            GL.BindVertexArray(vertexArray);

        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            OpenTK.Graphics.OpenGL.GL.DeleteVertexArrays(1, ref vertexArray);
            GL.DeleteProgram(shaderProgram);
            
            base.OnClosed(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Title = $"(VSync: {VSync}) FPS: {1f / e.Time}";
            ClearBackground(Color4.Aqua);   
            GL.UseProgram(shaderProgram);
            
            GL.DrawArrays(PrimitiveType.Points, 0, 1);
            GL.PointSize(1000);
            
            
            
            SwapBuffers();
        }

        private static void ClearBackground(Color4 backgroundColor)
        {
            GL.ClearColor(backgroundColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
    }
}