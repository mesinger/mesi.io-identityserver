using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mesi.Io.IdentityServer4.Data.Entities
{
    public class ApplicationSecret
    {
        [Key]
        [Column("name")]
        public string Name { get; set; }
        public string Value { get; set; }
    }
}