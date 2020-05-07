using System;
using OpenTK;

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
    }
}