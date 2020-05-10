namespace tmp
{
    public interface IGame
    {
        IWorld World { get; }
        Player Player { get; }
        void Start();
        void Update();
    }
}