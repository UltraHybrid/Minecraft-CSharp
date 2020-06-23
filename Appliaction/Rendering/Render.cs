using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace MinecraftSharp.Rendering
{
    public class Render
    {
        #region Variables
        private Matrix4 viewMatrix;
        private Matrix4 projectionMatrix, viewProjectionMatrix;

        private readonly SkyBox skyBox;
        private readonly World world;
        public readonly Lines lines;
        private readonly Entity entity;
        private readonly Camera camera;
        #endregion

        public Render(Camera camera,
            SkyBox skyBox,
            World worldRender,
            Lines lineRender,
            Entity entityRender)
        {
            this.camera = camera;
            this.skyBox = skyBox;
            world = worldRender;
            lines = lineRender;
            entity = entityRender;
        }

        public void RenderFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            ClearBackground(Color4.White);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.DepthTest);

            lines.Render(viewProjectionMatrix);
            world.Render(viewProjectionMatrix);
            entity.Render(viewProjectionMatrix);
            skyBox.Render(new Matrix4(new Matrix3(viewMatrix)) * projectionMatrix);
        }

        public void UpdateFrame()
        {
            UpdateMatrix();
            entity.Update();
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