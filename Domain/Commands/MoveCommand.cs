using tmp.Interfaces;

namespace tmp
{
    public class MoveCommand : ICommand
    {
        private readonly IMover entity;
        private readonly Direction direction;
        private readonly float time;
        private readonly IWorld<Chunk<Block>, Block> world;

        public MoveCommand(IMover entity, IWorld<Chunk<Block>, Block> world, Direction direction, float time)
        {
            this.entity = entity;
            this.world = world;
            this.direction = direction;
            this.time = time;
        }

        public void Execute()
        {
            entity.Move(new Piece(world, (PointI) entity.Position, 1), direction, time);
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