﻿using System;
using System.Threading;
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
            ThreadPool.SetMaxThreads(4,4);
            ThreadPool.SetMinThreads(4, 4);
            var startOffset = PointI.CreateXZ(1, 1);
            var worldSize = 30;
            var manager = new WorldManager2(new PerlinChunkGenerator(UsageGenerators.CoreGenerator));
            //var manager = new WorldManager(new RandomGenerator());
            var game = new Game(worldSize, startOffset, manager);
            var visualWorld = new VisualWorld(startOffset, worldSize);
            var visualManager = new VisualManager3(new Visualizer(game.World), visualWorld);
            //manager.AddAlert += (ch) => Console.WriteLine("Generate " + ch.Position);
            manager.AddAlert += visualManager.HandlerForAdd;
            
            game.Start();
            
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