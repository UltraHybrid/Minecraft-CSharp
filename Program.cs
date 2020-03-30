namespace tmp
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var player = new Player(new Vector(0, 30, 0),
                new Vector(1, 0, 0), 10, 15);
            var world = new World(new FlatGenerator());
            var game = new Window(world, player);
            game.Run(200, 200);
        }
    }
}