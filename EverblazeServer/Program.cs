using System;
using System.Windows.Forms;

namespace EverblazeServer
{

	public static class Program
	{

		/// <summary>
		///		A random number generator.
		/// </summary>
		public static Random random;


		/// 
		/// <summary>
		///		The main entry point for the application.
		/// </summary>
		/// 
		[STAThread]
		public static void Main(String[] args)
		{
			// Set up the random number generator.
			Everblaze.Program.random = new Random();


			Boolean autoStart = false;

			if(args.Length >= 1)
				if (args[0].Equals("--auto-start"))
					autoStart = true;

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new ServerForm(autoStart));

		}

	}

}
