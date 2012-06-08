using System;

namespace MapGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (gameMain game = new gameMain())
            {
                game.Run();
            }
        }
    }
#endif
}

