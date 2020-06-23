using System;
using MinecraftSharp.Domain.TrialVersion.Blocks;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Domain
{
    public delegate void BlockUpdateEvent(PointL position);
    public interface IWorldManager
    {
        PointI MakeFirstLaunch();
        void Update();
        void PutBlock(BlockType blockType, PointL position);
        event Action<Chunk<Block>> AddAlert;
        event Action<Chunk<Block>> DeleteAlert;
        event BlockUpdateEvent UpdateAlert;
    }
}