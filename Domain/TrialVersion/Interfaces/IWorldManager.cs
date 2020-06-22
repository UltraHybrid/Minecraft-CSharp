using System;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    public interface IWorldManager
    {
        World<Chunk<Block>, Block> World { get; }
        PointI MakeFirstLunch();
        void Update();
        void PutBlock(BlockType blockType, PointL position);
        event Action<Chunk<Block>> AddAlert; 
        event Action<Chunk<Block>> DeleteAlert;
        event BlockUpdateEvent UpdateAlert;
    }
}