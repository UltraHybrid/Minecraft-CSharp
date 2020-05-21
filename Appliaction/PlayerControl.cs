using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;
using tmp.Interfaces;

namespace tmp
{
    public class PlayerControl : IControl
    {
        public float MouseSensitivity { get; set; } = 0.05f;

        private Vector2 lastMousePosition;
        private readonly Dictionary<Key, bool> keys;
        private readonly EntityMover entity;
        private readonly IWorld<Chunk<Block>, Block> world;

        public PlayerControl(Dictionary<Key, bool> keys, EntityMover entity, IWorld<Chunk<Block>, Block> world)
        {
            this.keys = keys;
            this.entity = entity;
            this.world = world;
        }

        public void Move(float time)
        {
            if (keys[Key.W]) new MoveCommand(entity, world, Direction.Forward, time).Execute();
            if (keys[Key.S]) new MoveCommand(entity, world, Direction.Back, time).Execute();
            if (keys[Key.D]) new MoveCommand(entity, world, Direction.Right, time).Execute();
            if (keys[Key.A]) new MoveCommand(entity, world, Direction.Left, time).Execute();
            if (keys[Key.Space]) new MoveCommand(entity, world, Direction.Up, time).Execute();
            if (keys[Key.ShiftLeft]) new MoveCommand(entity, world, Direction.Down, time).Execute();
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