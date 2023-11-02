namespace ActivusXPro_CLI.Core.Models.User
{
    public class ADUser
    {
        public string SamAccountName { get; set; } = null!;
        public string ObjectName { get; set; } = null!;
        public string? UserPrincipalName { get; set; }
        public string? GivenName { get; set; }
        public string? Surname { get; set; }
        public string? Department { get; set; }
        public string? Descripition { get; set; }
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public string? EmployeeID { get; set; }
        public string? Title { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhysicalDeliveryOfficeName { get; set; }
        public string? WwwHomePage { get; set; }
        public string? Description { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostCode { get; set; }
        public List<string>? POBox { get; set; }
        public bool AccountEnabled { get; set; } = false;
        public bool PasswordNeverExpires { get; set; } = false;
        public List<string>? ProxyAddresses { get; set; }
        public string? Country { get; set; }
        public string OrgUnit { get; set; } = null!;
    }
}

