using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ABPCourse.Demo1.Configration
{
    public class PatientConfigration : IEntityTypeConfiguration<Patient.Patient>
    {
        public void Configure(EntityTypeBuilder<Patient.Patient> builder)
        {
            builder.ConfigureByConvention();
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.DateOfBirth).IsRequired();
            builder.Property(p => p.ContactNumber).IsRequired().HasMaxLength(20);
            builder.Property(p => p.MedicalHistory).HasMaxLength(500);
            builder.Property(p => p.Address).HasMaxLength(500);
        }
    }
}
