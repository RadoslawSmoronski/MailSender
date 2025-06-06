using MailSender.Contracts.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MailSender.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<ClientApp> ClientApps { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
