using Microsoft.EntityFrameworkCore;

namespace ZleceniaAPI.Entities
{
    public class OferiaDbContext : DbContext
    {
        private string _connectionString = "Server=DESKTOP-MCPFS1N;Database=OferiaDb;Trusted_Connection=True;TrustServerCertificate=True;";
        public DbSet<Address> Addresses { get; set; }
        public DbSet<StatusOfUser> StatusOfUsers { get; set; }
        public DbSet<TypeOfAccount> TypesOfAccounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UsersCategories> UsersCategories { get; set; }
        public DbSet<AreaOfWork> AreaOfWorks { get; set; }
        public DbSet<Order> Orders { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<User>()
                .Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<User>() 
                .Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);
            modelBuilder.Entity<User>()
               .HasOne(e => e.Address)
               .WithMany()
               .HasForeignKey(e => e.AddressId)
               .OnDelete(DeleteBehavior.NoAction);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
