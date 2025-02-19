using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPCourse.Demo1.Doctors;
using AutoMapper;

namespace ABPCourse.Demo1.Mapping
{
    public class DoctorMappingProfile: Profile
    {
        public DoctorMappingProfile()
        {
            CreateMap<Doctor, DoctorDto>();
            CreateMap<CreateDoctorDto, Doctor>();
        }
    }
}
