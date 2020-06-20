using tmp.Domain;
using tmp.Domain.TrialVersion;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Logic
{
    public interface IVisualizer<TSource>
    {
        VisualChunk Visualize(Chunk<TSource> worldChunk);
        VisualizerData UpdateOne(PointL position);
    }
}