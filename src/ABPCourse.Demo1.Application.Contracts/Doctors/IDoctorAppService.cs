using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABPCourse.Demo1.Doctors
{
    public interface IDoctorAppService
    {
        Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto input);
    }
}
