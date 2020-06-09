using System.Collections.Generic;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain.Commands
{
    public class MoveCommand : ICommand
    {
        private readonly IMover entity;
        private readonly IEnumerable<Direction> directions;
        private readonly float time;
        private readonly IWorld<Chunk<Block>, Block> world;

        public MoveCommand(IMover entity, IWorld<Chunk<Block>, Block> world, IEnumerable<Direction> directions, float time)
        {
            this.entity = entity;
            this.world = world;
            this.directions = directions;
            this.time = time;
        }

        public void Execute()
        {
            entity.Move(new Piece(world, entity.Position.AsPointI(), 3), directions, time);
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