using System.Collections.Generic;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain.Commands
{
    public class MoveCommand : ICommand
    {
        private readonly IMover2 entity;
        private readonly IEnumerable<Direction> directions;
        private readonly float time;
        private readonly World<Chunk<Block>, Block> world;

        public MoveCommand(IMover2 entity, World<Chunk<Block>, Block> world, IEnumerable<Direction> directions, float time)
        {
            this.entity = entity;
            this.world = world;
            this.directions = directions;
            this.time = time;
        }

        public void Execute()
        {
            entity.Move(new Piece(world, entity.Position.AsPointL(), 3), directions, time);
        }
    }

    public class RotateCommand : ICommand
    {
        private readonly IMover2 mover;
        private readonly float dx;
        private readonly float dy;

        public RotateCommand(IMover2 mover, float dx, float dy)
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