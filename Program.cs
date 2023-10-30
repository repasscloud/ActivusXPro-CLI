using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using ActivusXPro_CLI.Core.Utilities;

namespace ActivusXPro_CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                HelpMenu.MainHelp();
                return;
            }

            // get the current domain context
            Domain currentDomain = Domain.GetCurrentDomain();

            // get the LDAP path for the current domain
            string domainPath = $"LDAP://{currentDomain.Name}";

            // DN of domain
            string domainDN = currentDomain.GetDirectoryEntry().Properties["distinguishedName"].Value!.ToString()!;

            // select the command
            string commandSelect = args[0].ToLower();

            switch (commandSelect)
            {
                case "--search":
                    // check if there are additional args
                    if (args.Length == 4)
                    {
                        string additionalArg = args[1].ToLower();
                        string searchADProperty = args[2].ToLower();
                        string searchValue = args[3].ToLower();

                        switch (additionalArg)
                        {
                            case "/?":
                                HelpMenu.SearchADUserHelp();
                                break;
                            case "--user":
                                SearchAD.FindADUser(rootDN: domainPath, searchBy: searchADProperty, searchValue: searchValue);
                                break;
                            default:
                                HelpMenu.SearchADUserHelp();
                                break;
                        }
                    }
                    else
                    {
                        HelpMenu.SearchADUserHelp();
                    }
                    break;
                case "--new":
                    break;
                case "--update":
                    break;
                default:
                    Console.WriteLine("Invalid command");
                    break;
            }
        }
    }
}