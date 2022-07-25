using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Pharmacy.Models.Helper
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Skills> Skills { get; set; }

    }
}
