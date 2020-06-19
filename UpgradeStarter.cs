using System;
using System.IO;
using System.Linq;
using System.Threading;
using tmp.Domain;
using tmp.Domain.Generators;
using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;
using tmp.Logic;

namespace tmp
{
    public static class UpgradeStarter2
    {
        public static void Main(string[] args)
        {
            TextureInfo.Order = new[]
            {
                TextureSide.Left, TextureSide.Back, TextureSide.Right,
                TextureSide.Top, TextureSide.Front, TextureSide.Bottom
            };
            var threads = Environment.ProcessorCount;
            ThreadPool.SetMaxThreads(threads, threads);
            ThreadPool.SetMinThreads(threads, threads);
            var startOffset = PointI.CreateXZ(1, 1);
            var worldSize = 10;
            var gg = new WorldGenerator(
                new LandGenerator(UsageGenerators.CoreGenerator),
                new OreGenerator(UsageGenerators.OreCoreGenerator),
                new SimpleTreeSpawner());
            //var gg=new WorldGenerator(new PerlinChunkGenerator(UsageGenerators.CoreGenerator), new SimpleTreeSpawner());
            //var manager = new WorldManager2(new PerlinChunkGenerator(UsageGenerators.CoreGenerator));
            var manager = new WorldManager(gg);
            var game = new Game(worldSize, startOffset, manager);
            var visualWorld = new VisualWorld(startOffset, worldSize);
            var visualManager = new VisualManager3(new Visualizer(game.World), visualWorld);
            manager.AddAlert += visualManager.HandlerForAdd;
            
            Console.Beep();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            var memory = GC.GetTotalMemory(true);
            Console.WriteLine("World size: " + memory / (1024 * 1024) + " Mb");
            Console.Beep();

            using var painter = new Window(game, visualManager);
            painter.Run(200, 200);
        }
    }
}