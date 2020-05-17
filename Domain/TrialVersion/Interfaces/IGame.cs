namespace tmp.Interfaces
{
    public interface IGame
    {
        IWorld<Block> World { get; }
        Player Player { get; }
        void Start();
        void Update();
    }
}