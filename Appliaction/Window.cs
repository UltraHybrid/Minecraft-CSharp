using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using GL = OpenTK.Graphics.OpenGL4.GL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using tmp.Interfaces;
using tmp.Logic;
using OpenTKUtilities = OpenTK.Platform.Utilities;
using Vector3 = OpenTK.Vector3;

namespace tmp
{
    internal sealed class Window : GameWindow
    {
        public Window(Game game, VisualManager2 manager) : base(
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

            this.game = game;
            this.manager = manager;
            Location = new Point(100, 100);
            camera = new Camera(game.Player.Mover, new Vector3(0, game.Player.Height, 0));
            playerControl = new PlayerControl(keys, game.Player.Mover, game.World);
            render = new Render(camera, manager);
        }

        #region Variables

        private VisualManager2 manager;
        private Camera camera;
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
            game.Update();
            manager.Update();
            render.UpdateFrame();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Title = $"(VSync: {VSync}) FPS: {1f / e.Time} CORD: " + game.Player.Mover.Position;

            render.RenderFrame();

            SwapBuffers();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            keys[e.Key] = true;
            if (e.Key == Key.Escape)
                Close();
            if (e.Key == Key.U)
                render.UpdateFrame();
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
    }
}