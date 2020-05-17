namespace tmp.Logic
{
    public class VisualWorld : World<VisualizerData>
    {
        public VisualWorld(PointI startOffset, int size) : base(startOffset, size)
        {
        }

        public override VisualizerData GetItem(PointI position)
        {
            var (cPosition, elementPosition) = Translate2LocalNotation(position);
            if (chunks.ContainsKey(cPosition) && Chunk<VisualizerData>.CheckBounds((PointB) elementPosition))
                return chunks[cPosition][(PointB) elementPosition];
            return null;
        }

        public override bool TrySetItem(PointI position, VisualizerData value)
        {
            var (cPosition, elementPosition) = Translate2LocalNotation(position);
            if (chunks.ContainsKey(cPosition) && Chunk<VisualizerData>.CheckBounds((PointB) elementPosition))
            {
                chunks[cPosition][(PointB) elementPosition] = value;
                return true;
            }

            return false;
        }
    }
}