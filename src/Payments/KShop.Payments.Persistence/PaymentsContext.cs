﻿using KShop.Payments.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Payments.Persistence
{
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql("Server=127.0.0.1;Port=3308;Database=db_payments;Uid=asd;Pwd=asd;", new MySqlServerVersion(new Version(8, 0)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentsContext).Assembly);
        }

    }
}
