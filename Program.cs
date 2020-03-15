using OpenTK;

namespace tmp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var game = new Window())
            {
                game.Run(125, 200);
            }
        }
    }
}