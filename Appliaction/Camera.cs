using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;

namespace tmp
{
    public class Camera
    {
        private readonly EntityMover viewer;
        private readonly Vector3 offset;

        public Camera(EntityMover viewer, Vector3 offset)
        {
            this.viewer = viewer;
            this.offset = offset;
        }

        public Matrix4 GetViewMatrix()
        {
            var eye = viewer.Position.Convert() + offset;
            return Matrix4.LookAt(eye, eye + viewer.Front.Convert(), viewer.Up.Convert());
        }
    }

    public static class ModelExtension
    {
        public static Vector3 Convert(this Vector vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }
    }
}