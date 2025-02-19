using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ABPCourse.Demo1.Patients
{
    public interface IPatientAppService : IApplicationService
    {
        public Task<PatientDto> CreatePatientAsync(CreatePatientDto NewPatientCreated);
        public Task<PatientDto> GetPatientByIdAsync(Guid id);
        public Task<List<PatientDto>> GetPatientsAsync();
        public Task<PatientDto> UpdatePatientAsync(Guid id, Patient.Patient patientToUpdate);
        public Task DeletePatientAsync(Guid id);
        Task<AppointmentDto> BookAppointmentAsync(CreateAppointmentDto input);

    }
}
