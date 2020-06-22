﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using tmp.Domain.Entity;
using tmp.Domain.Generators;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain
{
    using BlockWorld = World<Chunk<Block>, Block>;

    public class Game: IGame
    {
        public World<Chunk<Block>, Block> World { get; }
        public Player Player { get; private set; }
        private readonly IWorldManager manager;
        private readonly IGenerator<Chunk<Block>, List<PointB>> animalSpawner;
        public List<Cow> Animals;

        public Game(World<Chunk<Block>, Block> world,
            IGenerator<Chunk<Block>, List<PointB>> animalSpawner,
            IWorldManager manager)
        {
            this.animalSpawner = animalSpawner;
            this.manager = manager;
            World = world;
            Animals = new List<Cow>();
        }

        public void Start()
        {
            manager.AddAlert += SpawnAnimals;
            var ready = manager.MakeFirstLunch();
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
            //Animals.ForEach(a => a.Follow(Player.Mover.Position.AsPointL(),
            //    new Piece(World, a.Mover.Position.AsPointL(), 5),
            //    time, 2));
        }

        private void SpawnAnimals(Chunk<Block> chunk)
        {
            foreach (var point in animalSpawner.Generate(chunk))
            {
                var animalPoint = World<Chunk<Block>, Block>.GetAbsolutePosition(point, chunk.Position).AsPointF();
                Animals.Add(new Cow(animalPoint));
            }
        }

        private PointF DefineSpawn(PointI ready)
        {
            var chunk = World[ready];
            for (byte x = 0; x < Chunk<Block>.XLength; x++)
            {
                for (byte z = 0; z < Chunk<Block>.ZLength; z++)
                {
                    for (byte y = 253; y > 0; y--)
                    {
                        var p0 = new PointB(x, y, z);
                        var p1 = new PointB(x, (byte) (y + 1), z);
                        var p2 = new PointB(x, (byte) (y + 2), z);
                        if (chunk[p0] != null && chunk[p1] == null && chunk[p2] == null)
                            return (BlockWorld.GetAbsolutePosition(p1, ready).AsVector() + new Vector3(0.5f, 0, 0.5f))
                                .AsPointF();
                    }
                }
            }

            throw new ArgumentException("Не удалось найти подходящего для спавна места", ready.ToString());
        }
    }
}