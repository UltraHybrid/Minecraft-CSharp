using System;
using System.Linq;

namespace tmp
{
    public static class Program
    {
        private static void Main1(string[] args)
        {
            TextureInfo.Order = new[]
            {
                TextureSide.Left, TextureSide.Back, TextureSide.Right,
                TextureSide.Top, TextureSide.Front, TextureSide.Bottom
            };
            
            var startPoint = new PointI(300, 0, 300);
            var world = new World(10, startPoint);
            var manager = new ChunkManager(new PerlinChunkGenerator(UsageGenerators.CoreGenerator));
            manager.SetWorld(world);
            manager.Notify += (ch) =>
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Was Create: " + ch.Position);
                Console.ForegroundColor = ConsoleColor.White;
            };
            var visualMap = new VisualMap(world.Size, world.GlobalOffset, new WorldVisualiser(world));
            manager.Notify += (ch) => visualMap.Offset = world.GlobalOffset;
            manager.Notify += visualMap.HandleNewChunk;
            var game = new Game(manager, world);

            Console.Beep();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            var memory = GC.GetTotalMemory(true);
            Console.WriteLine("World size: " + memory / (1024 * 1024) + " Mb");
            Console.Beep();

            //using var painter = new Window(game, visualMap);
            //painter.Run(200, 200);
        }
    }
}