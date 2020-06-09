using tmp.Domain;
using tmp.Domain.TrialVersion;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Logic
{
    public class VisualChunk: Chunk<VisualizerData>
    {
        public RevisedData SimpleData;
        public VisualChunk(PointI position) : base(position)
        {
        }
    }
}