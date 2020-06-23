namespace MinecraftSharp.Infrastructure
{
    public interface IGenerator<TSource, TResult>
    {
        TResult Generate(TSource source);
    }
}