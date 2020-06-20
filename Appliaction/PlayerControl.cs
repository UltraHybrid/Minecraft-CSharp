using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;
using tmp.Domain;
using tmp.Domain.Commands;
using tmp.Domain.TrialVersion;
using tmp.Domain.TrialVersion.Blocks;
using Veldrid;

namespace tmp
{
    public class PlayerControl : IControl
    {
        public float MouseSensitivity { get; set; } = 0.05f;

        private Vector2 lastMousePosition;
        private readonly Dictionary<Key, bool> keys;
        private readonly EntityMover2 entity;
        private readonly World<Chunk<Block>, Block> world;

        public PlayerControl(Dictionary<Key, bool> keys, EntityMover2 entity, World<Chunk<Block>, Block> world)
        {
            this.keys = keys;
            this.entity = entity;
            this.world = world;
        }

        public void Move(float time)
        {
            var directions = new List<Direction>();
            if (keys[Key.W]) directions.Add(Direction.Forward);
            if (keys[Key.S]) directions.Add(Direction.Back);
            if (keys[Key.D]) directions.Add(Direction.Right);
            if (keys[Key.A]) directions.Add(Direction.Left);
            if (keys[Key.Space]) directions.Add(Direction.Up);
            if (keys[Key.ShiftLeft]) directions.Add(Direction.Down);
            new MoveCommand(entity, world, directions, time).Execute();
        }

        public void MouseMove()
        {
            var mouse = Mouse.GetState();
            var deltaX = mouse.X - lastMousePosition.X;
            var deltaY = -mouse.Y + lastMousePosition.Y;
            lastMousePosition = new Vector2(mouse.X, mouse.Y);
            var yaw = deltaX * MouseSensitivity;
            var pitch = deltaY * MouseSensitivity;
            new RotateCommand(entity, yaw, -pitch).Execute();
        }
    }
}