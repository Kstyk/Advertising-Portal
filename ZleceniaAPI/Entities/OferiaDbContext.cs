using Microsoft.EntityFrameworkCore;

namespace ZleceniaAPI.Entities
{
    public class OferiaDbContext : DbContext
    {
        private string _connectionString = "Server=DESKTOP-MCPFS1N;Database=OferiaDb;Trusted_Connection=True;TrustServerCertificate=True;";
        public DbSet<StatusOfUser> StatusOfUsers { get; set; }
        public DbSet<TypeOfAccount> TypesOfAccounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UsersCategories> UsersCategories { get; set; }
        public DbSet<AreaOfWork> AreaOfWorks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
