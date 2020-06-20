namespace tmp.Domain.Entity
{
    public class Entity
    {
        public readonly string Name;
        public int Health { get; protected set; }
        public IMover2 Mover { get; protected set; }
    }
}