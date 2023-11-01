using System.Text;

namespace ActivusXPro_CLI.Core.Utilities.Security
{
	public class PasswordGenerator
	{
        private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_-+=<>?";

        public static string GenerateRandomPassword(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentException("Password length must be greater than zero.");
            }

            StringBuilder password = new StringBuilder(length);
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(0, Characters.Length);
                password.Append(Characters[index]);
            }

            return password.ToString();
        }
    }
}

