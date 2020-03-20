using System;
using OpenTK;
using tmp.TrialVersion;

namespace tmp
{
    class Program
    {
        private static void Main(string[] args)
        {
            var game = new Window();
            game.Run(200, 200);   
        }
    }
}