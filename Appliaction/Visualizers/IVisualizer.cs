namespace tmp
{
    public interface IVisualizer<TSource, TResult>
    {
        TResult GetVisibleFaces(TSource data);
    }
}