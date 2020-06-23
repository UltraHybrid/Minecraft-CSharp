using MinecraftSharp.Domain.TrialVersion.Blocks;

namespace MinecraftSharp.Domain
{
    public interface IGame
    {
        World<Chunk<Block>, Block> World { get; }
        Player Player { get; }
        void Start();
        void Update(float time);
    }
}