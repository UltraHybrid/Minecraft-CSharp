using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NUnit.Framework.Constraints;
using tmp.Domain.TrialVersion.Blocks;
using tmp.Infrastructure.SimpleMath;
using Plane = System.Numerics.Plane;

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
            var cameraPos = mover.Position.Add(new PointF(0, game.Player.Height, 0));
            var line = new Line(cameraPos, mover.Front);
            var piece = new Piece(game.World, cameraPos.AsPointL(), 4);
            var closestBlock = piece.Helper()
                .Where(pair => pair.block != null && pair.block != Block.Either)
                .Where(pair => Block.GetGeometry(pair.block.BlockType, pair.position).IsIntersect(line))
                .OrderBy(pair => cameraPos.GetSquaredDistance(pair.position.AsVector().AsPointF().Add(new Vector3(0.5f))))
                .FirstOrDefault();
            if (closestBlock == default((PointL, Block))) return;
            var planes = Block.GetGeometry(closestBlock.block.BlockType, closestBlock.position)
                    .GetPlanes().ToArray();
            var ordered = planes
                .Select(plane => (plane, plane.CalculateIntersectionPoint(line)))
                .Where(pair => pair.Item2 != null)
                .OrderBy(pair => cameraPos.GetSquaredDistance(pair.Item2.Value))
                .ToList();
            if (!ordered.Any()) return;
            var closestPlane = ordered.First();
            var index = Array.IndexOf(planes, closestPlane.plane);
            var finalCoords = closestBlock.position.GetNeighbours().ToArray()[index];
            if (piece.GetItem(finalCoords) != null) return;
            var blockGeometry = Block.GetGeometry(game.Player.ActiveBlock, finalCoords);
            if (game.Player.Mover.Geometry.IsCollision(blockGeometry)) return;
            game.PutBlock(game.Player.ActiveBlock, finalCoords);
            /*var mover = game.Player.Mover;
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

            Console.WriteLine(gg);*/
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
            var cameraPos = mover.Position.Add(new PointF(0, game.Player.Height, 0));
            var line = new Line(cameraPos, mover.Front);
            var piece = new Piece(game.World, cameraPos.AsPointL(), 4);
            var closestBlock = piece.Helper()
                .Where(pair => pair.block != null && pair.block != Block.Either)
                .Where(pair => Block.GetGeometry(pair.block.BlockType, pair.position).IsIntersect(line))
                .OrderBy(pair => cameraPos.GetSquaredDistance(pair.position.AsVector().AsPointF().Add(new Vector3(0.5f))))
                .ToList();
            if (!closestBlock.Any()) return;
            game.PutBlock(null, closestBlock.First().position);

            /*var mover = game.Player.Mover;
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

            Console.WriteLine("Delete " + gg);*/
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