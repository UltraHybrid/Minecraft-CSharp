using tmp.Interfaces;

namespace tmp
{
    public class GameWorld : World<Chunk<Block>, Block>
    {
        public GameWorld(PointI startOffset, int size) : base(startOffset, size)
        {
        }

        public override Block GetItem(PointI position)
        {
            var (cPosition, bPosition) = Translate2LocalNotation(position);
            if (chunks.ContainsKey(cPosition) && Chunk<Block>.CheckBounds((PointB) bPosition))
                return chunks[cPosition][(PointB) bPosition];
            return Block.Either;
        }

        public override bool TrySetItem(PointI position, Block value)
        {
            var (cPosition, bPosition) = Translate2LocalNotation(position);
            if (!chunks.ContainsKey(cPosition) || !Chunk<Block>.CheckBounds((PointB) bPosition)) return false;
            chunks[cPosition][(PointB) bPosition] = value;
            return true;
        }
    }
}