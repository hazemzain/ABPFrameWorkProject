using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPCourse.Demo1.Patients;
using ABPCourse.Demo1.Payment;
using AutoMapper.Internal.Mappers;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using IObjectMapper = Volo.Abp.ObjectMapping.IObjectMapper;

namespace ABPCourse.Demo1.Patient
{
    [IgnoreAntiforgeryToken]
    public class PatientAppService : ApplicationService, IPatientAppService
    {
        private readonly IRepository<Patient, Guid> _patientRepository;
        private readonly IObjectMapper _objectMapper;

        public PatientAppService(IRepository<Patient, Guid> patientRepository, IObjectMapper objectMapper)
        {
            _patientRepository = patientRepository;
            _objectMapper = objectMapper;

        }
        public async Task<PatientDto> CreatePatientAsync(CreatePatientDto NewPatientCreated)
        {
            var patient = _objectMapper.Map<CreatePatientDto, Patient>(NewPatientCreated);
            if (patient == null)
            {
                throw new UserFriendlyException("Mapping failed! Please check input data.");
            }
            var inputValidation = new PatientValidator().Validate(patient);
            if (!inputValidation.IsValid)
            {
                throw new UserFriendlyException(("ValidationErrors"),
                    inputValidation.Errors.Select(e => e.ErrorMessage).JoinAsString(", "));
            }

            try
            {
                await _patientRepository.InsertAsync(patient);
                return _objectMapper.Map<Patient, PatientDto>(patient);

            }
            catch (Exception e)
            {

                throw new ApplicationException("An error occurred while creating the patient.", e);
            }

        }

        public async Task DeletePatientAsync(Guid id)
        {
            await _patientRepository.DeleteAsync(id);
        }

        public async Task<PatientDto> GetPatientByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new NullReferenceException("The Id Should not be Empty.");
            }
            var patient = await _patientRepository.GetAsync(id);
            return _objectMapper.Map<Patient, PatientDto>(patient);
        }

        public async Task<List<PatientDto>> GetPatientsAsync()
        {
            var patients = await _patientRepository.GetListAsync();
            return _objectMapper.Map<List<Patient>, List<PatientDto>>(patients);
        }

        public async Task<PatientDto> UpdatePatientAsync(Guid id, Patient patientToUpdate)
        {
            if (id == Guid.Empty)
            {
                throw new NullReferenceException("The Id Should not be Empty.");
            }
            if(patientToUpdate==null)
            {
                throw new NullReferenceException("The Patient Should not be Empty.");
            }
            var patient = await _patientRepository.GetAsync(id);
            patient.Name = patientToUpdate.Name;
            patient.DateOfBirth = patientToUpdate.DateOfBirth;
            patient.ContactNumber = patientToUpdate.ContactNumber;
            patient.MedicalHistory = patientToUpdate.MedicalHistory;
            patient.Address = patientToUpdate.Address;
            await _patientRepository.UpdateAsync(patient);
            // Fetch the updated entity (Optional, but ensures latest data)
            var updatedPatient = await _patientRepository.GetAsync(id);

            return _objectMapper.Map<Patient, PatientDto>(updatedPatient);
        }
    }
}
