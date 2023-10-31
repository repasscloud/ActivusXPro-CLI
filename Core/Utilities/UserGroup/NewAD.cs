﻿using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using ActivusXPro_CLI.Core.Models.User;

namespace ActivusXPro_CLI.Core.Utilities.UserGroup
{
	public class NewAD
	{
        public static void ADUser(List<string> cliArgs, string orgUnit, string orgDN)
		{
			bool containsObjectName = cliArgs.Any(arg => arg.ToLower().StartsWith("on:"));
			bool containsSamAccountName = cliArgs.Any(arg => arg.ToLower().StartsWith("sam:"));

			if (containsObjectName && containsSamAccountName)
			{
                // create new ADUser object
                ADUser adUser = new();

                // set the OrgUnit for user (unless specified)
                string newADUserOrgUnit = orgUnit;

                foreach (string item in cliArgs)
                {
                    string[] parts = item.Split(':');
                    if (parts.Length == 2)
                    {
                        string key = parts[0].ToLower();
                        string value = parts[1];

                        switch (key)
                        {
                            case "on":
                                adUser.ObjectName = value;
                                break;
                            case "sam":
                                adUser.SamAccountName = value;
                                break;
                            case "upn":
                                adUser.UserPrincipalName = value;
                                break;
                            case "gn":
                                adUser.GivenName = value;
                                break;
                            case "sn":
                                adUser.Surname = value;
                                break;
                            case "dn":
                                adUser.DisplayName = value;
                                break;
                            case "e":
                                adUser.Email = value;
                                adUser.ProxyAddresses = new List<string> { $"SMTP:{value}" };
                                break;
                            case "dep":
                                adUser.Department = value;
                                break;
                            case "t":
                                adUser.Title = value;
                                break;
                            case "ph":
                                adUser.PhoneNumber = value;
                                break;
                            case "www":
                                adUser.WwwHomePage = value;
                                break;
                            case "enabled":
                                if (value == "true")
                                {
                                    adUser.AccountEnabled = true;
                                }
                                else
                                {
                                    adUser.AccountEnabled = false;
                                }
                                break;
                            case "pne":
                                if (value == "true")
                                {
                                    adUser.PasswordNeverExpires = true;
                                }
                                else
                                {
                                    adUser.PasswordNeverExpires = false;
                                }
                                break;
                            case "ou":
                                newADUserOrgUnit = value;
                                break;
                        }
                    }
                    else if (parts.Length == 1)
                    {
                        //TODO handle when switches are provided here
                    }
                }

                string containerPath = $"LDAP://{orgDN}";

                // create DirectoryEntry for the container
                DirectoryEntry container = new DirectoryEntry(containerPath);

                // create DirectoryEntry for the new user
                DirectoryEntry newUser = container.Children.Add($"CN={adUser.ObjectName}", "user");

                // set the user attributes
                newUser.Properties["sAMAccountName"].Value = adUser.SamAccountName;
                newUser.Properties["userPrincipalName"].Value = adUser.UserPrincipalName ?? string.Empty;
                newUser.Properties["givenName"].Value = adUser.GivenName ?? string.Empty;
                newUser.Properties["sn"].Value = adUser.Surname ?? string.Empty;
                newUser.Properties["displayName"].Value = adUser.DisplayName ?? string.Empty;
                newUser.Properties["mail"].Value = adUser.Email ?? string.Empty;
                newUser.Properties["employeeNumber"].Value = adUser.EmployeeID ?? string.Empty;
                newUser.Properties["department"].Value = adUser.Department ?? string.Empty;
                newUser.Properties["title"].Value = adUser.Title ?? string.Empty;
                newUser.Properties["telephoneNumber"].Value = adUser.PhoneNumber ?? string.Empty;
                newUser.Properties["wWWHomePage"].Value = adUser.WwwHomePage ?? string.Empty;

                // save the user
                newUser.CommitChanges();

                // enable user?
                if (adUser.AccountEnabled)
                {
                    // retrieve the user account
                    using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
                    {
                        // search for the user by sAMAccountName
                        UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, adUser.SamAccountName.ToString());

                        // enable AD User
                        if (user != null)
                        {
                            user.Enabled = true;

                            // password never expires?
                            if (adUser.PasswordNeverExpires)
                            {
                                user.PasswordNeverExpires = true;
                            }

                            // save the user object
                            user.Save();
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid parameters passed");
            }
        }
	}
}

