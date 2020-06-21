using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure.SimpleMath;

namespace tmp.Domain.Commands
{
    public class PutCommand : ICommand
    {
        private readonly Game game;

        public PutCommand(Game game)
        {
            this.game = game;
        }

        public void Execute()
        {
            var mover = game.Player.Mover;
            var point = mover.Position.Add(new PointF(0, game.Player.Height, 0));
            var line = new Line(point, mover.Front);
            var p = new Piece(game.World, mover.Position.AsPointL(), 4);
            var playerPointL = point.AsPointL();
            var piece = new Piece(game.World, playerPointL, 5);
            var gg = piece.Helper()
                .Where(p => p.block != null && p.block != Block.Either)
                .OrderBy(p => playerPointL.GetDistance(p.position))
                .FirstOrDefault(p => Block.GetGeometry(p.block.BlockType, p.position).IsIntersect(line));
            var neigbors = new[]
            {
                new PointL(1, 0, 0), new PointL(-1, 0, 0),
                new PointL(0, 1, 0), new PointL(0, -1, 0),
                new PointL(0, 0, 1), new PointL(0, 0, -1)
            };
            if (gg != default((PointL, Block)))
            {
                var result = neigbors.Select(n => gg.position.Add(n))
                    .OrderBy(n => playerPointL.GetDistance(n))
                    .Where(p => piece.GetItem(p) == null)
                    .FirstOrDefault(p => Block.GetGeometry(BaseBlocks.Dirt, p).IsIntersect(line));
                Console.WriteLine(result);
                if (result != default)
                    game.PutBlock(game.Player.ActiveBlock, result);
            }

            Console.WriteLine(gg);
        }
    }

    public class BreakCommand : ICommand
    {
        private readonly Game game;

        public BreakCommand(Game game)
        {
            this.game = game;
        }

        public void Execute()
        {
            var mover = game.Player.Mover;
            var point = mover.Position.Add(new PointF(0, game.Player.Height, 0));
            var line = new Line(point, mover.Front);
            var p = new Piece(game.World, mover.Position.AsPointL(), 4);
            var playerPointL = point.AsPointL();
            var piece = new Piece(game.World, playerPointL, 5);
            var gg = piece.Helper()
                .Where(p => p.block != null && p.block != Block.Either)
                .OrderBy(p => playerPointL.GetDistance(p.position))
                .FirstOrDefault(p => Block.GetGeometry(p.block.BlockType, p.position).IsIntersect(line));
            if (gg != default((PointL, Block)))
            {
                game.PutBlock(null, gg.position);
            }

            Console.WriteLine("Delete " + gg);
        }
    }

    public class SwapBlock : ICommand
    {
        private readonly IReadOnlyList<BlockType> allBlockType = BaseBlocks.AllBlocks;
        private readonly Player player;
        private readonly int index;

        public SwapBlock(Player player, int number)
        {
            this.player = player;
            index = number % allBlockType.Count;
            Console.WriteLine(index);
        }

        public void Execute()
        {
            Console.WriteLine(allBlockType[index].Name);
            player.ActiveBlock = allBlockType[index];
        }
    }
}