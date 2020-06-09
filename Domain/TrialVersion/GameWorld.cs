using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    public class GameWorld : World<Chunk<Block>, Block>
    {
        public GameWorld(PointI startOffset, int size) : base(startOffset, size)
        {
        }

        public override Block GetItem(PointI position)
        {
            var (cPosition, bPosition) = Translate2LocalNotation(position);
            var blockPosition = bPosition.AsPointB();
            if (chunks.ContainsKey(cPosition) && Chunk<Block>.CheckBounds(blockPosition))
                return chunks[cPosition][blockPosition];
            return Block.Either;
        }

        public override bool TrySetItem(PointI position, Block value)
        {
            var (cPosition, bPosition) = Translate2LocalNotation(position);
            var blockPosition = bPosition.AsPointB();
            if (!chunks.ContainsKey(cPosition) || !Chunk<Block>.CheckBounds(blockPosition)) return false;
            chunks[cPosition][blockPosition] = value;
            return true;
        }
    }
}