using MinecraftSharp.Domain;
using MinecraftSharp.Domain.TrialVersion.Blocks;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Logic
{
    public interface IVisualManager
    {
        VisualWorld World { get; }
        bool HasDataToAdd();
        (PointI, PointI) GetDataToAdd();
        bool HasDataToUpdate();
        PointI GetDataToUpdate();
        void HandlerForAdd(Chunk<Block> chunk);
        void HandlerForDelete();
        void HandlerForUpdate(PointL position);
    }
}