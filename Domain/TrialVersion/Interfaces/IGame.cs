namespace tmp.Interfaces
{
    public interface IGame
    {
        World<Chunk<Block>, Block> World { get; }
        Player Player { get; }
        void Start();
        void Update();
    }
}