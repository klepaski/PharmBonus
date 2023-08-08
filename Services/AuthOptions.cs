using Microsoft.IdentityModel.Tokens;
using System.Text;
 
namespace Med.Services
{
    public class AuthOptions
    {
        public const string ISSUER = "ChistyakovaJulia";
        public const string AUDIENCE = "AllClients";
        const string KEY = "everyone_likes_small_mimes123!@";
        public const int LIFETIME = 30;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}