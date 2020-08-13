using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IdentityServer4.Stores;
using Mesi.Io.IdentityServer4.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Mesi.Io.IdentityServer4.Service
{
    public class SigningCredentialStore : ISigningCredentialStore
    {
        private readonly CertificateOptions _certificateOptions;
        
        public SigningCredentialStore(IOptionsSnapshot<CertificateOptions> certificateOptions)
        {
            _certificateOptions = certificateOptions.Value;
        }
        
        public async Task<SigningCredentials> GetSigningCredentialsAsync()
        {
            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(await File.ReadAllBytesAsync(_certificateOptions.Private), out var bytesRead);
            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
            return signingCredentials;
        }
    }
}