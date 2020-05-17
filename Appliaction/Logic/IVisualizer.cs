namespace tmp.Logic
{
    public interface IVisualizer<TSource, TResult>
    {
        Chunk<TResult> Visualize(Chunk<TSource> worldChunk);
        void UpdateOne(PointI position);
    }
}