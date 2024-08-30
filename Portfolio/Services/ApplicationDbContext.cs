using Microsoft.EntityFrameworkCore;
using Portfolio.Models;

namespace Portfolio.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Security> securities { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<SecurityShare> SecurityShares { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration des relations pour les SecurityShares
            modelBuilder.Entity<Security>()
                .HasMany(s => s.SecurityShares)
                .WithOne(ss => ss.Security)
                .HasForeignKey(ss => ss.SecurityId);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.SecurityShares)
                .WithOne(ss => ss.Account)
                .HasForeignKey(ss => ss.AccountId);

            // Configuration des relations pour Orders
            modelBuilder.Entity<Order>()
               .HasOne(o => o.Security)
               .WithMany(s => s.Orders)
               .HasForeignKey(o => o.SecurityId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Account)
                .WithMany(a => a.Orders)
                .HasForeignKey(o => o.AccountID);


            modelBuilder.Entity<Transaction>()
        .HasOne(t => t.Order)
        .WithMany(o => o.Transactions) // Assurez-vous que la navigation inverse est configurée si nécessaire
        .HasForeignKey(t => t.OrderID)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Security)
                .WithMany(s => s.Transactions) // Assurez-vous que la navigation inverse est configurée si nécessaire
                .HasForeignKey(t => t.SecurityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions) // Assurez-vous que la navigation inverse est configurée si nécessaire
                .HasForeignKey(t => t.AccountID)
                .OnDelete(DeleteBehavior.Restrict);


        }



        // Méthode pour mettre à jour la valeur totale du marché
        public async Task UpdateTotalMarketValueAsync(int accountId)
        {
            var account = await Accounts.Include(a => a.SecurityShares)
                                        .ThenInclude(ss => ss.Security)
                                        .FirstOrDefaultAsync(a => a.AccountID == accountId);

            if (account != null)
            {
                account.TotalMarketValue = account.SecurityShares.Sum(ss => ss.Quantite * (ss.Security?.MarketValue ?? 0));
                await SaveChangesAsync();
            }
        }

        public override int SaveChanges()
        {
            // Ne pas appeler UpdateAccountTotalMarketValues ici
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Ne pas appeler UpdateAccountTotalMarketValues ici
            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAccountTotalMarketValues(int accountId)
        {
            var account = await Accounts
                .Include(a => a.SecurityShares)
                    .ThenInclude(ss => ss.Security)
                .FirstOrDefaultAsync(a => a.AccountID == accountId);

            if (account == null)
            {
                throw new ArgumentException("Account not found");
            }

            // Calculer la valeur totale du marché en mémoire
            var totalMarketValue = account.SecurityShares
                .Sum(ss => (ss.Security?.MarketValue ?? 0) * ss.Quantite);

            account.TotalMarketValue = totalMarketValue;

            await SaveChangesAsync();
        }
    }
}
