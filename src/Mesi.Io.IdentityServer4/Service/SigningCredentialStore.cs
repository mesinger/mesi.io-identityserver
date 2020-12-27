using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IdentityServer4.Stores;
using Mesi.Io.IdentityServer4.Data;
using Mesi.Io.IdentityServer4.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Mesi.Io.IdentityServer4.Service
{
    public class SigningCredentialStore : ISigningCredentialStore
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<SigningCredentialStore> _logger;
        private readonly IdentityServerSecretOptions _secretOptions;

        public SigningCredentialStore(ApplicationDbContext dbContext, ILogger<SigningCredentialStore> logger, IOptions<IdentityServerSecretOptions> secretOptions)
        {
            _dbContext = dbContext;
            _logger = logger;
            _secretOptions = secretOptions.Value;
        }
        
        public async Task<SigningCredentials> GetSigningCredentialsAsync()
        {
            var secret = await _dbContext.Secrets.SingleOrDefaultAsync(s => s.Name == _secretOptions.Private);

            if (secret == null)
            {
                _logger.LogError("Unable to retrieve signing key");
                throw new Exception("Unable to retrieve signing key");
            }
            
            var signingKey = Convert.FromBase64String(secret.Value);
            
            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(signingKey, out _);
            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
            return signingCredentials;
        }
    }
}