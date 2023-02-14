using System.Text;
using System.Security.Cryptography;

namespace SingSpace
{
    public class Sing
    {
        public (string, string) SingDoc(byte[] doc)
        {
            var resultData = "";
            var resultSing = "";
            if (doc == null) return (resultData, resultSing);
            resultData = Convert.ToBase64String(doc);
            resultSing = Sha256(resultData);
            return (resultData, resultSing);
        }

        private string Sha256(string input, bool isLowercase = false)
        {
            if (string.IsNullOrEmpty(input)) return "";

            using (SHA256 sha256 = SHA256.Create())
            {
                var byteHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var hash = BitConverter.ToString(byteHash).Replace("-", "");
                return (isLowercase) ? hash.ToLower() : hash;
            }
        }

    }
}