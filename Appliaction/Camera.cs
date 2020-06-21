using OpenTK;
using tmp.Domain;
using tmp.Infrastructure.SimpleMath;
using Vector3 = OpenTK.Vector3;

namespace tmp
{
    public class Camera
    {
        public readonly IMover viewer;
        private readonly Vector3 offset;

        public Camera(IMover viewer, Vector3 offset)
        {
            this.viewer = viewer;
            this.offset = offset;
        }

        public Matrix4 GetViewMatrix()
        {
            var eye = viewer.Position.Convert() + offset;
            return Matrix4.LookAt(eye, eye + viewer.Front.Convert(), Vector3.UnitY);
        }
    }
}