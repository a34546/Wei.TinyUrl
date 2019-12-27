using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Wei.Repository;
using Wei.TinyUrl.Data.Entities;

namespace Wei.TinyUrl.Data
{
    public class TinyUrlDbContext : BaseDbContext
    {
        public TinyUrlDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<UrlMapping> UrlMapping { get; set; }
    }
}
