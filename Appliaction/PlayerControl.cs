using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;

namespace tmp
{
    public class PlayerControl : IControl
    {
        public float MouseSensitivity { get; set; } = 0.05f;
        
        private Vector2 lastMousePosition;
        private Dictionary<Key, bool> keys;
        private EntityMover entity;
        
        public PlayerControl(Dictionary<Key, bool> keys, EntityMover entity)
        {
            this.keys = keys;
            this.entity = entity;
        }

        public void Move(float time)
        {
            if (keys[Key.W]) new MoveCommand(entity, Direction.Forward, time).Execute();
            if (keys[Key.S]) new MoveCommand(entity, Direction.Back, time).Execute();
            if (keys[Key.D]) new MoveCommand(entity, Direction.Right, time).Execute();
            if (keys[Key.A]) new MoveCommand(entity, Direction.Left, time).Execute();
            if (keys[Key.Space]) new MoveCommand(entity, Direction.Up, time).Execute();
            if (keys[Key.ShiftLeft]) new MoveCommand(entity, Direction.Down, time).Execute();
        }

        public void MouseMove()
        {
            var mouse = Mouse.GetState();
            var deltaX = mouse.X - lastMousePosition.X;
            var deltaY = -mouse.Y + lastMousePosition.Y;
            lastMousePosition = new Vector2(mouse.X, mouse.Y);
            var yaw = deltaX * MouseSensitivity;
            var pitch = deltaY * MouseSensitivity;
            new RotateCommand(entity, yaw, pitch).Execute();
        }
    }
}