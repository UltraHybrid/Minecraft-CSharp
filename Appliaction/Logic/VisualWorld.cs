using System;
using System.Linq;
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


        public (float[] vertex, int[] indices) GetRowData(PointI position)
        {
            var chunk = this[new PointI(position.X, 0, position.Z)];
            if (chunk == null)
                throw new ArgumentException("Данного чанка не существует " + new PointI(position.X, 0, position.Z));
            var rd = chunk.RowData[position.Y];
            var i = chunk.Indices[position.Y];
            return (rd.ToArray(), i.ToArray());
        }


        public override VisualizerData GetItem(PointL position)
        {
            var (cPosition, ePosition) = Translate2LocalNotation(position);
            if (chunks.ContainsKey(cPosition) && Chunk<VisualizerData>.CheckBounds(ePosition))
                return chunks[cPosition][ePosition];
            return null;
        }

        public override bool TrySetItem(PointL position, VisualizerData value)
        {
            var (cPosition, ePosition) = Translate2LocalNotation(position);
            if (chunks.ContainsKey(cPosition) && Chunk<VisualizerData>.CheckBounds(ePosition))
            {
                chunks[cPosition][ePosition] = value;
                return true;
            }

            return false;
        }
    }
}