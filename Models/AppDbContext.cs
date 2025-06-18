using Microsoft.EntityFrameworkCore;
using System;

namespace W04_Phase_01.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Person> Person { get; set; }
        //public DbSet<Product> Products { get; set; }
        //public DbSet<SalesTransaction> SalesTransactions { get; set; }
    }
}