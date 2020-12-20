using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Mesi.Io.IdentityServer4.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Mesi.Io.IdentityServer4.Service
{
    public class ValidationKeyStore : IValidationKeysStore
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ValidationKeyStore> _logger;

        public ValidationKeyStore(ApplicationDbContext dbContext, ILogger<ValidationKeyStore> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task<IEnumerable<SecurityKeyInfo>> GetValidationKeysAsync()
        {
            var secret = await _dbContext.Secrets.SingleOrDefaultAsync(s => s.Name == "public_1");

            if (secret == null)
            {
                _logger.LogError("Unable to retrieve secret key");
                throw new Exception("Unable to retrieve secret key");
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