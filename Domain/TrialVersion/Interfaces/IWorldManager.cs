using System;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
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