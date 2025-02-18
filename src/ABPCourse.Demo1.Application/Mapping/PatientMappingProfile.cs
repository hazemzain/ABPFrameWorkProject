using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPCourse.Demo1.Patients;
using AutoMapper;

namespace ABPCourse.Demo1.Mapping
{
    public class PatientMappingProfile: Profile
    {

        public PatientMappingProfile()
        {
            CreateMap<Patient.Patient, PatientDto>();
            CreateMap<PatientDto, Patient.Patient>();
            CreateMap<CreatePatientDto, Patient.Patient > ();
            CreateMap<CreatePatientDto, Patient.Patient>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
        }
    }
}
