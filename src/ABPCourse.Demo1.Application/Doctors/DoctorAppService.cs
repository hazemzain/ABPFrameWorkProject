using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace ABPCourse.Demo1.Doctors
{
    [IgnoreAntiforgeryToken]
    public class DoctorAppService: ApplicationService, IDoctorAppService
    {
        private readonly IRepository<Doctor, Guid> _doctorRepository;
        private readonly IObjectMapper _objectMapper;



        public DoctorAppService(IRepository<Doctor, Guid> doctorRepository, IObjectMapper objectMapper)
        {
            _doctorRepository = doctorRepository;

            _objectMapper = objectMapper;

        }
        public async Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto input)
        {
            var doctor = _objectMapper.Map<CreateDoctorDto, Doctor>(input);
            await _doctorRepository.InsertAsync(doctor);
            return _objectMapper.Map<Doctor, DoctorDto>(doctor);
        }
    }
}
