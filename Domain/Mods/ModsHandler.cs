using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tmp.Mods.Parsers;

namespace tmp.Domain.Mods
{
    class ModsHandler
    {
        // TODO: необходимо написать путь до папки с модами
        private readonly DirectoryInfo modsDirectory = new DirectoryInfo("");

        // Название папки с блоками в моде
        private const string blocks = "blocks";

        public IEnumerable<BlockType> GetBlocksFromMod(DirectoryInfo modDirectory)
        {
            return new DirectoryInfo(Path.Combine(modDirectory.FullName, blocks))
                .GetDirectories()
                .Select(dir => BlockParser.GetBlock(dir));
        }

        public IEnumerable<BlockType> GetBlocksFromAllMods(DirectoryInfo? modsDirectory = null)
        {
            if (modsDirectory is null)
                modsDirectory = this.modsDirectory;

            return modsDirectory
                .GetDirectories()
                .SelectMany(dir => GetBlocksFromMod(dir));
        }
    }
}
