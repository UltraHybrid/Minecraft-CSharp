using System.Collections.Generic;

namespace tmp
{
    public interface IVisualizer<TSource, TResult>
    {
        IReadOnlyList<VisualizerData> GetVisibleFaces(TSource data);
    }
}