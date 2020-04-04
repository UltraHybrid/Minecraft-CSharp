namespace tmp
{
    public class BlockType
    {
        public readonly string Name;
        public readonly int Hardness;
        public readonly TextureInfo Textures;


        public BlockType(string name, int hardness, TextureInfo textures)
        {
            Name = name;
            Hardness = hardness;
            Textures = textures;
        }
    }
}