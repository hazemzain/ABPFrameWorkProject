using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPCourse.Demo1.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ABPCourse.Demo1.Configration
{
    public class AppointmentConfigration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {

            builder.Property(x => x.PatientId).IsRequired();
            builder.Property(x => x.DoctorId).IsRequired();
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.Notes).HasMaxLength(500);
            builder.Property(x => x.IsPaid).IsRequired();
        }
    }
}
