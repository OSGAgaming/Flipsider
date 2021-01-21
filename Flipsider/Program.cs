using System;

namespace Flipsider
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            using (Main? game = new Main())
                game.Run();
        }
    }
}
