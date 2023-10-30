using System.DirectoryServices;
using ActivusXPro_CLI.Core.Models.User;
using OfficeOpenXml;

namespace ActivusXPro_CLI.Core.Utilities
{
	public class SearchAD
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootDN">Automatically provided by the invocation code</param>
        /// <param name="searchBy">The ActiveDirectory property to search by</param>
        /// <param name="searchValue">The value to search for</param>
		public static void FindADUser(string rootDN, string searchBy, string searchValue)
		{
            switch (searchBy)
            {
                case "s":
                    FindADUserByValue(rootDN: rootDN, adProperty: "sAMAccountName", searchValue: searchValue);
                    break;
                case "upn":
                    FindADUserByValue(rootDN: rootDN, adProperty: "userPrincipalName", searchValue: searchValue);
                    break;
                case "gn":
                    FindADUserByValue(rootDN: rootDN, adProperty: "givenName", searchValue: searchValue);
                    break;
                case "sn":
                    FindADUserByValue(rootDN: rootDN, adProperty: "sn", searchValue: searchValue);
                    break;
                case "mail":
                    FindADUserByValue(rootDN: rootDN, adProperty: "mail", searchValue: searchValue);
                    break;
                case "empid":
                    FindADUserByValue(rootDN: rootDN, adProperty: "employeeID", searchValue: searchValue);
                    break;
                case "cn":
                    FindADUserByValue(rootDN: rootDN, adProperty: "cn", searchValue: searchValue);
                    break;
                case "dn":
                    FindADUserByValue(rootDN: rootDN, adProperty: "distinguishedName", searchValue: searchValue);
                    break;
                case "--enabled":
                    FindAllEnabledADUsers(rootDN: rootDN);
                    break;
                default:
                    HelpMenu.SearchADUserHelp();
                    return;
            }

			
		}

