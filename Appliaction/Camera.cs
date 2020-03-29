using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;

namespace tmp
{
    public class Camera
    {
        private Dictionary<Key, bool> keys;
        private Vector2 lastMousePosition;
        private float MouseSensitivity { get; set; }
        private readonly EntityMover viewer;
        private readonly Vector3 offset;

        public Camera(Dictionary<Key, bool> keys, EntityMover viewer, Vector3 offset)
        {
            this.keys = keys;
            this.viewer = viewer;
            MouseSensitivity = 0.05f;
            this.offset = offset;
        }

        public Matrix4 GetViewMatrix()
        {
            var eye = viewer.Position.Convert() + offset;
            return Matrix4.LookAt(eye, eye + viewer.Front.Convert(), viewer.Up.Convert());
        }

        public void Move(float time)
        {
            var directions = new List<Direction>();
            if (keys[Key.W])
                directions.Add(Direction.Forward);
            if (keys[Key.S])
                directions.Add(Direction.Back);
            if (keys[Key.D])
                directions.Add(Direction.Right);
            if (keys[Key.A])
                directions.Add(Direction.Left);
            if (keys[Key.Space])
                directions.Add(Direction.Up);
            if (keys[Key.ShiftLeft])
                directions.Add(Direction.Down);
            viewer.Move(directions, time);
        }

        public void MouseMove()
        {
            var mouse = Mouse.GetState();
            var deltaX = mouse.X - lastMousePosition.X;
            var deltaY = -mouse.Y + lastMousePosition.Y;
            lastMousePosition = new Vector2(mouse.X, mouse.Y);
            var yaw = deltaX * MouseSensitivity;
            var pitch = deltaY * MouseSensitivity;
            viewer.Rotate(yaw, pitch);
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