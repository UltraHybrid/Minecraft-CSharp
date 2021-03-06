﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MinecraftSharp.Domain;
using MinecraftSharp.Domain.Commands;
using MinecraftSharp.Loaders;
using MinecraftSharp.Logic;
using MinecraftSharp.Rendering;
using OpenTK;
using OpenTK.Graphics;
using GL = OpenTK.Graphics.OpenGL4.GL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using OpenTKUtilities = OpenTK.Platform.Utilities;


namespace MinecraftSharp
{
    internal sealed class Window : GameWindow
    {
        public Window(Game game,
            IVisualManager manager) : base(
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

            Texture.InitArray(Directory.GetFiles(Path.Combine("Textures"), "*.png").ToList());
            Model.Load(Directory.GetFiles(Path.Combine("Models", "source"), "*.fbx").ToList());

            this.game = game;
            game.Start();
            Location = new Point(100, 100);
  
            var camera = new Camera(game.Player);
            var skyBoxRender = new SkyBox();
            var worldRender = new World(manager, game);
            var linesRender = new Lines(game);
            var entityRender = new Entity(game);
            render = new Render(camera, skyBoxRender, worldRender, linesRender, entityRender);
            playerControl = new PlayerControl(keys, game.Player.Mover, game.World);
        }

        #region Variables

        private Render render;
        private readonly Dictionary<Key, bool> keys;
        private Game game;
        private PlayerControl playerControl;

        #endregion

        protected override void OnResize(EventArgs e)
        {
            render.Resize(Width, Height);
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);
            CursorVisible = false;
            render.Initialise(Width, Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            playerControl.Move((float) e.Time);
            game.Update((float) e.Time);
            render.UpdateFrame();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Title = $"(VSync: {VSync}) FPS: {1f / e.Time} CORD: " + game.Player.Mover.Position;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            render.RenderFrame();
            SwapBuffers();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            keys[e.Key] = true;
            if (e.Key == Key.Escape)
            {
                Close();
                Environment.Exit(111);
            }

            if (e.Key == Key.Z)
            {
                var com = new Spectator(game);
                com.Execute();
                render.Lines.Aim(Width, Height);
                
            }

            if (e.Key == Key.X)
            {
                render.Lines.Axes();
            }
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            keys[e.Key] = false;
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            Mouse.SetPosition(Bounds.X + Width / 2f, Bounds.Y + Height / 2f);
            playerControl.MouseMove();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButton.Left)
            {
                new PutCommand(game).Execute();
            }

            if (e.Button == MouseButton.Right)
            {
                new BreakCommand(game).Execute();
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            new SwapBlock(game.Player, e.Value).Execute();
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
        }
        
    }
}