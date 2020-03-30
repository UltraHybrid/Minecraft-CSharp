namespace tmp
{
    public class ChunkManager : IChunkManager
    {
        private readonly IGenerator<Chunk> generator;

        public ChunkManager(IGenerator<Chunk> generator)
        {
            this.generator = generator;
        }

        public Chunk Load(int x, int z)
        {
            throw new System.NotImplementedException();
        }

        public Chunk Create(int x, int z)
        {
            throw new System.NotImplementedException();
        }

        public void Save(Chunk chunk)
        {
            throw new System.NotImplementedException();
        }
    }
}