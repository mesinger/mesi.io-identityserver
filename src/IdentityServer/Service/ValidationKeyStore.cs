using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Service
{
    public class ValidationKeyStore : IValidationKeysStore
    {
        public async Task<IEnumerable<SecurityKeyInfo>> GetValidationKeysAsync()
        {
            var rsa = RSA.Create();
            rsa.ImportRSAPublicKey(await File.ReadAllBytesAsync("key.pub"), out var bytesRead);
            var keyInfo = new SecurityKeyInfo
            {
                Key = new RsaSecurityKey(rsa)
            };
            return new []{keyInfo};
        }
    }
}