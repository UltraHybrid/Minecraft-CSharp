namespace tmp.Domain.Entity
{
    public interface IEntity
    {
        public string Name { get; }
        public int Health { get; }
        public IMover Mover { get; }
    }
}