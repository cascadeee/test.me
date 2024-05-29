using System.Security.Cryptography;
using System.Text;

namespace testme
{
    public class MD5Hash
    {
        public static string hashPassword(string password)
        {
            MD5 mD5 = MD5.Create();
            byte[] h = Encoding.ASCII.GetBytes(password);
            byte[] hash = mD5.ComputeHash(h);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
