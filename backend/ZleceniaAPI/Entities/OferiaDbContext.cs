using Microsoft.EntityFrameworkCore;

namespace ZleceniaAPI.Entities
{
    public class OferiaDbContext : DbContext
    {
        public OferiaDbContext(DbContextOptions<OferiaDbContext> options) : base(options)
        {
            
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<StatusOfUser> StatusOfUsers { get; set; }
        public DbSet<TypeOfAccount> TypesOfAccounts { get; set; }
        public DbSet<UsersCategories> UsersCategories { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AreaOfWork> AreaOfWorks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Opinion> Opinions { get; set; }
        public DbSet<ResetPassword> ResetPasswords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Address
            modelBuilder.Entity<Address>()
                .Property(e => e.PostalCode)
                .IsRequired()
                .HasMaxLength(6);
            modelBuilder.Entity<Address>()
                .Property(e => e.City)
                .IsRequired()
                .HasMaxLength(255);
            modelBuilder.Entity<Address>()
                .Property(e => e.Street)
                .IsRequired()
                .HasMaxLength(255);
            modelBuilder.Entity<Address>()
                .Property(e => e.BuildingNumber)
                .IsRequired()
                .HasMaxLength(20);

            // Area of work
            modelBuilder.Entity<AreaOfWork>()
                .Property(e => e.Voivodeship)
                .HasMaxLength(60);
            modelBuilder.Entity<AreaOfWork>()
                .Property(e => e.WholeCountry)
                .HasMaxLength(60);

            // Category
            modelBuilder.Entity<Category>()
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            // User
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
            modelBuilder.Entity<User>()
                .Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(25);
            modelBuilder.Entity<User>()
                .Property(e => e.Description)
                .HasMaxLength(10000);
            modelBuilder.Entity<User>()
                .Property(e => e.CompanyName)
                .HasMaxLength(255);

            // Order
            modelBuilder.Entity<Order>()
                .Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(2500);
            modelBuilder.Entity<Order>()
                .Property(e => e.Budget)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Order>()
                .Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(255);
            modelBuilder.Entity<Order>()
               .HasOne(e => e.WinnerOffer)
               .WithMany()
               .HasForeignKey(e => e.WinnerOfferId)
               .OnDelete(DeleteBehavior.NoAction);

            // Offer
            modelBuilder.Entity<Offer>()
                .HasOne<Order>(s => s.Order)
                .WithMany(of => of.Offers)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Offer>()
                .Property(e => e.Content)
                .IsRequired()
                .HasMaxLength(2000);
            modelBuilder.Entity<Offer>()
               .Property(e => e.Price)
               .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Offer>()
                .Property(e => e.PriceFor)
                .HasMaxLength(50);

            // Opinions
            modelBuilder.Entity<Opinion>()
                .Property(e => e.Comment)
                .HasMaxLength(5000);
            modelBuilder.Entity<Opinion>()
                .HasOne<Order>(s => s.Order)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Opinion>()
                .HasOne<Offer>(s => s.Offer)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Opinion>()
                .HasOne<User>(s => s.Principal)
                .WithMany()
                .HasForeignKey(o => o.PrincipalId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Opinion>()
                .HasOne<User>(s => s.Contractor)
                .WithMany()
               .HasForeignKey(o => o.ContractorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
