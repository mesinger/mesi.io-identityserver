using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace KeyGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var rsa = RSA.Create(4096);
            await File.WriteAllBytesAsync("key", rsa.ExportRSAPrivateKey());
            await File.WriteAllBytesAsync("key.pub", rsa.ExportRSAPublicKey());
        }
    }
}