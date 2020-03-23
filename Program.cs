using System;
using OpenTK;
using tmp.TrialVersion;

namespace tmp
{
    class Program
    {
        private static void Main(string[] args)
        {
            var player = new Player(new Vector(0, 0, 0),
                new Vector(0, 0, 1), 10, 7);
            var game = new Window(player);
            game.Run(200, 200);
        }
    }
}