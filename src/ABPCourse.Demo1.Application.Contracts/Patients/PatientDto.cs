using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ABPCourse.Demo1.Patients
{
    public class PatientDto: EntityDto<Guid>
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ContactNumber { get; set; }
        public string MedicalHistory { get; set; }
        public string Address { get; set; }
    }
}
