using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IdentityServer4.Stores;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Service
{
    public class SigningCredentialStore : ISigningCredentialStore
    {
        public async Task<SigningCredentials> GetSigningCredentialsAsync()
        {
            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(await File.ReadAllBytesAsync("key"), out var bytesRead);
            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
            return signingCredentials;
        }
    }
}