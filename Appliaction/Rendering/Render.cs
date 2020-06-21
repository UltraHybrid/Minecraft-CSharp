﻿using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using tmp.Domain;
using tmp.Infrastructure.SimpleMath;
using tmp.Logic;

namespace tmp.Rendering
{
    public class Render
    {
        #region Variables

        private int texture, arrayTex;
        private readonly List<PointI> chunksCords = new List<PointI>();
        private readonly List<int> chunkSidesCount = new List<int>();

        private Matrix4 viewMatrix;
        private Matrix4 projectionMatrix, viewProjectionMatrix;

        private readonly SkyBox skyBox;
        private readonly World world;
        public readonly Lines lines;
        public readonly Entity Entity;

        private readonly Game game;
        private readonly VisualManager3 visualManager;
        private readonly Camera camera;
        private readonly float heigth;

        #endregion

        public Render(Camera camera, VisualManager3 visualManager, Game game)
        {
            this.game = game;
            heigth = game.Player.Height;
            this.camera = camera;
            this.visualManager = visualManager;
            skyBox = new SkyBox();
            world = new World(visualManager, camera.viewer);
            lines = new Lines(camera.viewer);
            Entity = new Entity(game);
        }

        public void RenderFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            ClearBackground(Color4.White);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.DepthTest);

            lines.Render(viewProjectionMatrix, heigth);
            world.Render(viewProjectionMatrix);
            Entity.Render(viewProjectionMatrix);
            skyBox.Render(new Matrix4(new Matrix3(viewMatrix)) * projectionMatrix);
            
        }

        public void UpdateFrame()
        {
            UpdateMatrix();
            Entity.Update();
            world.Update();
        }
        
        private void UpdateMatrix()
        {
            viewMatrix = camera.GetViewMatrix();
            viewProjectionMatrix = viewMatrix * projectionMatrix;
        }
        public void Initialise(int width, int height)
        {
            Resize(width, height);
        }

        private static void ClearBackground(Color4 backgroundColor)
        {
            GL.ClearColor(backgroundColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void Resize(int width, int height)
        {
            GL.Viewport(0, 0, width, height);
            viewMatrix = Matrix4.Identity;
            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.05f, width / (float) height, 0.1f, 1575);
        }
    }
}
