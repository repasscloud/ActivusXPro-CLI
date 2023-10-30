using System.DirectoryServices;
using ActivusXPro_CLI.Core.Models.User;

namespace ActivusXPro_CLI.Core.Utilities
{
	public class SearchAD
	{
		public static void FindADUser(string rootDN, string searchBy, string searchValue)
		{
            // set the adProperty we will search by
            string adProperty = string.Empty;

            switch (searchBy)
            {
                case "s":
                    adProperty = "sAMAccountName";
                    break;
                case "upn":
                    adProperty = "userPrincipalName";
                    break;
                case "gn":
                    adProperty = "givenName";
                    break;
                case "sn":
                    adProperty = "sn";
                    break;
                case "mail":
                    adProperty = "mail";
                    break;
                case "empid":
                    adProperty = "employeeID";
                    break;
                case "cn":
                    adProperty = "cn";
                    break;
                case "dn":
                    adProperty = "distinguishedName";
                    break;
                default:
                    HelpMenu.SearchADUserHelp();
                    return;
            }

			using (DirectorySearcher searcher = new DirectorySearcher(new DirectoryEntry(rootDN)))
			{
                Console.WriteLine($"Searching by: (&(objectClass=user)({adProperty}={searchValue}))");

                searcher.Filter = $"(&(objectClass=user)({adProperty}={searchValue}))";

                SearchResult? result = searcher.FindOne();

                if (result != null)
                {
                    Console.WriteLine("User found:");
                    ADUser user = new ADUser
                    {
                        SamAccountName = result.Properties["sAMAccountName"][0].ToString()!,
                        UserPrincipalName = result.Properties.Contains("userPrincipalName") ? result.Properties["userPrincipalName"][0].ToString() : null,
                        GivenName = result.Properties.Contains("givenName") ? result.Properties["givenName"][0].ToString() : null,
                        Surname = result.Properties.Contains("sn") ? result.Properties["sn"][0].ToString() : null,
                        DisplayName = result.Properties.Contains("displayName") ? result.Properties["displayName"][0].ToString() : null,
                        Email = result.Properties.Contains("mail") ? result.Properties["mail"][0].ToString() : null,
                        EmployeeID = result.Properties.Contains("employeeID") ? result.Properties["employeeID"][0].ToString() : null,
                        Department = result.Properties.Contains("department") ? result.Properties["department"][0].ToString() : null,
                        Title = result.Properties.Contains("title") ? result.Properties["title"][0].ToString() : null,
                        PhoneNumber = result.Properties.Contains("telephoneNumber") ? result.Properties["telephoneNumber"][0].ToString() : null,
                        AccountEnabled = int.Parse(result.Properties["userAccountControl"][0].ToString()!) != 2, // Check if the account is enabled
                        PasswordNeverExpires = int.Parse(result.Properties["userAccountControl"][0].ToString()!) == 0x200, // Check if the password never expires
                        Country = result.Properties.Contains("co") ? result.Properties["co"][0].ToString() : null,
                    };

                    Console.WriteLine($"SamAccountName: {user.SamAccountName}");
                    Console.WriteLine($"UserPrincipalName: {user.UserPrincipalName ?? "N/A"}");
                    Console.WriteLine($"GivenName: {user.GivenName ?? "N/A"}");
                    Console.WriteLine($"Surname: {user.Surname ?? "N/A"}");
                    Console.WriteLine($"DisplayName: {user.DisplayName ?? "N/A"}");
                    Console.WriteLine($"Email: {user.Email ?? "N/A"}");
                    Console.WriteLine($"EmployeeID: {user.EmployeeID ?? "N/A"}");
                    Console.WriteLine($"Department: {user.Department ?? "N/A"}");
                    Console.WriteLine($"Title: {user.Title ?? "N/A"}");
                    Console.WriteLine($"PhoneNumber: {user.PhoneNumber ?? "N/A"}");
                    Console.WriteLine($"AccountEnabled: {user.AccountEnabled}");
                    Console.WriteLine($"PasswordNeverExpires: {user.PasswordNeverExpires}");
                    Console.WriteLine($"Country: {user.Country}");

                    if (user.ProxyAddresses != null && user.ProxyAddresses.Count > 0)
                    {
                        Console.WriteLine("ProxyAddresses:");
                        foreach (string proxyAddress in user.ProxyAddresses)
                        {
                            Console.WriteLine($"  - {proxyAddress}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ProxyAddresses: N/A");
                    }
                }
                else
                {
                    Console.WriteLine("User not found.");
                }
            }
		}
	}
}

