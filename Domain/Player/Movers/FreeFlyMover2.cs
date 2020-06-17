using System.Collections.Generic;

namespace tmp.Domain
{
   /* public class FreeFlyMover2 : EntityMover2
    {
        public override float Speed { get; set; }
        public override float Pitch { get; set; }
        public override float Yaw { get; set; }

        public FreeFlyMover2(Vector_Last position, Vector_Last front) : base(position, front)
        {
            Speed = 15f;
        }

        public override void Move(Piece piece, IEnumerable<Direction> directions, float time)
        {
            var distance = Speed * time;
            var frontXZ = new Vector_Last(Front.X, 0, Front.Z).Normalized();
            var resultMove = Vector_Last.Zero;
            foreach (var direction in directions)
            {
                resultMove += direction switch
                {
                    Direction.Forward => frontXZ,
                    Direction.Back => -frontXZ,
                    Direction.Right => -Left,
                    Direction.Left => Left,
                    Direction.Up => Up,
                    Direction.Down => -Up,
                };
            }

            basis = basis.Shift(distance * (resultMove.Normalized()));
        }

        public override void Rotate(float deltaYaw, float deltaPitch)
        {
            Yaw += deltaYaw;
            Pitch += deltaPitch;
            basis = basis.Rotate(Yaw, Pitch).Normalized();
        }
    }*/
}