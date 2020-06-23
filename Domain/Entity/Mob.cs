using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Domain.Entity
{
    public abstract class Mob : IEntity
    {
        public string Name { get; }
        public int Health { get; }
        public abstract EntityMover Mover { get; }

        public abstract void GoTo(PointL target, Piece piece, float time);
        public abstract void Follow(PointL target, Piece piece, float time, float distance);
    }
}