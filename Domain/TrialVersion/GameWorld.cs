using MinecraftSharp.Domain.TrialVersion.Blocks;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Domain
{
    public class GameWorld : World<Chunk<Block>, Block>
    {
        public GameWorld(PointI startOffset, int size) : base(startOffset, size)
        {
        }

        public override Block GetItem(PointL position)
        {
            var (cPosition, bPosition) = Translate2LocalNotation(position);
            if (chunks.ContainsKey(cPosition) && Chunk<Block>.CheckBounds(bPosition))
                return chunks[cPosition][bPosition];
            return Block.Either;
        }

        public override bool TrySetItem(PointL position, Block value)
        {
            var (cPosition, bPosition) = Translate2LocalNotation(position);
            if (!chunks.ContainsKey(cPosition) || !Chunk<Block>.CheckBounds(bPosition)) return false;
            chunks[cPosition][bPosition] = value;
            return true;
        }
    }
}