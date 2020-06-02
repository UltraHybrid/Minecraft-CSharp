using OpenTK;

namespace tmp
{
    public interface IRender
    {
        void Render(Matrix4 viewProjectionMatrix);
        void Update();
    }
}