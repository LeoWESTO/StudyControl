using System.Security.Cryptography;
using System.Text;

namespace StudyControlWeb.Data
{
    public static class PasswordManager
    {
        public static string Hash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                var sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }
        public static bool Verify(string input, string hash)
        {
            return StringComparer.OrdinalIgnoreCase.Compare(Hash(input), hash) == 0;
        }
    }
}
