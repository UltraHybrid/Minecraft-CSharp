using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Ninject;
using Ninject.Modules;
using tmp.Domain;
using tmp.Domain.Generators;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;
using tmp.Logic;

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

            //Bind<IGenerator<PointI, ChunkB>>().To<LandGenerator>()
            //   .WhenInjectedInto<WorldGenerator>()
            //    .Named("Land")
            //    .WithConstructorArgument(UsageGenerators.CoreGenerator);

            //Bind<IGenerator<PointI, ChunkB>>().To<WorldGenerator>()
            //    .WithConstructorArgument(new LandGenerator(UsageGenerators.CoreGenerator));
            //.WhenInjectedInto<WorldManager>();

            //Bind<IGenerator<ChunkB, ChunkB>>().To<OreGenerator>()
            //    .WithConstructorArgument(UsageGenerators.OreCoreGenerator);

            //Bind<IGenerator<ChunkB, ChunkB>>().To<SimpleTreeSpawner>();

            //Bind<IGenerator<ChunkB, ChunkB>>().To<BedrockGenerator>()
            //    .WithConstructorArgument(UsageGenerators.BedrockCoreGenerator);

            Bind<IGenerator<ChunkB, List<PointB>>>().To<CowSpawner>();

            //Bind<IGenerator<PointI, Chunk<Block>>>().To<WorldGenerator>().WithConstructorArgument()
            Bind<GameWorld>().ToSelf().InSingletonScope();
            Bind<World<ChunkB, Block>>().To<GameWorld>().InSingletonScope();
            Bind<IWorldManager>().To<WorldManager>()
                .InSingletonScope()
                .WithConstructorArgument(typeof(IGenerator<PointI, ChunkB>),
                    c => new WorldGenerator(new LandGenerator(UsageGenerators.CoreGenerator),
                        new IGenerator<ChunkB, ChunkB>[] {new SimpleTreeSpawner(),}))
                .WithConstructorArgument(typeof(World<Chunk<Block>, Block>),
                    c => c.Kernel.Get<World<ChunkB, Block>>());

            Bind<Game>().ToSelf().InSingletonScope();
        }
    }

    public class GraphicModule : NinjectModule
    {
        public override void Load()
        {
            Bind<World<VisualChunk, VisualizerData>>().To<VisualWorld>().InSingletonScope();
            Bind<IVisualizer<Block>>().To<Visualizer>().InSingletonScope();
            Bind<VisualManager3>().ToSelf().InSingletonScope();
            Bind<Window>().ToSelf().InSingletonScope();
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
            var visManager = container.Get<VisualManager3>();
            manager.AddAlert += visManager.HandlerForAdd;
            manager.UpdateAlert += visManager.HandlerForUpdate;
            //g.MakeFirstLunch();
            var window = container.Get<Window>();
            window.Run(200,200);
            /*var startOffset = PointI.CreateXZ(1, 1);
            var worldSize = 10;
            var gg = new WorldGenerator(
                new LandGenerator(UsageGenerators.CoreGenerator),
                new OreGenerator(UsageGenerators.OreCoreGenerator),
                new SimpleTreeSpawner(),
                new BedrockGenerator(UsageGenerators.BedrockCoreGenerator)
                );
            //var gg=new WorldGenerator(new PerlinChunkGenerator(UsageGenerators.CoreGenerator), new SimpleTreeSpawner());
            //var manager = new WorldManager2(new PerlinChunkGenerator(UsageGenerators.CoreGenerator));
            var manager = new WorldManager(gg);
            var game = new Game(worldSize, startOffset, manager);*/
            //var visualWorld = new VisualWorld(startOffset, worldSize);
            //var visualManager = new VisualManager3(new Visualizer(game.World), visualWorld);
            //manager.AddAlert += visualManager.HandlerForAdd;
            //manager.UpdateAlert += visualManager.HandlerForUpdate;

            Console.Beep();

            //container.Bind<Window>().ToSelf().OnActivation(window => window.Run(200, 200));
            //container.Get<Window>();

            //using var painter = new Window(game, visualManager);
            //painter.Run(200, 200);
        }
    }
}