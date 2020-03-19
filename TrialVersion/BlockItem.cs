namespace tmp.TrialVersion
{
    public class BlockItem
    {
        public readonly string Name;
        public readonly int Hardness;
        public readonly string TextureName;


        public BlockItem(string name, int hardness, string textureName)
        {
            Name = name;
            Hardness = hardness;
            TextureName = textureName;
        }
    }
}