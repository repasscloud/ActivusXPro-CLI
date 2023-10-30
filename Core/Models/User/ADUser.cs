namespace ActivusXPro_CLI.Core.Models.User
{
    public class ADUser
    {
        public string SamAccountName { get; set; } = null!;
        public string? UserPrincipalName { get; set; }
        public string? GivenName { get; set; }
        public string? Surname { get; set; }
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public string? EmployeeID { get; set; }
        public string? Department { get; set; }
        public string? Title { get; set; }
        public string? PhoneNumber { get; set; }
        public string? WwwHomePage { get; set; }
        public bool AccountEnabled { get; set; } = false;
        public bool PasswordNeverExpires { get; set; } = false;
        public List<string>? ProxyAddresses { get; set; }
        public string? Country { get; set; }
        public string OrgUnit { get; set; } = null!;
    }
}

