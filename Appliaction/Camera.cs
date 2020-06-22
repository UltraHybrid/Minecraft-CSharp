﻿using OpenTK;
using tmp.Domain;
using tmp.Infrastructure.SimpleMath;
using Vector3 = OpenTK.Vector3;

namespace tmp
{
    public class Camera
    {
        private readonly IMover viewer;
        private readonly Vector3 offset;

        public Camera(Player player)
        {
            this.viewer = player.Mover;
            this.offset = player.Height* Vector3.UnitY;
        }

        public Matrix4 GetViewMatrix()
        {
            var eye = viewer.Position.Convert() + offset;
            return Matrix4.LookAt(eye, eye + viewer.Front.Convert(), Vector3.UnitY);
        }
    }
}