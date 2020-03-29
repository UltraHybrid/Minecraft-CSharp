namespace tmp
{
    public class BlockType
    {
        public readonly string Name;
        public readonly int Hardness;
        public readonly string[] TextureName;


        public BlockType(string name, int hardness, string[] textureName)
        {
            Name = name;
            Hardness = hardness;
            TextureName = textureName;
        }
    }
}