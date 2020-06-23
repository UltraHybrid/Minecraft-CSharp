using MinecraftSharp.Domain;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Logic
{
    public interface IVisualizer<TSource>
    {
        VisualChunk Visualize(Chunk<TSource> worldChunk);
        VisualizerData UpdateOne(PointL position);
    }
}