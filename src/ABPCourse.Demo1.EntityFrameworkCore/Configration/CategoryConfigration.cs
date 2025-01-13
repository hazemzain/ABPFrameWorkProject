using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPCourse.Demo1.Catagories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ABPCourse.Demo1.Configration
{
    public class CategoryConfigration : IEntityTypeConfiguration<Catogry>
    {
        public void Configure(EntityTypeBuilder<Catogry> builder)
        {
            builder.ConfigureByConvention();
            builder.Property(x=>x.Id).ValueGeneratedNever();
            builder.Property(x => x.NameAr).HasMaxLength(Demo1Consts.NameArLenght);
            builder.Property(x => x.NameEn).HasMaxLength(Demo1Consts.NameEnLenght);
            builder.Property(x => x.DescriptionAr).HasMaxLength(Demo1Consts.DescriptionArLenght);
            builder.Property(x => x.DescriptionEn).HasMaxLength(Demo1Consts.DescriptionEnLenght);
            builder.ToTable("Categories");
        }
    }
}
