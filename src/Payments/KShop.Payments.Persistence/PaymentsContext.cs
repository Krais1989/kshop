
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Payments.Persistence
{
    /// <summary>
    /// Контекст для миграций
    /// </summary>
    public class OrderContextDesignFactory : IDesignTimeDbContextFactory<PaymentsContext>
    {
        public PaymentsContext CreateDbContext(string[] args)
        {
            var constr = "Server=127.0.0.1;Port=3308;Database=db_payments;Uid=asd;Pwd=asd;";
            Console.WriteLine($"Design context: {constr}");
            var optionsBuilder = new DbContextOptionsBuilder<PaymentsContext>();
            optionsBuilder.UseMySql(constr, new MySqlServerVersion(new Version(8, 0)));
            return new PaymentsContext(optionsBuilder.Options);
        }
    }

    public class PaymentsContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentLog> PaymentLogs { get; set; }

        public PaymentsContext()
        {
        }

        public PaymentsContext(DbContextOptions<PaymentsContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentsContext).Assembly);
        }

    }
}
