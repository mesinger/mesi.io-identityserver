using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Mesi.Io.IdentityServer4.Config;
using Mesi.Io.IdentityServer4.Data;
using Microsoft.EntityFrameworkCore;

namespace Mesi.Io.IdentityServer4.Service
{
    /// <inheritdoc />
    public class ClientStore : IClientStore
    {
        private readonly ApplicationDbContext _dbContext;

        public ClientStore(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        /// <inheritdoc />
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = await _dbContext.IdentityServerClients.SingleAsync(c => c.ClientId == clientId);
            return client.ToIdentityServerClient();
        }
    }
}