using MailSender.Contracts.DTOs;
using MailSender.Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace MailSender.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<ClientApp> ClientApps { get; set; }
        public DbSet<MailLog> MailLogs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MailLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
