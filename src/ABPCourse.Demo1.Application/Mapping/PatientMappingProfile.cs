using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPCourse.Demo1.Appointments;
using ABPCourse.Demo1.Messages;
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

            CreateMap<CreateAppointmentDto, Appointment>();
            CreateMap<CreateMessageDto, Message>();
            CreateMap<Appointment, AppointmentDto>();
            CreateMap<CreateAppointmentDto, Appointment>()
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.PatientId))
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes));
        }
    }
}
