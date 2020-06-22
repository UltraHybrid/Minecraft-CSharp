namespace tmp.Domain.Entity
{
    public interface IEntity
    {
        public string Name { get; }
        public int Health { get; }
        public EntityMover Mover { get; }
    }
}