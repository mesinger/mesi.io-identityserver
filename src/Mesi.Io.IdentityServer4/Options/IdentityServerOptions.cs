namespace Mesi.Io.IdentityServer4.Options
{
    public record IdentityServerSecretOptions(string Private, string Public)
    {
        public IdentityServerSecretOptions() : this("", "")
        {
            
        }
    }
}