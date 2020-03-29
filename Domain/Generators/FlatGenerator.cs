namespace tmp
{
    public class FlatGenerator : IGenerator
    {
        public Chunk Generate(int x, int z)
        {
            var chunk = new Chunk();
            for (var i = 0; i < Chunk.XLenght; i++)
            for (var j = 0; j < 5; j++)
            for (var k = 0; k < Chunk.ZLength; k++)
            {
                var position = new Point3(i, j, k);
                chunk[position] = j switch
                {
                    0 => new Block(BaseBlocks.Bedrock, position),
                    4 => new Block(BaseBlocks.Grass, position),
                    _ => new Block(BaseBlocks.Dirt, position)
                };
            }

            return chunk;
        }

        public Point3 GetHigh(Point3 point)
        {
            throw new System.NotImplementedException();
        }
    }
}