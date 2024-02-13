using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyUrlShortener.Models
{
    [Table("UrlShorts")]
    public class UrlShort
    {
        [Key]
        public int Id { get; set; }
        public string ShortUrl { get; set; }
        public string UserUrl { get; set; }
    }
}
