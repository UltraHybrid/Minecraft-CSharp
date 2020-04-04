using System;
using System.Linq;

namespace tmp
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            
            
            var player = new Player(new Vector(0, 10, 0),
                new Vector(0, 0, 1), 10, 15);

            var coreGenerator = new PerlinHighGenerator(0.01f, 0.05f, 3.0f, 8);
            var otherGenerator = new PerlinHighGenerator(0.02f, 0.01f, 10.0f, 7);
            var otherGenerator2 = new PerlinHighGenerator(0.43f, (1 / 215.0f), 20.0f, 9);

            var world = new World(new PerlinChunkGenerator(coreGenerator), 3);

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