using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Mesi.Io.IdentityServer4.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Mesi.Io.IdentityServer4.Service
{
    public class ValidationKeyStore : IValidationKeysStore
    {
        private readonly CertificateOptions _certificateOptions;
        
        public ValidationKeyStore(IOptionsSnapshot<CertificateOptions> certificateOptions)
        {
            _certificateOptions = certificateOptions.Value;
        }
        
        public async Task<IEnumerable<SecurityKeyInfo>> GetValidationKeysAsync()
        {
            var rsa = RSA.Create();
            rsa.ImportRSAPublicKey(await File.ReadAllBytesAsync(_certificateOptions.Public), out var bytesRead);
            var keyInfo = new SecurityKeyInfo
            {
                Key = new RsaSecurityKey(rsa)
            };
            return new []{keyInfo};
        }
    }
}