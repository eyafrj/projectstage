using Microsoft.EntityFrameworkCore;
using BLL.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.core.Models;

namespace DAL.EF
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Security> Securities { get; set; }
        public DbSet<SecurityShare> SecurityShare { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                    .HasOne(t => t.Account)
                    .WithMany()
                    .HasForeignKey(t => t.AccountID)
                    .OnDelete(DeleteBehavior.Restrict); // Change to Restrict

            modelBuilder.Entity<Transaction>()
                    .HasOne(t => t.Order)
                    .WithMany()
                    .HasForeignKey(t => t.OrderID)
                    .OnDelete(DeleteBehavior.Restrict); // Change to Restrict

            modelBuilder.Entity<Transaction>()
                    .HasOne(t => t.Security)
                    .WithMany()
                    .HasForeignKey(t => t.SecurityId)
                    .OnDelete(DeleteBehavior.Restrict); // 
        }


    }
}
