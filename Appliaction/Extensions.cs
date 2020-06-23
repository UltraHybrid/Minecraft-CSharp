using System;
using MinecraftSharp.Infrastructure.SimpleMath;
using OpenTK;

namespace MinecraftSharp
{
    public static class Extensions
    {
        public static Vector3 Convert(this PointI point)
        {
            return new Vector3(point.X, point.Y, point.Z);
        }

        public static void Print(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static Vector3 Convert(this System.Numerics.Vector3 vectorLast)
        {
            return new Vector3(vectorLast.X, vectorLast.Y, vectorLast.Z);
        }

        public static Vector3 Convert(this PointF point)
        {
            return new Vector3(point.X, point.Y, point.Z);
        }
    }
}