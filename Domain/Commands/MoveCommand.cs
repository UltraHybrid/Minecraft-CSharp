using System.Collections.Generic;
using MinecraftSharp.Domain.Entity;
using MinecraftSharp.Domain.TrialVersion.Blocks;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Domain.Commands
{
    public class MoveCommand : ICommand
    {
        private readonly IMover entity;
        private readonly IReadOnlyList<Direction> directions;
        private readonly float time;
        private readonly World<Chunk<Block>, Block> world;

        public MoveCommand(IMover entity, World<Chunk<Block>, Block> world, IReadOnlyList<Direction> directions, float time)
        {
            this.entity = entity;
            this.world = world;
            this.directions = directions;
            this.time = time;
        }

        public void Execute()
        {
            entity.Move(new Piece(world, entity.Position.AsPointL(), 5, new List<Mob>()), directions, time);
        }
    }

    public class RotateCommand : ICommand
    {
        private readonly IMover mover;
        private readonly float dx;
        private readonly float dy;

        public RotateCommand(IMover mover, float dx, float dy)
        {
            this.mover = mover;
            this.dx = dx;
            this.dy = dy;
        }

        public void Execute()
        {
            mover.Rotate(dx, dy);
        }
    }
}