using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace tmp
{
    public class Camera
    {
        private Vector3 _position;
        private Vector3 _orientation;
        
        public Camera(Vector3 position=default, Vector3 orientation=default)
        {
            _orientation = orientation == default 
                ? new Vector3((float)Math.PI, 0, 0) 
                : orientation;
            _position = position;
            lookAt = new Vector3
            {
                X = (float) (Math.Sin(_orientation.X) * Math.Cos(_orientation.Y)),
                Y = (float) Math.Sin(_orientation.Y),
                Z = (float) (Math.Cos(_orientation.X) * Math.Cos(_orientation.Y))
            };

            MoveSpeed = 0.1f;
        }

        public Vector3 lookAt;

        public Matrix4 GetViewMatrix() => Matrix4.LookAt(_position, _position + lookAt, Vector3.UnitY);

        public void Move(float x, float y, float z)
        {
            var offset = new Vector3();
 
            var forward = new Vector3((float)Math.Sin(_orientation.X), 0, (float)Math.Cos(_orientation.X));
            var right = new Vector3(-forward.Z, 0, forward.X);
 
            offset += x * right;
            offset += y * forward;
            offset.Y += z;
 
            offset.NormalizeFast();
            offset = Vector3.Multiply(offset, MoveSpeed);
 
            _position += offset;
        }

        private float MoveSpeed { get; set; }
        
        public void AddRotation(float x, float y)
        { 
            x *= MouseSensitivity;
            y *= MouseSensitivity;
 
            _orientation.X = (_orientation.X + x) % ((float)Math.PI * 2.0f);
            _orientation.Y = Math.Max(Math.Min(_orientation.Y + y, (float)Math.PI / 2.0f - 0.1f), (float)-Math.PI / 2.0f + 0.1f);
            lookAt = new Vector3
            {
                X = (float) (Math.Sin(_orientation.X) * Math.Cos(_orientation.Y)),
                Y = (float) Math.Sin(_orientation.Y),
                Z = (float) (Math.Cos(_orientation.X) * Math.Cos(_orientation.Y))
            };
        }

        private float MouseSensitivity { get; set; }
    }
}