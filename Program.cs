﻿using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using ActivusXPro_CLI.Core.Utilities;
using ActivusXPro_CLI.Core.Utilities.UserGroup;
using OfficeOpenXml;

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

            // EPPlus License
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // get the current domain context
            Domain currentDomain = Domain.GetCurrentDomain();

            // get the LDAP path for the current domain
            string domainPath = $"LDAP://{currentDomain.Name}";

            // DN of domain
            string domainDN = currentDomain.GetDirectoryEntry().Properties["distinguishedName"].Value!.ToString()!;

            // select the runtimeCommand
            string runtimeCommand = args[0].ToLower();

            switch (runtimeCommand)
            {
                #region User Runtime Commands
                case "user":
                    if (args.Length >= 2)
                    {
                        switch (args[1].ToLower())
                        {
                            case "new":
                                // grab the additional arguments into a List<string>
                                List<string> additionalUserNewArgs = new List<string>();
                                for (int i = 2; i < args.Length; i++)
                                {
                                    additionalUserNewArgs.Add(args[i]);
                                }
                                // pass them to the new user function
                                NewAD.ADUser(cliArgs: additionalUserNewArgs, orgUnit: $"LDAP://CN=Users,{domainDN}");
                                break;
                            case "update":
                                break;
                            case "delete":
                                break;
                            case "-h":
                            case "--help":
                                break;
                            default:
                                //TODO Invalid "user" runtime args[1] message
                                break;
                        }
                    }
                    else
                    {
                        //TODO show the user runtime help menu
                    }
                    break;
                #endregion

                //case "--search":
                //    // check if there are additional args
                //    if (args.Length == 4)
                //    {
                //        string additionalArg = args[1].ToLower();
                //        string searchADProperty = args[2].ToLower();
                //        string searchValue = args[3].ToLower();

                //        switch (additionalArg)
                //        {
                //            case "/?":
                //                HelpMenu.SearchADUserHelp();
                //                break;
                //            case "--user":
                //                SearchAD.FindADUser(rootDN: domainPath, searchBy: searchADProperty, searchValue: searchValue);
                //                break;
                //            default:
                //                HelpMenu.SearchADUserHelp();
                //                break;
                //        }
                //    }
                //    else
                //    {
                //        HelpMenu.SearchADUserHelp();
                //    }
                //    break;
                //case "--new":
                //    break;
                //case "--update":
                //    break;
                //case "--report":
                //    // actxpro.exe --report --class:<user|computer> --<disabled|enabled> [--file:<xlsx|csv>]
                //    if (args.Length == 4)
                //    {
                //        try
                //        {
                //            // assign all the raw args parsed
                //            string[] objectClassParts = args[1].ToLower().Split(':');
                //            string accountControl = args[2].ToLower();
                //            string[] exportFileTypeParts = args[3].Split(new char[] { ':' }, 2);

                //            // set the split results to empty strings
                //            string objectClass = string.Empty;
                //            string exportFilePath = string.Empty;

                //            // split the parts to the split results
                //            if (objectClassParts.Length == 2)
                //            {
                //                objectClass = objectClassParts[1];
                //            }
                //            else
                //            {
                //                //TODO: handle error here
                //            }
                //            if (exportFileTypeParts.Length == 2)
                //            {
                //                //string argumentName = exportFileTypeParts[0];
                //                exportFilePath = exportFileTypeParts[1];

                //                // Now, argumentName contains "--file" and argumentValue contains "C:\temp\export.xlsx"
                //                // You can use these values as needed.
                //            }
                //            else
                //            {
                //                // Handle the case where the split didn't produce two parts.
                //                // TODO: Handle error here
                //            }

                //            // process report
                //            switch (objectClass)
                //            {
                //                case "user":
                //                    switch (accountControl)
                //                    {
                //                        case "--disabled":
                //                            SearchAD.FindAllDisabledADUsers(rootDN: domainPath, filePath: exportFilePath);
                //                            break;
                //                        case "--enabled":
                //                            break;
                //                        default:
                //                            //TODO: incorrect args[2]
                //                            break;
                //                    }
                //                    break;
                //                case "computer":
                //                    break;
                //                default:
                //                    //TODO: Report incorrect object class filter
                //                    break;
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            Console.WriteLine($"Error: {ex}");
                //        }
                //    }
                //    break;
                //default:
                //    Console.WriteLine("Invalid command");
                //    break;
            }
        }
    }
}