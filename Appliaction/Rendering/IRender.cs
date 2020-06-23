using OpenTK;

namespace MinecraftSharp.Rendering
{
    public interface IRender
    {
        void Render(Matrix4 viewProjectionMatrix);
        void Update();
    }
}