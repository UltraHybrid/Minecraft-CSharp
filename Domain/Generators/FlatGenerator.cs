namespace tmp
{
    public class FlatGenerator : IGenerator<Chunk>
    {
        public Chunk Generate(int x, int z)
        {
            var chunk = new Chunk();
            for (byte i = 0; i < Chunk.XLenght; i++)
            for (byte j = 0; j < 5; j++)
            for (byte k = 0; k < Chunk.ZLength; k++)
            {
                var position = new PointB(i, j, k);
                chunk[position] = j switch
                {
                    0 => new Block(BaseBlocks.Bedrock, position),
                    4 => new Block(BaseBlocks.Grass, position),
                    _ => new Block(BaseBlocks.Dirt, position)
                };
            }

            return chunk;
        }
    }
}