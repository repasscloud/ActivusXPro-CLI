using System;
using System.DirectoryServices.AccountManagement;
using static System.Net.Mime.MediaTypeNames;

namespace ActivusXPro_CLI.Core.Utilities.Helper
{
	public class IsExistsObject
	{
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public static bool IsSamAccountNameExists(string domainName, string samAccountName)
        {
            try
            {
                using (PrincipalContext context = new PrincipalContext(ContextType.Domain, domainName))
                {
                    // Search for a user with the specified samAccountName
                    UserPrincipal user = UserPrincipal.FindByIdentity(context, samAccountName);

                    // If user is not null, the samAccountName already exists
                    return user != null;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the search
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}

