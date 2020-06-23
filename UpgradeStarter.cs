﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Ninject;
using Ninject.Modules;
using NUnit.Framework;
using OpenTK;
using tmp.Domain;
using tmp.Domain.Generators;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;
using tmp.Logic;
using tmp.Rendering;

namespace tmp
{
    using ChunkB = Chunk<Block>;
    using BlockWorld = World<Chunk<Block>, Block>;

    public class GameModule : NinjectModule
    {
        public override void Load()
        {
            Bind<int>().ToConstant(10);
            Bind<PointI>().ToConstant(PointI.CreateXZ(1, 1));
            Bind<BlockWorld>().To<GameWorld>().InSingletonScope();

            Bind<LandGenerator>().ToSelf()
                .WithConstructorArgument((typeof(IGenerator<PointF, float>)), UsageGenerators.CoreGenerator);
            Bind<IGenerator<ChunkB, ChunkB>>().To<OreGenerator>()
                .WithConstructorArgument((typeof(IGenerator<PointF, float>)), UsageGenerators.OreCoreGenerator);
            Bind<IGenerator<ChunkB, ChunkB>>().To<SimpleTreeSpawner>();
            Bind<IGenerator<ChunkB, ChunkB>>().To<BedrockGenerator>()
                .WithConstructorArgument((typeof(IGenerator<PointF, float>)), UsageGenerators.BedrockCoreGenerator);
            Bind<IGenerator<PointI, ChunkB>>().To<WorldGenerator>()
                .WhenInjectedInto<IWorldManager>()
                .WithConstructorArgument(typeof(IGenerator<PointI, ChunkB>),
                    c => c.Kernel.Get<LandGenerator>());

            Bind<IWorldManager>().To<WorldManager>().InSingletonScope()
                .WithConstructorArgument(c => c.Kernel.Get<World<ChunkB, Block>>());
            Bind<IGenerator<ChunkB, List<PointB>>>().To<CowSpawner>();
            Bind<Game>().ToSelf().InSingletonScope();
        }
    }

    public class GraphicModule : NinjectModule
    {
        public override void Load()
        {
            Bind<VisualWorld>().ToSelf().InSingletonScope();
            Bind<IVisualizer<Block>>().To<Visualizer>();
            Bind<IVisualManager>().To<VisualManager>().InSingletonScope();
            Bind<GameWindow>().To<Window>().InSingletonScope();
        }
    }

    public static class UpgradeStarter2
    {
        public static void Main(string[] args)
        {
            var threads = Environment.ProcessorCount;
            ThreadPool.SetMaxThreads(threads, threads);
            ThreadPool.SetMinThreads(threads, threads);
            TextureInfo.Order = new[]
            {
                TextureSide.Left, TextureSide.Back, TextureSide.Right,
                TextureSide.Top, TextureSide.Front, TextureSide.Bottom
            };

            var container = new StandardKernel(new GameModule(), new GraphicModule());

            var manager = container.Get<IWorldManager>();
            var visualManager = container.Get<IVisualManager>();
            manager.AddAlert += visualManager.HandlerForAdd;
            manager.UpdateAlert += visualManager.HandlerForUpdate;

            using var window = container.Get<GameWindow>();
            window.Run(200, 200);
        }

        #region OldMain

        public static void Main1(string[] args)
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
            var worldSize = 15;
            var gg = new WorldGenerator(
                new LandGenerator(UsageGenerators.CoreGenerator),
                new IGenerator<ChunkB, ChunkB>[]
                {
                    new OreGenerator(UsageGenerators.OreCoreGenerator),
                    new SimpleTreeSpawner(),
                    new BedrockGenerator(UsageGenerators.BedrockCoreGenerator)
                }
            );
            //var gg=new WorldGenerator(new PerlinChunkGenerator(UsageGenerators.CoreGenerator), new SimpleTreeSpawner());
            //var manager = new WorldManager2(new PerlinChunkGenerator(UsageGenerators.CoreGenerator));
            //var gg = new FlatGenerator();
            var world = new GameWorld(startOffset, worldSize);
            var manager = new WorldManager(world, gg);
            var game = new Game(world, manager, new CowSpawner());


            var visualWorld = new VisualWorld(startOffset, worldSize);
            var visualManager = new VisualManager(new Visualizer(game.World), visualWorld);
            manager.AddAlert += visualManager.HandlerForAdd;
            manager.UpdateAlert += visualManager.HandlerForUpdate;

            Console.Beep();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            var memory = GC.GetTotalMemory(true);
            Console.WriteLine("World size: " + memory / (1024 * 1024) + " Mb");
            Console.Beep();


            using var painter = new Window(game,
                visualManager);
            painter.Run(200, 200);
        }

        #endregion
    }
}