using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace UrlShort.Models
{
    public class UrlContext : DbContext
    {

        public UrlContext(DbContextOptions<UrlContext> options)
            : base(options)
        {
        }

        public DbSet<Url> Urls { get; set; }        
    }
}
