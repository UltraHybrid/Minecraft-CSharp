using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain.TrialVersion.Blocks
{
    public class BlockType
    {
        public readonly string Name;
        public readonly int Hardness;
        public readonly TextureInfo Textures;
        public readonly Basis Form;


        public BlockType(string name, int hardness, TextureInfo textures, Basis form)
        {
            Name = name;
            Hardness = hardness;
            Textures = textures;
            Form = form;
        }
    }
}