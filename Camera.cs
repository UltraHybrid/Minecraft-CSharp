using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;

namespace tmp
{
    public class Camera
    {
        private Vector3 position;
        private Vector3 direction;
        private Vector3 target;
        private Vector3 front;
        private Vector3 up;
        private Vector3 right;
        private Dictionary<Key, bool> keys;
        private float pitch;
        private float yaw;

        public Camera(Dictionary<Key, bool> keys, Vector3 position = default, Vector3 orientation = default)
        {
            this.keys = keys;
            target = new Vector3(0, 0, 0);
            this.position = new Vector3(0, 0, -5);
            direction = new Vector3(this.position - target);
            MoveSpeed = 5f;
            MouseSensitivity = 0.1f;
            front = new Vector3(0, 0, 1);
            right = Vector3.Cross(Vector3.UnitY, direction).Normalized();
            up = Vector3.Cross(direction, right).Normalized();
        }

        public Matrix4 GetViewMatrix() => Matrix4.LookAt(position, position + front, up);

        public void Move(float time)
        {
            if (keys[Key.W])
                position += MoveSpeed * new Vector3(front.X, 0, front.Z) * time;
            if (keys[Key.S])
                position -= MoveSpeed * new Vector3(front.X, 0, front.Z) * time;
            if (keys[Key.D])
                position += MoveSpeed * right * time;
            if (keys[Key.A])
                position -= MoveSpeed * right * time;
            if (keys[Key.Space])
                position += MoveSpeed * up * time;
            if (keys[Key.ShiftLeft])
                position -= MoveSpeed * up * time;
            Console.WriteLine(position);
        }

        private Vector2 lastPos;
        private bool firstMouse = true;
        public void MouseMove()
        {
            var mouse = Mouse.GetState();

            if (firstMouse)
            {
                lastPos = new Vector2(mouse.X, mouse.Y);
                firstMouse = false;
            }
            else
            {
                var deltaX = mouse.X - lastPos.X;
                var deltaY = -mouse.Y + lastPos.Y;
                lastPos = new Vector2(mouse.X, mouse.Y);


                yaw += deltaX * MouseSensitivity;
                pitch += deltaY * MouseSensitivity;
            }

            if(pitch > 89.0f)
                pitch = 89.0f;
            if(pitch < -89.0f)
                pitch = -89.0f;

            front.X = (float)Math.Cos(MathHelper.DegreesToRadians(yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(pitch));
            front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
            front.Z = (float) Math.Sin(MathHelper.DegreesToRadians(yaw)) * (float)Math.Cos(MathHelper.DegreesToRadians(pitch));
            front.Normalize();
            right = -Vector3.Cross(up, front).Normalized();
        }

        private float MoveSpeed { get; set; }
        

        private float MouseSensitivity { get; set; }
    }
}