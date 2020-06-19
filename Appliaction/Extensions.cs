using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using tmp.Domain;
using tmp.Domain.TrialVersion;
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

        public static (List<Vector3> positions, List<Vector2> textureData) AdaptToStupidData(
            this Chunk<VisualizerData> chunk)
        {
            var positions = new List<Vector3>();
            var textureData = new List<Vector2>();
            foreach (var visData in chunk.Where(x => x != null))
            {
                foreach (var face in visData.Faces)
                {
                    positions.Add(visData.Position.Convert());
                    textureData.Add(new Vector2(face.Number, Texture.textures[face.Name]));
                }
            }

            return (positions, textureData);
        }


        public static Vector3 Convert(this System.Numerics.Vector3 vectorLast)
        {
            return new Vector3(vectorLast.X, vectorLast.Y, vectorLast.Z);
        }
        
        public static Vector3 Convert(this PointF vectorLast)
        {
            return new Vector3(vectorLast.X, vectorLast.Y, vectorLast.Z);
        }
    }
}