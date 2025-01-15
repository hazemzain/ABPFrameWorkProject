using ABPCourse.Demo1.Catagories;
using ABPCourse.Demo1.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ABPCourse.Demo1.Configration
{
    public class PaymentConfigration: IEntityTypeConfiguration<payment>
    {
       

        public void Configure(EntityTypeBuilder<payment> builder)
        {
            builder.ConfigureByConvention();

            builder.HasKey(p => p.Id);

            builder.Property(p => p.TransactionId).IsRequired().HasMaxLength(50);

            builder.Property(p => p.Amount).IsRequired().HasColumnType("decimal(18,2)");

            builder.Property(p => p.PaymentDate).IsRequired();

            builder.Property(p => p.PaymentStatus).IsRequired().HasMaxLength(20);

            builder.Property(p => p.PayerName).IsRequired().HasMaxLength(100);
        }
    }
}
