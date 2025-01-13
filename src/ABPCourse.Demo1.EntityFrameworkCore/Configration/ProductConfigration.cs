using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPCourse.Demo1.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ABPCourse.Demo1.Configration
{
    public class ProductConfigration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ConfigureByConvention();
            

            builder.HasKey(p => p.Id);

            builder.Property(p => p.NameAr).IsRequired().HasMaxLength(200);

            builder.Property(p => p.NameEn).IsRequired().HasMaxLength(200);

            builder.Property(p => p.DescriptionAr).HasMaxLength(1000);

            builder.Property(p => p.DescriptionEn).HasMaxLength(1000);

            builder.HasOne(p => p.Category).WithMany().HasForeignKey(p => p.CategoryId).IsRequired();
            builder.ToTable("Products");
        }
    }
}
