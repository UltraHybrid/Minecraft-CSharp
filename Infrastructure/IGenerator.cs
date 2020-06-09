namespace tmp.Infrastructure
{
    public interface IGenerator<TSource, TResult>
    {
        TResult Generate(TSource x, TSource z);
    }
}