using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MyUrlShortener.Models
{
    public class UrlShortenerContext : DbContext
    {
        public DbSet<UrlShort> UrlShorts { get; set; }


        public UrlShortenerContext(DbContextOptions<UrlShortenerContext> options) : base (options)
        {

        }
    }
}
