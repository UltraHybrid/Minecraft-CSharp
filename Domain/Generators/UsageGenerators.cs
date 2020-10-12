using MinecraftSharp.Infrastructure;

namespace MinecraftSharp.Domain.Generators
{
    public static class UsageGenerators
    {
        public static readonly PerlinHighGenerator CoreGenerator =
            new PerlinHighGenerator(0.01f, 0.05f, 3.0f, 8);

        public static readonly PerlinHighGenerator OtherGenerator =
            new PerlinHighGenerator(0.02f, 0.01f, 10.0f, 7);

        public static readonly PerlinHighGenerator OtherGenerator2 =
            new PerlinHighGenerator(0.43f, (1 / 215.0f), 20.0f, 9);

        public static readonly PerlinHighGenerator OreCoreGenerator =
            new PerlinHighGenerator(0.06f, 0.4f, 3.5f, 3) {Seed = 114.78f};

        public static readonly PerlinHighGenerator BedrockCoreGenerator =
            new PerlinHighGenerator(0.7f, 0.8f, 3f, 4) {Seed = 111f};

        /*public static readonly Perlin3DChunkGenerator OreGenerator = new Perlin3DChunkGenerator(
            new PerlinHighGenerator(0.06f, 0.4f, 3.5f, 3) {Seed = 114.78f}
        );*/
    }
}