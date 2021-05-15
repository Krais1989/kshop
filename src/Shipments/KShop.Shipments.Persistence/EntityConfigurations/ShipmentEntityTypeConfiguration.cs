﻿using KShop.Shipments.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KShop.Shipments.Persistence.EntityConfigurations
{
    public class ShipmentEntityTypeConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.HasKey(e => e.ID);
        }
    }
}
