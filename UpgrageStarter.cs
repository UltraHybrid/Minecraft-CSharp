using System;

namespace tmp
{
    public static class UpgrageStarter
    {
        public static void Main(string[] args)
        {
            TextureInfo.Order = new[]
            {
                TextureSide.Left, TextureSide.Back, TextureSide.Right,
                TextureSide.Top, TextureSide.Front, TextureSide.Bottom
            };

            var startOffset = PointI.CreateXZ(0, 0);
            var worldSize = 30;
            var manager = new ChunkManager2(new PerlinChunkGenerator(UsageGenerators.CoreGenerator));
            var game = new Game2(worldSize, startOffset, manager);
            var visualMap = new VisualMap(worldSize, startOffset, new WorldVisualizer3(game.World));
            manager.Notify += (ch) => Console.WriteLine("Generate " + ch.Position);
            manager.Notify += visualMap.HandleNewChunk;
            game.Start();
            
            Console.Beep();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            var memory = GC.GetTotalMemory(true);
            Console.WriteLine("World size: " + memory / (1024 * 1024) + " Mb");
            Console.Beep();

            using var painter = new Window(game, visualMap);
            painter.Run(100, 200);
        }
    }
}