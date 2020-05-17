namespace tmp.Logic
{
    public interface IVisualizer<TSource>
    {
        VisualChunk Visualize(Chunk<TSource> worldChunk);
        void UpdateOne(PointI position);
    }
}