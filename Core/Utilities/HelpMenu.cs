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
            Console.WriteLine(" --new\t\tCreate a new User or Computer object");
            Console.WriteLine(" --update\tUpdate an existing User or Computer.");
            Console.WriteLine();
            Console.WriteLine("Available target types:");
            Console.WriteLine(" --user\t\tTarget a User object");
            Console.WriteLine(" --computer\tTarget a Computer object");
            Console.WriteLine();
            Console.WriteLine("Additional help menu available using \"/?\"");
            Console.WriteLine("  eg: actxpro.exe --search --user /?");
        }

        public static void SearchADUserHelp()
        {
            Console.WriteLine("ActivusX Pro Tool Help");
            Console.WriteLine("Usage: actxpro.exe --search --user <ADProperty> <value>");
            Console.WriteLine();
            Console.WriteLine("ADProperty options:");
            Console.WriteLine(" s\tSamAccountName");
            Console.WriteLine(" upn\tUserPrincipalName");
            Console.WriteLine(" gn\tGivenName");
            Console.WriteLine(" sn\tSurname");
            Console.WriteLine(" mail\tEmail Address");
            Console.WriteLine(" empid\tEmployee ID");
            Console.WriteLine(" cn\tCountry");
            Console.WriteLine(" dn\tDistinguishedName");
            Console.WriteLine();
            Console.WriteLine("Example 1:");
            Console.WriteLine(" actxpro.exe --search --user upn simple.simon@ourdomain.com");
            Console.WriteLine("Example 2:");
            Console.WriteLine(" actxpro.exe --search --user s simple.s*");
        }
	}
}

