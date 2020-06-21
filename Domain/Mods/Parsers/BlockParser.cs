using System.IO;

namespace tmp.Mods.Parsers
{
    class BlockParser
    {
        // Названия файлов с текстурами кторон блока
        private const string left = "left";
        private const string back = "back";
        private const string right = "right";
        private const string front = "front";
        private const string top = "top";
        private const string bottom = "bottom";

        // Название файла с информацией о блоке
        private const string info = "info";

        // Названия полей в конфигурационном файле
        private const string hardness = "hardness";
        private const string name = "name";

        public static BlockType GetBlock(DirectoryInfo directoryBlock)
        {
            var textureInfo = new TextureInfo(
                Path.Combine(directoryBlock.FullName, left),
                Path.Combine(directoryBlock.FullName, back),
                Path.Combine(directoryBlock.FullName, right),
                Path.Combine(directoryBlock.FullName, front),
                Path.Combine(directoryBlock.FullName, top),
                Path.Combine(directoryBlock.FullName, bottom)
            );

            var config = ConfigParser.Parse(Path.Combine(directoryBlock.FullName, info));

            return new BlockType(config[name], int.Parse(config[hardness]), textureInfo);
        }
    }
}
