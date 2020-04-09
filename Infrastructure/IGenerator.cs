namespace tmp
{
    public interface IGenerator<TSource, TResult>
    {
        TResult Generate(TSource x, TSource z);
    }
}