namespace tmp
{
    public class MoveCommand : ICommand
    {
        private readonly IMover entity;
        private readonly Direction direction;
        private readonly float time;

        public MoveCommand(IMover entity, Direction direction, float time)
        {
            this.entity = entity;
            this.direction = direction;
            this.time = time;
        }

        public void Execute()
        {
            entity.Move(direction, time);
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