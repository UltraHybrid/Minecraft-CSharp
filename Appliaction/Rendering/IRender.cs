using OpenTK;

namespace tmp.Rendering
{
    public interface IRender
    {
        void Render(Matrix4 viewProjectionMatrix);
        void Update();
    }
}