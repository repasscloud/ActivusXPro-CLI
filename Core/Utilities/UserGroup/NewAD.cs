﻿using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using ActivusXPro_CLI.Core.Models.User;
using ActivusXPro_CLI.Core.Utilities.Helper;
using ActivusXPro_CLI.Core.Utilities.Security;

namespace ActivusXPro_CLI.Core.Utilities.UserGroup
{
    public class NewAD
    {
        public static void ADUser(List<string> cliArgs, string userCN)
        {
            bool containsObjectName = cliArgs.Any(arg => arg.ToLower().StartsWith("on:"));
            bool containsSamAccountName = cliArgs.Any(arg => arg.ToLower().StartsWith("sam:"));
            bool setRandomPassword = false;

            if (containsObjectName && containsSamAccountName)
            {
                // create new ADUser object
                ADUser adUser = new();
                adUser.AccountEnabled = false;
                adUser.PasswordNeverExpires = false;

                foreach (string item in cliArgs)
                {
                    string[] parts = item.Split(':');
                    if (parts.Length == 2)
                    {
                        string key = parts[0].ToLower();
                        string value = parts[1];

                        switch (key)
                        {
                            case "c":
                                adUser.Country = value;
                                break;
                            case "co":
                                adUser.Company = value;
                                break;
                            case "cy":
                                adUser.City = value;
                                break;
                            case "dep":
                                adUser.Department = value;
                                break;
                            case "desc":
                                adUser.Descripition = value;
                                break;
                            case "dn":
                                adUser.DisplayName = value;
                                break;
                            case "e":
                                adUser.Email = value;
                                adUser.ProxyAddresses = new List<string> { $"SMTP:{value}" };
                                break;
                            case "gn":
                                adUser.GivenName = value;
                                break;
                            case "on":
                                adUser.ObjectName = value;
                                break;
                            case "pc":
                            case "zip":
                                adUser.PostCode = value;
                                break;
                            case "ph":
                                adUser.PhoneNumber = value;
                                break;
                            case "pobox":
                                adUser.POBox = new List<string> { $"{value}" };
                                break;
                            case "pdon":
                                adUser.PhysicalDeliveryOfficeName = value;
                                break;
                            case "sam":
                                adUser.SamAccountName = value;
                                break;
                            case "sn":
                                adUser.Surname = value;
                                break;
                            case "st":
                                adUser.StreetAddress = value;
                                break;
                            case "stt":
                                adUser.State = value;
                                break;
                            case "t":
                                adUser.Title = value;
                                break;
                            case "upn":
                                adUser.UserPrincipalName = value;
                                break;
                            case "www":
                                adUser.WwwHomePage = value;
                                break;
                            case "enabled":
                                if (value == "true")
                                {
                                    adUser.AccountEnabled = true;
                                }
                                break;
                            case "pne":
                                if (value == "true")
                                {
                                    adUser.PasswordNeverExpires = true;
                                }
                                break;
                            case "password":
                                if (value == "true")
                                {
                                    setRandomPassword = true;
                                }
                                break;
                            case "ou":
                                userCN = value;
                                break;
                            case "pp":
                                adUser.ProfilePath = value;
                                break;
                        }
                    }
                    else if (parts.Length == 1)
                    {
                        //TODO handle when switches are provided here
                    }
                }

                // check if the user exists in AD with the current samaccountname
                bool exists = IsExistsObject.IsSamAccountNameExists(domainName: "judgejudy.local", samAccountName: adUser.SamAccountName);

                if (!exists)
                {
                    // create DirectoryEntry for the container
                    DirectoryEntry container = new DirectoryEntry(userCN);

                    // create DirectoryEntry for the new user
                    DirectoryEntry newUser = container.Children.Add($"CN={adUser.ObjectName}", "user");

                    // set the sAMAccountName attribute (required)
                    newUser.Properties["sAMAccountName"].Value = adUser.SamAccountName;

                    // set the user attributes
                    if (!string.IsNullOrEmpty(adUser.GivenName))
                        newUser.Properties["givenName"].Value = adUser.GivenName;
                    if (!string.IsNullOrEmpty(adUser.Surname))
                        newUser.Properties["sn"].Value = adUser.Surname;
                    if (!string.IsNullOrEmpty(adUser.Company))
                        newUser.Properties["company"].Value = adUser.Company;
                    if (!string.IsNullOrEmpty(adUser.Department))
                        newUser.Properties["department"].Value = adUser.Department;
                    if (!string.IsNullOrEmpty(adUser.Descripition))
                        newUser.Properties["description"].Value = adUser.Descripition;
                    if (!string.IsNullOrEmpty(adUser.DisplayName))
                        newUser.Properties["displayName"].Value = adUser.DisplayName;
                    if (!string.IsNullOrEmpty(adUser.EmployeeID))
                        newUser.Properties["employeeNumber"].Value = adUser.EmployeeID;
                    if (!string.IsNullOrEmpty(adUser.Email))
                        newUser.Properties["mail"].Value = adUser.Email;
                    if (!string.IsNullOrEmpty(adUser.PhysicalDeliveryOfficeName))
                        newUser.Properties["physicalDeliveryOfficeName"].Value = adUser.PhysicalDeliveryOfficeName;
                    if (!string.IsNullOrEmpty(adUser.Title))
                        newUser.Properties["title"].Value = adUser.Title;
                    if (!string.IsNullOrEmpty(adUser.PhoneNumber))
                        newUser.Properties["telephoneNumber"].Value = adUser.PhoneNumber;
                    if (!string.IsNullOrEmpty(adUser.WwwHomePage))
                        newUser.Properties["wWWHomePage"].Value = adUser.WwwHomePage;
                    if (!string.IsNullOrEmpty(adUser.StreetAddress))
                        newUser.Properties["streetAddress"].Value = adUser.StreetAddress.Replace("\\n", Environment.NewLine);
                    if (!string.IsNullOrEmpty(adUser.City))
                        newUser.Properties["l"].Value = adUser.City;
                    if (!string.IsNullOrEmpty(adUser.State))
                        newUser.Properties["st"].Value = adUser.State;
                    if (!string.IsNullOrEmpty(adUser.PostCode))
                        newUser.Properties["postalCode"].Value = adUser.PostCode;
                    if (!string.IsNullOrEmpty(adUser.Country))
                        newUser.Properties["c"].Value = adUser.Country;

                    // PO Box
                    if (adUser.POBox != null && adUser.POBox.Count > 0)
                    {
                        newUser.Properties["postOfficeBox"].Value = adUser.POBox[0];
                    }

                    // if userPrincipalName is not null, set it now
                    if (!string.IsNullOrEmpty(adUser.UserPrincipalName))
                        newUser.Properties["userPrincipalName"].Value = adUser.UserPrincipalName;

                    // set proxyAddresses
                    if (adUser.ProxyAddresses != null && adUser.ProxyAddresses.Count > 0)
                    {
                        newUser.Properties["proxyAddresses"].Value = adUser.ProxyAddresses[0];
                    }

                    // save the user
                    newUser.CommitChanges();

                    // retrieve the user account for PrincipalContext changes
                    using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
                    {
                        // search for the user by sAMAccountName
                        UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, adUser.SamAccountName.ToString());

                        // enable AD User
                        if (user != null)
                        {
                            // enable user?
                            if (adUser.AccountEnabled)
                            {
                                user.Enabled = true;

                                // password never expires? (only if account is enabled)
                                if (adUser.PasswordNeverExpires)
                                {
                                    user.PasswordNeverExpires = true;
                                }
                            }

                            // set random password?
                            if (setRandomPassword)
                            {
                                user.SetPassword(newPassword: PasswordGenerator.GenerateRandomPassword(length: 20));
                            }

                            // Remote Desktop profilePath
                            //if (!string.IsNullOrEmpty(adUser.ProfilePath))
                            //{
                            //    DirectoryEntry userEntry = (DirectoryEntry)user.GetUnderlyingObject();
                            //    userEntry.Properties["TerminalServicesProfilePath"].Value = adUser.ProfilePath;
                            //    userEntry.CommitChanges();
                            //}

                            // save the user object
                            user.Save();
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"User with sAMAccountName {adUser.SamAccountName} already exists");
                }
            }
            else
            {
                Console.WriteLine("Invalid parameters passed");
            }
        }
    }
}

