using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ABPCourse.Demo1.Patient
{
    public class Patient:FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ContactNumber { get; set; }
        public string MedicalHistory { get; set; }
        public string Address { get; set; }

    }
}
