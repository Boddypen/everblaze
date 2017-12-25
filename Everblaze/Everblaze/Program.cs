using System;

namespace Everblaze
{

	#if WINDOWS

    public static class Program
    {
		
        /// 
		/// <summary>
        ///		The main entry point for the application.
        /// </summary>
		/// 
        public static void Main(string[] args)
        {
			
			// Run the game.
            using (Game game = new Game("localhost"))
            {
                game.Run();
            }

        }

    }

	#endif

}
