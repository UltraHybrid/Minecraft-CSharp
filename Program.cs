using System;
using System.Linq;

namespace tmp
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            TextureInfo.Order = new[]
            {
                TextureOrder.Left, TextureOrder.Back, TextureOrder.Right,
                TextureOrder.Top, TextureOrder.Front, TextureOrder.Bottom
            };

            var coreGenerator = new PerlinHighGenerator(0.01f, 0.05f, 3.0f, 8);
            var otherGenerator = new PerlinHighGenerator(0.02f, 0.01f, 10.0f, 7);
            var otherGenerator2 = new PerlinHighGenerator(0.43f, (1 / 215.0f), 20.0f, 9);
            var uGenerator = new Perlin3DChunkGenerator(
                new PerlinHighGenerator(0.06f, 0.4f, 3.5f, 3) {Seed = 114.78f}
            );

            var startPoint = new PointI(300, 0, 300);
            var world = new World(10, startPoint);
            var manager = new ChunkManager(new PerlinChunkGenerator(coreGenerator));
            manager.SetWorld(world);
            manager.Notify += (ch) =>
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Was Create: " + ch.Position);
                Console.ForegroundColor = ConsoleColor.White;
            };
            var visualMap = new VisualMap(world.Size, world.globalOffset, new WorldVisualiser(world));
            manager.Notify += (ch) => visualMap.Offset = world.globalOffset;
            manager.Notify += visualMap.HandleNewChunk;
            var game = new Game(manager, world);

            Console.Beep();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            var memory = GC.GetTotalMemory(true);
            Console.WriteLine("World size: " + memory / (1024 * 1024) + " Mb");
            Console.Beep();

            using var painter = new Window(game, visualMap);
            painter.Run(200, 200);
        }
    }
}