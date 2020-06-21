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
                .OrderBy(pair =>
                    cameraPos.GetSquaredDistance(pair.position.AsPointF().Add(new Vector3(0.5f))))
                .FirstOrDefault();
            if (closestBlock == default((PointL, Block))) return;

            var geometry = Block.GetGeometry(closestBlock.block.BlockType, closestBlock.position);
            foreach (var e in geometry)
            {
                Console.Write(e);
            }

            Console.WriteLine();
            var planes = geometry.GetPlanes().ToArray();

            var intersectPoints = geometry.GetIntersectPoints(line).OrderBy(cameraPos.GetSquaredDistance).ToList();
            if (!intersectPoints.Any()) return;
            var closestPoint = intersectPoints.First();

            var closestPlane = planes.First(p => p.ContainsPoint(closestPoint));
            /*var ordered = planes
                .Select(plane => (plane, plane.CalculateIntersectionPoint(line)))
                .Where(pair => pair.Item2 != null)
                //.Where(pair => geometry.IsOnSurface(pair.Item2.Value))
                .OrderBy(pair => cameraPos.GetSquaredDistance(pair.Item2.Value))
                .ToList();
            if (!ordered.Any()) return;
            var closestPlane = ordered.First();
            var index = Array.IndexOf(planes, closestPlane.plane);*/
            var index = Array.IndexOf(planes, closestPlane);
            var finalCoords = closestBlock.position.GetNeighbours().ToArray()[index];
            if (piece.GetItem(finalCoords) != null) return;
            var blockGeometry = Block.GetGeometry(game.Player.ActiveBlock, finalCoords);
            if (game.Player.Mover.Geometry.IsCollision(blockGeometry)) return;
            game.PutBlock(game.Player.ActiveBlock, finalCoords);
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
                .OrderBy(pair =>
                    cameraPos.GetSquaredDistance(pair.position.AsVector().AsPointF().Add(new Vector3(0.5f))))
                .ToList();
            if (!closestBlock.Any()) return;
            game.PutBlock(null, closestBlock.First().position);
        }
    }

    public class Spectator
    {
        private readonly Game game;
        public Parallelogram figure;

        public Spectator(Game game)
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
                .OrderBy(pair =>
                    cameraPos.GetSquaredDistance(pair.position.AsPointF().Add(new Vector3(0.5f))))
                .FirstOrDefault();
            if (closestBlock == default((PointL, Block))) return;

            var geometry = Block.GetGeometry(closestBlock.block.BlockType, closestBlock.position);
            var planes = geometry.GetPlanes().ToArray();
            Console.WriteLine("ClosestBlock " + closestBlock.position);

            var intersectPoints = geometry.GetIntersectPoints(line).OrderBy(cameraPos.GetSquaredDistance).ToList();
            if (!intersectPoints.Any()) return;
            var closestPoint = intersectPoints.First();

            var closestPlane = planes.First(p => p.ContainsPoint(closestPoint));
            var index = Array.IndexOf(planes, closestPlane);
            figure = geometry.Squads[index];
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
            var amount = Math.Abs(number / allBlockType.Count) + 1;
            index = (number + amount * allBlockType.Count) % allBlockType.Count;
            Console.WriteLine(index);
        }

        public void Execute()
        {
            Console.WriteLine(allBlockType[index].Name);
            player.ActiveBlock = allBlockType[index];
        }
    }
}