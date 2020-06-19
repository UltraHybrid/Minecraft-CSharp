using tmp.Domain;
using tmp.Domain.TrialVersion;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Logic
{
    public class VisualWorld : World<VisualChunk, VisualizerData>
    {
        public VisualWorld(PointI startOffset, int size) : base(startOffset, size)
        {
        }
        
        
        public float[] GetRowData(PointI position)
        {
            return this[new PointI(position.X, 0, position.Z)].RowData[position.Y].ToArray();
        } 


        public override VisualizerData GetItem(PointI position)
        {
            var (cPosition, ePosition) = Translate2LocalNotation(position);
            var elementPosition = ePosition.AsPointB();
            if (chunks.ContainsKey(cPosition) && Chunk<VisualizerData>.CheckBounds(elementPosition))
                return chunks[cPosition][elementPosition];
            return null;
        }

        public override bool TrySetItem(PointI position, VisualizerData value)
        {
            var (cPosition, ePosition) = Translate2LocalNotation(position);
            var elementPosition = ePosition.AsPointB();
            if (chunks.ContainsKey(cPosition) && Chunk<VisualizerData>.CheckBounds(elementPosition))
            {
                chunks[cPosition][elementPosition] = value;
                return true;
            }

            return false;
        }
    }
}