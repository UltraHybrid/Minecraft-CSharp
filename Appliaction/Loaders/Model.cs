using System.Collections.Generic;
using System.IO;
using Assimp;

namespace MinecraftSharp.Loaders
{
    public static class Model
    {
        private static AssimpContext AssimpContext { get; } = new AssimpContext();
        private const PostProcessSteps Flags = PostProcessSteps.FlipUVs | PostProcessSteps.Triangulate;

        public static Dictionary<string, List<Mesh>> Models { get; } = new Dictionary<string, List<Mesh>>();

        public static void Load(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                var name = Path.GetFileNameWithoutExtension(path);
                Models.Add(name, AssimpContext.ImportFile(path, Flags).Meshes);
            }
        }
    }
}