using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace ABPCourse.Demo1.Appointments
{
    public class Appointment: Entity<Guid>
    {
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public bool IsPaid { get; set; }
    }
}
