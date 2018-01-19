using System;

namespace Everblaze
{

	#if WINDOWS

    public static class Program
    {

		/// <summary>
		///		A Random Number Generator.
		/// </summary>
		public static Random random;

        /// 
		/// <summary>
        ///		The main entry point for the application.
        /// </summary>
		/// 
        public static void Main(string[] args)
        {

			// Initialise the random number generator.
			Program.random = new Random();


			// Run the game.
            using (Game game = new Game("localhost"))
            {
                game.Run();
            }

        }

    }

	#endif

}
