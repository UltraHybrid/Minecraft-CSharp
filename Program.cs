using System;

namespace tmp
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var player = new Player(new Vector(0, 30, 0),
                new Vector(1, 0, 0), 10, 15);

            var coreGenerator = new PerlinHighGenerator(0.01f, 0.05f, 3.0f, 8);
            var world = new World(new PerlinChunkGenerator(coreGenerator));

            Console.Beep();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            var memory = GC.GetTotalMemory(true);
            Console.WriteLine("World size: " + memory / (1024 * 1024) + " Mb");
            Console.Beep();

            using var game = new Window(world, player);
            game.Run(200, 200);
        }
    }
}