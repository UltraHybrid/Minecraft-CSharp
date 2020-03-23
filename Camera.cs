using System;
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
        private readonly Player player;

        public Camera(Dictionary<Key, bool> keys, Player player)
        {
            this.keys = keys;
            this.player = player;
            MouseSensitivity = 0.05f;
        }

        public Matrix4 GetViewMatrix()
        {
            var eye = player.Mover.Position.Convert() + new Vector3(0, player.Height, 0);
            return Matrix4.LookAt(eye, eye + player.Mover.Front.Convert(), player.Mover.Up.Convert());
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
            player.Mover.Move(directions, time);
        }

        public void MouseMove()
        {
            var mouse = Mouse.GetState();
            var deltaX = mouse.X - lastMousePosition.X;
            var deltaY = -mouse.Y + lastMousePosition.Y;
            lastMousePosition = new Vector2(mouse.X, mouse.Y);
            var yaw = deltaX * MouseSensitivity;
            var pitch = deltaY * MouseSensitivity;
            player.Mover.Rotate(yaw, pitch);
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