using System;
using OpenTK;
using tmp.TrialVersion;

namespace tmp
{
    class Program
    {
        static void Main(string[] args)
        {
            var world = new World();
            for (var x = 0; x < World.MaxCount; x++)
            {
                for (var z = 0; z < World.MaxCount; z++)
                {
                    var r= world.GetVisibleBlock(x, z);
                    Console.WriteLine(r["dirt.png"].Count);
                }
            }

            using (var game = new Window())
            {
                
                game.Run(10, 200);
            }
        }
    }
}