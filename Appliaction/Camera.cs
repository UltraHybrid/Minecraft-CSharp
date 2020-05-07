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
        private readonly IMover viewer;
        private readonly Vector3 offset;

        public Camera(Dictionary<Key, bool> keys, IMover viewer, Vector3 offset)
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
            if (keys[Key.W]) new MoveCommand(viewer, Direction.Forward, time).Execute();
            if (keys[Key.S]) new MoveCommand(viewer, Direction.Back, time).Execute();
            if (keys[Key.D]) new MoveCommand(viewer, Direction.Right, time).Execute();
            if (keys[Key.A]) new MoveCommand(viewer, Direction.Left, time).Execute();
            if (keys[Key.Space]) new MoveCommand(viewer, Direction.Up, time).Execute();
            if (keys[Key.ShiftLeft]) new MoveCommand(viewer, Direction.Down, time).Execute();
        }

        public void MouseMove()
        {
            var mouse = Mouse.GetState();
            var deltaX = mouse.X - lastMousePosition.X;
            var deltaY = -mouse.Y + lastMousePosition.Y;
            lastMousePosition = new Vector2(mouse.X, mouse.Y);
            var yaw = deltaX * MouseSensitivity;
            var pitch = deltaY * MouseSensitivity;
            new RotateCommand(viewer, yaw, pitch).Execute();
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