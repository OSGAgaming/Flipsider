using System;

namespace FlipEngine
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            using (FlipGame? game = new FlipGame())
                game.Run();
        }
    }
}
