using tmp.Domain;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Logic
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