using System;
using System.Collections.Generic;
using System.Numerics;
using MinecraftSharp.Domain.Entity;
using MinecraftSharp.Domain.TrialVersion.Blocks;
using MinecraftSharp.Infrastructure;
using MinecraftSharp.Infrastructure.SimpleMath;

namespace MinecraftSharp.Domain
{
    using BlockWorld = World<Chunk<Block>, Block>;

    public class Game : IGame
    {
        public BlockWorld World { get; }
        public Player Player { get; private set; }

        private readonly IWorldManager manager;

        private readonly IGenerator<Chunk<Block>, List<PointI>> animalSpawner;
        public List<Cow> Animals;

        public Game(BlockWorld world,
            IWorldManager manager,
            IGenerator<Chunk<Block>, List<PointI>> animalSpawner)
        {
            this.manager = manager;
            this.animalSpawner = animalSpawner;
            //manager.AddAlert += SpawnAnimals;
            World = world;
            Animals = new List<Cow>();
        }

        public void Start()
        {
            var ready = manager.MakeFirstLaunch();
            Console.WriteLine("Сгенерирован первый чанк");
            Player = new Player("Player", DefineSpawn(ready), new Vector3(1, 0, 0), 10);
        }

        public void PutBlock(BlockType blockType, PointL position)
        {
            manager.PutBlock(blockType, position);
        }

        public void Update(float time)
        {
            manager.Update();
            /*Animals.ForEach(a => a.Follow(Player.Mover.Position.AsPointL(),
                new Piece(World, a.Mover.Position.AsPointL(), 5, Animals),
                time, 2));*/
        }

        /*private void SpawnAnimals(Chunk<Block> chunk)
        {
            foreach (var point in animalSpawner.Generate(chunk))
            {
                var animalPoint = World<Chunk<Block>, Block>.GetAbsolutePosition(point, chunk.Position).AsPointF();
                Animals.Add(new Cow(animalPoint));
            }
        }*/

        private PointF DefineSpawn(PointI ready)
        {
            var chunk = World[ready];
            var (xL, yL, zL) = Chunk<Block>.GetSize;
            foreach (var (x, z) in Utils.DualFor(xL, zL))
                for (var y = yL - 3; y > 0; y--)
                {
                    var p0 = new PointI(x, y, z);
                    var p1 = new PointI(x, y + 1, z);
                    var p2 = new PointI(x, y + 2, z);
                    if (chunk[p0] != null && chunk[p1] == null && chunk[p2] == null)
                        return (BlockWorld.GetAbsolutePosition(p1, ready).AsVector() + new Vector3(0.5f, 0, 0.5f))
                            .AsPointF();
                }

            throw new ArgumentException("Не удалось найти подходящего для спавна места", ready.ToString());
        }
    }
}