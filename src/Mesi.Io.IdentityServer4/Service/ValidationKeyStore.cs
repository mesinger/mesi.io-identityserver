using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Mesi.Io.IdentityServer4.Data;
using Mesi.Io.IdentityServer4.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Mesi.Io.IdentityServer4.Service
{
    public class ValidationKeyStore : IValidationKeysStore
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ValidationKeyStore> _logger;
        private readonly IdentityServerSecretOptions _secretOptions;

        public ValidationKeyStore(ApplicationDbContext dbContext, ILogger<ValidationKeyStore> logger, IOptions<IdentityServerSecretOptions> secretOptions)
        {
            _dbContext = dbContext;
            _logger = logger;
            _secretOptions = secretOptions.Value;
        }
        
        public async Task<IEnumerable<SecurityKeyInfo>> GetValidationKeysAsync()
        {
            var secret = await _dbContext.Secrets.SingleOrDefaultAsync(s => s.Name == _secretOptions.Public);

            if (secret == null)
            {
                _logger.LogError("Unable to retrieve validation key");
                throw new Exception("Unable to retrieve validation key");
            }
            
            var key = Convert.FromBase64String(secret.Value);
            
            var rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(key, out _);
            
            var securityKeyInfo = new SecurityKeyInfo
            {
                Key = new RsaSecurityKey(rsa),
                SigningAlgorithm = "RS256"
            };
            
            return new[] {securityKeyInfo};
        }
    }
}