using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IdentityServer4.Stores;
using Mesi.Io.IdentityServer4.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Mesi.Io.IdentityServer4.Service
{
    public class SigningCredentialStore : ISigningCredentialStore
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<SigningCredentialStore> _logger;
        
        public SigningCredentialStore(ApplicationDbContext dbContext, ILogger<SigningCredentialStore> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task<SigningCredentials> GetSigningCredentialsAsync()
        {
            var secret = await _dbContext.Secrets.SingleOrDefaultAsync(s => s.Name == "signing_key");

            if (secret == null)
            {
                _logger.LogError("Unable to retrieve secret key");
                throw new Exception("Unable to retrieve secret key");
            }
            
            var signingKey = Convert.FromBase64String(secret.Value);

            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(signingKey, out _);
            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
            return signingCredentials;
        }
    }
}