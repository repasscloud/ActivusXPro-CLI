namespace ActivusXPro_CLI.Core.Utilities
{
	public class HelpMenu
	{
		public static void MainHelp()
		{
			Console.WriteLine("ActivusX Pro Tool Help");
            Console.WriteLine("Usage: actxpro.exe <command> <targetType> [param] <value>");
            Console.WriteLine();
            Console.WriteLine("Available commands:");
            Console.WriteLine(" --search\tSearch for User or Computer object");
            Console.WriteLine(" --new\tCreate a new User or Computer object");
            Console.WriteLine(" --update\tUpdate an existing User or Computer.");
            Console.WriteLine();
            Console.WriteLine("Available target types:");
            Console.WriteLine(" --user\tTarget a User object");
            Console.WriteLine(" --computer\tTarget a Computer object");
            Console.WriteLine();
            Console.WriteLine("Additional help menu available using \"/?\"");
            Console.WriteLine("  eg: actxpro.exe --search --user /?");
        }
	}
}

