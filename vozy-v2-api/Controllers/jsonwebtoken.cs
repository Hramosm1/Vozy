using JWT.Algorithms;
using JWT.Builder;
namespace vozy_v2_api.Controllers
{
    public class jsonwebtoken
    {
        private readonly string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
        public string crearToken(string usuario, string password)
        {
            var token = JwtBuilder.Create()
                      .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                      .WithSecret(secret)
                      .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(24).ToUnixTimeSeconds())
                      .AddClaim("user", usuario)
                      .AddClaim("password", password)
                      .Encode();

            return token;
        }

        public bool validarToken(string token)
        {
            try
            {
                JwtBuilder.Create()
                                .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                                .WithSecret(secret)
                                .MustVerifySignature()
                                .Decode(token);
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