        private static void FindADUserByValue(string rootDN, string adProperty, string searchValue)
        {
            using (DirectorySearcher searcher = new DirectorySearcher(new DirectoryEntry(rootDN)))
            {
                //Console.WriteLine($"Searching by: (&(objectClass=user)({adProperty}={searchValue}))");

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

                    Console.WriteLine($"  SamAccountName: {user.SamAccountName}");
                    Console.WriteLine($"  UserPrincipalName: {user.UserPrincipalName ?? "N/A"}");
                    Console.WriteLine($"  GivenName: {user.GivenName ?? "N/A"}");
                    Console.WriteLine($"  Surname: {user.Surname ?? "N/A"}");
                    Console.WriteLine($"  DisplayName: {user.DisplayName ?? "N/A"}");
                    Console.WriteLine($"  Email: {user.Email ?? "N/A"}");
                    Console.WriteLine($"  EmployeeID: {user.EmployeeID ?? "N/A"}");
                    Console.WriteLine($"  Department: {user.Department ?? "N/A"}");
                    Console.WriteLine($"  Title: {user.Title ?? "N/A"}");
                    Console.WriteLine($"  PhoneNumber: {user.PhoneNumber ?? "N/A"}");
                    Console.WriteLine($"  AccountEnabled: {user.AccountEnabled}");
                    Console.WriteLine($"  PasswordNeverExpires: {user.PasswordNeverExpires}");
                    Console.WriteLine($"  Country: {user.Country}");

                    if (user.ProxyAddresses != null && user.ProxyAddresses.Count > 0)
                    {
                        Console.WriteLine("  ProxyAddresses:");
                        foreach (string proxyAddress in user.ProxyAddresses)
                        {
                            Console.WriteLine($"    - {proxyAddress}");
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

        public static void FindAllDisabledADUsers(string rootDN, string filePath)
        {
            using (DirectorySearcher searcher = new DirectorySearcher(new DirectoryEntry(rootDN)))
            {
                searcher.Filter = $"(&(objectClass=user)(userAccountControl:1.2.840.113556.1.4.803:=2))";

                SearchResultCollection results = searcher.FindAll();

                // result of disabled users great than 1 and not null
                if (results != null && results.Count > 1)
                {
                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("DisabledUsers");

                        // add headers to the worksheet
                        worksheet.Cells["A1"].Value = "sAMAccountName";
                        worksheet.Cells["B1"].Value = "userPrincipalName";
                        worksheet.Cells["C1"].Value = "givenName";
                        worksheet.Cells["D1"].Value = "sn";
                        worksheet.Cells["E1"].Value = "displayName";
                        worksheet.Cells["F1"].Value = "mail";
                        worksheet.Cells["G1"].Value = "employeeID";
                        worksheet.Cells["H1"].Value = "department";
                        worksheet.Cells["I1"].Value = "title";
                        worksheet.Cells["J1"].Value = "telephoneNumber";
                        worksheet.Cells["K1"].Value = "Country";

                        // add data from the result collection
                        for (int i = 0; i < results.Count; i++)
                        {
                            var result = results[i];
                            int row = i + 2; // start from row 2 to avoid overwriting headers

                            worksheet.Cells["A" + row].Value = result.Properties["sAMAccountName"][0].ToString()!;
                            worksheet.Cells["B" + row].Value = result.Properties.Contains("userPrincipalName") ? result.Properties["userPrincipalName"][0].ToString() : null;
                            worksheet.Cells["C" + row].Value = result.Properties.Contains("givenName") ? result.Properties["givenName"][0].ToString() : null;
                            worksheet.Cells["D" + row].Value = result.Properties.Contains("sn") ? result.Properties["sn"][0].ToString() : null;
                            worksheet.Cells["E" + row].Value = result.Properties.Contains("displayName") ? result.Properties["displayName"][0].ToString() : null;
                            worksheet.Cells["F" + row].Value = result.Properties.Contains("mail") ? result.Properties["mail"][0].ToString() : null;
                            worksheet.Cells["G" + row].Value = result.Properties.Contains("employeeID") ? result.Properties["employeeID"][0].ToString() : null;
                            worksheet.Cells["H" + row].Value = result.Properties.Contains("department") ? result.Properties["department"][0].ToString() : null;
                            worksheet.Cells["I" + row].Value = result.Properties.Contains("title") ? result.Properties["title"][0].ToString() : null;
                            worksheet.Cells["J" + row].Value = result.Properties.Contains("telephoneNumber") ? result.Properties["telephoneNumber"][0].ToString() : null;
                            worksheet.Cells["K" + row].Value = result.Properties.Contains("co") ? result.Properties["co"][0].ToString() : null;
                        }

                        // save the package to the specified file path
                        var fileInfo = new FileInfo(filePath);
                        package.SaveAs(fileInfo);
                    }
                }
            }
        }

        private static void FindAllEnabledADUsers(string rootDN)
        {
            using (DirectorySearcher searcher = new DirectorySearcher(new DirectoryEntry(rootDN)))
            {
                searcher.Filter = $"(&(objectClass=user)(!(userAccountControl:1.2.840.113556.1.4.803:=2)))";

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

                    Console.WriteLine($"  SamAccountName: {user.SamAccountName}");
                    Console.WriteLine($"  UserPrincipalName: {user.UserPrincipalName ?? "N/A"}");
                    Console.WriteLine($"  GivenName: {user.GivenName ?? "N/A"}");
                    Console.WriteLine($"  Surname: {user.Surname ?? "N/A"}");
                    Console.WriteLine($"  DisplayName: {user.DisplayName ?? "N/A"}");
                    Console.WriteLine($"  Email: {user.Email ?? "N/A"}");
                    Console.WriteLine($"  EmployeeID: {user.EmployeeID ?? "N/A"}");
                    Console.WriteLine($"  Department: {user.Department ?? "N/A"}");
                    Console.WriteLine($"  Title: {user.Title ?? "N/A"}");
                    Console.WriteLine($"  PhoneNumber: {user.PhoneNumber ?? "N/A"}");
                    Console.WriteLine($"  AccountEnabled: {user.AccountEnabled}");
                    Console.WriteLine($"  PasswordNeverExpires: {user.PasswordNeverExpires}");
                    Console.WriteLine($"  Country: {user.Country}");

                    if (user.ProxyAddresses != null && user.ProxyAddresses.Count > 0)
                    {
                        Console.WriteLine("  ProxyAddresses:");
                        foreach (string proxyAddress in user.ProxyAddresses)
                        {
                            Console.WriteLine($"    - {proxyAddress}");
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

