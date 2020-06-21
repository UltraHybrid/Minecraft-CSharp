using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using tmp.Domain;
using tmp.Domain.TrialVersion;
using tmp.Infrastructure;
using tmp.Infrastructure.SimpleMath;
using tmp.Logic;

namespace tmp
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