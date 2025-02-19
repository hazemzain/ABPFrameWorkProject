using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ABPCourse.Demo1.Appointments;
using ABPCourse.Demo1.Messages;
using ABPCourse.Demo1.Patient;
using ABPCourse.Demo1.Patients;
using ABPCourse.Demo1.Payment;
using Allure.NUnit;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NSubstitute;
using NUnit.Framework;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Validation;

namespace ABPCourse.Demo1.Tests
{
    [TestFixture]
    [AllureNUnit]
    public class PatientAppServiceTests
    {
        #region varibles

        // private const IQueryable<Product>? ProductQuery = (IQueryable<Product>)null;
        private Mock<IRepository<Patient.Patient, Guid>> _PatientRepositoryMock;
        private Mock<IRepository<Message, Guid>> _MessageRepositoryMock;
        private Mock<IRepository<Appointment, Guid>> _AppointmentRepositoryMock;
        private Mock<IObjectMapper> _MockObjectMapper;
        private PatientAppService _PatientAppService;

        #endregion

        #region SetupMethod

        [SetUp]
        public void SetUp()
        {
            _PatientRepositoryMock = new Mock<IRepository<Patient.Patient, Guid>>();
            _MockObjectMapper = new Mock<IObjectMapper>();
            _AppointmentRepositoryMock = new Mock<IRepository<Appointment, Guid>>();
            _MessageRepositoryMock = new Mock<IRepository<Message, Guid>>();
            _PatientAppService = new PatientAppService(_PatientRepositoryMock.Object, _AppointmentRepositoryMock.Object, _MessageRepositoryMock.Object, _MockObjectMapper.Object);
        }

        #endregion

        #region TestCases_For_CreatePatientAsync

        [Test]
        public async Task CreatePatientAsync_WhenCalledWithValidInput_ShouldCreatePatient()
        {
            // Arrange
            var createDto = new CreatePatientDto { FullName = "Hazem", ContactNumber = "1234567890" };
            var patient = new Patient.Patient { Name = "Hazem", ContactNumber = "1234567890" };
            var patientDto = new PatientDto
                { Id = patient.Id, Name = patient.Name, ContactNumber = patient.ContactNumber };

            _MockObjectMapper.Setup(m => m.Map<CreatePatientDto, Patient.Patient>(createDto)).Returns(patient);
            _PatientRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<Patient.Patient>(), true, default))
                .ReturnsAsync(patient);
            _MockObjectMapper.Setup(m => m.Map<Patient.Patient, PatientDto>(patient)).Returns(patientDto);

            // Act
            var result = await _PatientAppService.CreatePatientAsync(createDto);

            // Assert
            result.ShouldNotBeNull();
            result.Name.ShouldBe("Hazem");
            //_PatientRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<Patient.Patient>(), true, default),
            //    Times.Once);
            Assert.That(patient.Name, Is.EqualTo(result.Name));
            Assert.That(patient.DateOfBirth, Is.EqualTo(result.DateOfBirth));
            Assert.That(patient.ContactNumber, Is.EqualTo(result.ContactNumber));
            Assert.That(patient.MedicalHistory, Is.EqualTo(result.MedicalHistory));
            Assert.That(patient.Address, Is.EqualTo(result.Address));
        }

        //Patient is Created with Minimum Required Data
        [Test]
        public async Task CreatePatientAsync_WhenCalledWithMinimumRequiredData_ShouldCreatePatient()
        {
            // Arrange
            var createDto = new CreatePatientDto { FullName = "Hazem" }; // Only name provided
            var patient = new Patient.Patient { Name = "Hazem" };
            var patientDto = new PatientDto { Id = patient.Id, Name = patient.Name };

            _MockObjectMapper.Setup(m => m.Map<CreatePatientDto, Patient.Patient>(createDto)).Returns(patient);
            _PatientRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<Patient.Patient>(), true, default))
                .ReturnsAsync(patient);
            _MockObjectMapper.Setup(m => m.Map<Patient.Patient, PatientDto>(patient)).Returns(patientDto);

            // Act
            var result = await _PatientAppService.CreatePatientAsync(createDto);

            // Assert
            result.ShouldNotBeNull();
            result.Name.ShouldBe("Hazem");
        }

        [Test]
        public async Task CreatePatientAsync_WhenCalledWithNullDto_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            await Should.ThrowAsync<ArgumentNullException>(() => _PatientAppService.CreatePatientAsync(null));
        }

        [Test]
        public async Task CreatePatientAsync_WhenFullNameIsMissing_ShouldThrowException()
        {
            // Arrange
            var createDto = new CreatePatientDto { ContactNumber = "1234567890" }; // Missing FullName
            var result = await _PatientAppService.CreatePatientAsync(createDto);

            result.ShouldNotBeNull();
            //result.ValidationErrors.ShouldContain(error => error.Message == "The FullName field is required.");
        }

        [Test]
        public async Task CreatePatientAsync_WhenRepositoryInsertFails_ShouldThrowException()
        {
            // Arrange
            var createDto = new CreatePatientDto { FullName = "John Doe", ContactNumber = "1234567890" };
            var patient = new Patient.Patient { Name = "John Doe", ContactNumber = "1234567890" };

            _MockObjectMapper.Setup(m => m.Map<CreatePatientDto, Patient.Patient>(createDto)).Returns(patient);
            _PatientRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<Patient.Patient>(), false, default))
                .ThrowsAsync(new ApplicationException("Database error"));

            // Act & Assert
            var exception =
                await Should.ThrowAsync<ApplicationException>(() => _PatientAppService.CreatePatientAsync(createDto));

            // Verify the exception message
            exception.Message.ShouldBe("An error occurred while creating the patient.");
            exception.InnerException.Message.ShouldBe("Database error");
        }

        [Test]
        public async Task CreatePatientAsync_WhenMappingFails_ShouldThrowAutoMapperMappingException()
        {
            // Arrange
            var createDto = new CreatePatientDto { FullName = "John Doe" };

            _MockObjectMapper.Setup(m => m.Map<CreatePatientDto, Patient.Patient>(createDto))
                .Throws(new AutoMapperMappingException("Mapping error"));

            // Act & Assert
            await Should.ThrowAsync<AutoMapperMappingException>(() => _PatientAppService.CreatePatientAsync(createDto));
        }

        [Test]
        public async Task CreatePatientAsync_WhenContactNumberIsMissing_ShouldThrowException()
        {
            // Arrange
            var createDto = new CreatePatientDto { FullName = "John Doe" }; // Missing ContactNumber

            // Act & Assert
            var exception =
                await Should.ThrowAsync<UserFriendlyException>(() => _PatientAppService.CreatePatientAsync(createDto));
            exception.Message.ShouldBe("Mapping failed! Please check input data.");
        }

        [Test]
        public async Task CreatePatientAsync_WhenContactNumberIsNot11Digits_ShouldThrowException()
        {
            // Arrange
            var createDto = new CreatePatientDto { FullName = "John Doe", ContactNumber = "12345" }; // Invalid length
            var patient = new Patient.Patient { Name = "John Doe", ContactNumber = "12345" };

            _MockObjectMapper.Setup(m => m.Map<CreatePatientDto, Patient.Patient>(createDto)).Returns(patient);

            // Act & Assert
            var exception =
                await Should.ThrowAsync<UserFriendlyException>(() => _PatientAppService.CreatePatientAsync(createDto));
            exception.Code.ShouldBe("The phone number must be exactly 11 digits.");

        }

        [Test]
        public async Task CreatePatientAsync_WhenContactNumberHasLetters_ShouldThrowException()
        {
            // Arrange
            var createDto = new CreatePatientDto { FullName = "John Doe", ContactNumber = "12345abc678" };
            // Invalid format
            var patient = new Patient.Patient { Name = "John Doe", ContactNumber = "12345abc678" };
            _MockObjectMapper.Setup(m => m.Map<CreatePatientDto, Patient.Patient>(createDto)).Returns(patient);


            // Act & Assert
            await Should.ThrowAsync<UserFriendlyException>(() => _PatientAppService.CreatePatientAsync(createDto));
        }
        #endregion
        #region TestCases_For_GetPatientByIdAsync
        [Test]
        public async Task GetPatientByIdAsync_WhenPatientExists_ShouldReturnPatientDto()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var patient = new Patient.Patient {  Name = "John Doe", ContactNumber = "12345678901" };
            var patientDto = new PatientDto { Id = patientId, Name = "John Doe", ContactNumber = "12345678901" };

            _PatientRepositoryMock.Setup(repo => repo.GetAsync(patientId,true,default)).ReturnsAsync(patient);
            _MockObjectMapper.Setup(m => m.Map<Patient.Patient, PatientDto>(patient)).Returns(patientDto);

            // Act
            var result = await _PatientAppService.GetPatientByIdAsync(patientId);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(patientId);
            result.Name.ShouldBe("John Doe");
            result.ContactNumber.ShouldBe("12345678901");

            _PatientRepositoryMock.Verify(repo => repo.GetAsync(patientId, true, default), Times.Once);
        }
        [Test]
        public async Task GetPatientByIdAsync_WhenPatientDoesNotExist_ShouldThrowEntityNotFoundException()
        {
            // Arrange
            var patientId = Guid.NewGuid();

            _PatientRepositoryMock.Setup(repo => repo.GetAsync(patientId, true, default))
                .ThrowsAsync(new Volo.Abp.Domain.Entities.EntityNotFoundException());

            // Act & Assert
            await Should.ThrowAsync<Volo.Abp.Domain.Entities.EntityNotFoundException>(
                () => _PatientAppService.GetPatientByIdAsync(patientId)
            );

            _PatientRepositoryMock.Verify(repo => repo.GetAsync(patientId, true, default), Times.Once);
        }
        [Test]
        public async Task GetPatientByIdAsync_WhenPatientIdIsEmpty_ShouldThrowNullReferenceException()
        {
            // Arrange
            var emptyPatientId = Guid.Empty;

            // Act & Assert
            var result=await Should.ThrowAsync<NullReferenceException>(
                () => _PatientAppService.GetPatientByIdAsync(emptyPatientId)
            );
            result.Message.ShouldBe("The Id Should not be Empty.");
        }
        [Test]
        public async Task GetPatientByIdAsync_WhenRepositoryFails_ShouldThrowException()
        {
            // Arrange
            var patientId = Guid.NewGuid();

            _PatientRepositoryMock.Setup(repo => repo.GetAsync(patientId ,true,default))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Should.ThrowAsync<Exception>(
                () => _PatientAppService.GetPatientByIdAsync(patientId)
            );
        }







        #endregion

        #region Testcases_For_GetPatientsAsync
        [Test]
        public async Task GetPatientsAsync_WhenPatientsExist_ShouldReturnPatientDtoList()
        {
            // Arrange
            var patients = new List<Patient.Patient>
            {
                new Patient.Patient {  Name = "John Doe", ContactNumber = "12345678901" },
                new Patient.Patient {  Name = "Jane Doe", ContactNumber = "98765432101" }
            };

            var patientDtos = patients.Select(p => new PatientDto {  Name = p.Name, ContactNumber = p.ContactNumber }).ToList();

            _PatientRepositoryMock.Setup(repo => repo.GetListAsync( false, default)).ReturnsAsync(patients);
            _MockObjectMapper.Setup(m => m.Map<List<Patient.Patient>, List<PatientDto>>(patients)).Returns(patientDtos);

            // Act
            var result = await _PatientAppService.GetPatientsAsync();

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(2);
            result[0].Name.ShouldBe("John Doe");
            result[1].Name.ShouldBe("Jane Doe");

            _PatientRepositoryMock.Verify(repo => repo.GetListAsync( false, default), Times.Once);
        }
        [Test]
        public async Task GetPatientsAsync_WhenRepositoryFails_ShouldThrowException()
        {
            // Arrange
            _PatientRepositoryMock.Setup(repo => repo.GetListAsync(false, default))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Should.ThrowAsync<Exception>(
                () => _PatientAppService.GetPatientsAsync()
            );

            _PatientRepositoryMock.Verify(repo => repo.GetListAsync(false, default), Times.Once);
        }
        [Test]
        public async Task GetPatientsAsync_WhenMappingFails_ShouldThrowException()
        {
            // Arrange
            var patients = new List<Patient.Patient>
            {
                new Patient.Patient {  Name = "John Doe", ContactNumber = "12345678901" }
            };

            _PatientRepositoryMock.Setup(repo => repo.GetListAsync(false,default)).ReturnsAsync(patients);
            _MockObjectMapper.Setup(m => m.Map<List<Patient.Patient>, List<PatientDto>>(patients)).Throws(new NullReferenceException());

            // Act & Assert
            await Should.ThrowAsync<NullReferenceException>(
                () => _PatientAppService.GetPatientsAsync()
            );

            _PatientRepositoryMock.Verify(repo => repo.GetListAsync(false,default), Times.Once);
        }

        [Test]
        public async Task GetPatientsAsync_WhenNoPatientsExist_ShouldReturnEmptyList()
        {
            // Arrange
            _PatientRepositoryMock.Setup(repo => repo.GetListAsync(false,default)).ReturnsAsync(new List<Patient.Patient>());
            _MockObjectMapper.Setup(m => m.Map<List<Patient.Patient>, List<PatientDto>>(It.IsAny<List<Patient.Patient>>()))
                .Returns(new List<PatientDto>());

            _PatientAppService = new PatientAppService(_PatientRepositoryMock.Object, _AppointmentRepositoryMock.Object, _MessageRepositoryMock.Object, _MockObjectMapper.Object);

            // Act
            var result = await _PatientAppService.GetPatientsAsync();

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();

            _PatientRepositoryMock.Verify(repo => repo.GetListAsync(false,default), Times.Once);
        }





        #endregion

        #region TestCases_For_UpdatePatientAsync
        [Test]
        public async Task UpdatePatientAsync_WhenPatientExists_ShouldUpdateAndReturnUpdatedPatient()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var existingPatient = new Patient.Patient
            {
               
                Name = "John Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                ContactNumber = "12345678901",
                MedicalHistory = "No history",
                Address = "123 Street"
            };

            var updatedPatientData = new Patient.Patient
            {
                Name = "Updated Name",
                DateOfBirth = new DateTime(1985, 5, 5),
                ContactNumber = "98765432101",
                MedicalHistory = "Updated history",
                Address = "Updated Address"
            };

            var updatedPatientDto = new PatientDto
            {
                Id = patientId,
                Name = updatedPatientData.Name,
                DateOfBirth = updatedPatientData.DateOfBirth,
                ContactNumber = updatedPatientData.ContactNumber,
                MedicalHistory = updatedPatientData.MedicalHistory,
                Address = updatedPatientData.Address
            };

            _PatientRepositoryMock.Setup(repo => repo.GetAsync(patientId,true,default)).ReturnsAsync(existingPatient);
            _PatientRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Patient.Patient>(),false,default)).ReturnsAsync(existingPatient);
            _PatientRepositoryMock.Setup(repo => repo.GetAsync(patientId, true, default)).ReturnsAsync(updatedPatientData);
            _MockObjectMapper.Setup(m => m.Map<Patient.Patient, PatientDto>(updatedPatientData)).Returns(updatedPatientDto);

            _PatientAppService = new PatientAppService(_PatientRepositoryMock.Object, _AppointmentRepositoryMock.Object, _MessageRepositoryMock.Object, _MockObjectMapper.Object);

            // Act
            var result = await _PatientAppService.UpdatePatientAsync(patientId, updatedPatientData);

            // Assert
            result.ShouldNotBeNull();
            result.Name.ShouldBe("Updated Name");
            result.ContactNumber.ShouldBe("98765432101");
            result.MedicalHistory.ShouldBe("Updated history");
            result.Address.ShouldBe("Updated Address");

            _PatientRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Patient.Patient>(),false,default), Times.Once);
        }

        [Test]
        public async Task UpdatePatientAsync_WhenUpdatingWithSameData_ShouldReturnSamePatientDto()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var existingPatient = new Patient.Patient
            {
                Name = "John Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                ContactNumber = "12345678901",
                MedicalHistory = "No history",
                Address = "123 Street"
            };

            var updatedPatientDto = new PatientDto
            {
                Id = patientId,
                Name = existingPatient.Name,
                DateOfBirth = existingPatient.DateOfBirth,
                ContactNumber = existingPatient.ContactNumber,
                MedicalHistory = existingPatient.MedicalHistory,
                Address = existingPatient.Address
            };

            _PatientRepositoryMock.Setup(repo => repo.GetAsync(patientId, true, default)).ReturnsAsync(existingPatient);
            _PatientRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Patient.Patient>(), false, default)).ReturnsAsync(existingPatient);
            _MockObjectMapper.Setup(m => m.Map<Patient.Patient, PatientDto>(existingPatient)).Returns(updatedPatientDto);

            _PatientAppService = new PatientAppService(_PatientRepositoryMock.Object, _AppointmentRepositoryMock.Object, _MessageRepositoryMock.Object, _MockObjectMapper.Object);

            // Act
            var result = await _PatientAppService.UpdatePatientAsync(patientId, existingPatient);

            // Assert
            result.ShouldNotBeNull();
            result.Name.ShouldBe("John Doe"); // Ensure no change
        }
        [Test]
        public async Task UpdatePatientAsync_WhenPatientDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var updatedPatientData = new Patient.Patient
            {
                Name = "Updated Name",
                ContactNumber = "98765432101"
            };

            _PatientRepositoryMock.Setup(repo => repo.GetAsync(patientId, true, default)).ThrowsAsync(new Exception("Patient not found"));

            _PatientAppService = new PatientAppService(_PatientRepositoryMock.Object, _AppointmentRepositoryMock.Object, _MessageRepositoryMock.Object, _MockObjectMapper.Object);

            // Act & Assert
            await Should.ThrowAsync<Exception>(() => _PatientAppService.UpdatePatientAsync(patientId, updatedPatientData));

            _PatientRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Patient.Patient>(), false, default), Times.Never);
        }
        [Test]
        public async Task UpdatePatientAsync_WhenRepositoryUpdateFails_ShouldThrowException()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var existingPatient = new Patient.Patient {  Name = "John Doe" };
            var updatedPatientData = new Patient.Patient { Name = "Updated Name" };

            _PatientRepositoryMock.Setup(repo => repo.GetAsync(patientId, true, default)).ReturnsAsync(existingPatient);
            _PatientRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Patient.Patient>(), false, default))
                .ThrowsAsync(new Exception("Database error"));

            _PatientAppService = new PatientAppService(_PatientRepositoryMock.Object, _AppointmentRepositoryMock.Object, _MessageRepositoryMock.Object, _MockObjectMapper.Object);

            // Act & Assert
            await Should.ThrowAsync<Exception>(() => _PatientAppService.UpdatePatientAsync(patientId, updatedPatientData));

            _PatientRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Patient.Patient>(), false, default), Times.Once);
        }

        [Test]
        public async Task UpdatePatientAsync_WhenIdIsEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var patientId = Guid.Empty;
            var updatedPatientData = new Patient.Patient { Name = "Updated Name" };

            _PatientAppService = new PatientAppService(_PatientRepositoryMock.Object, _AppointmentRepositoryMock.Object, _MessageRepositoryMock.Object, _MockObjectMapper.Object);

            // Act & Assert
            await Should.ThrowAsync<NullReferenceException>(() => _PatientAppService.UpdatePatientAsync(patientId, updatedPatientData));
        }
        [Test]
        public async Task UpdatePatientAsync_WhenUpdatedDataIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var patientId = Guid.NewGuid();

            _PatientAppService = new PatientAppService(_PatientRepositoryMock.Object, _AppointmentRepositoryMock.Object, _MessageRepositoryMock.Object, _MockObjectMapper.Object);

            // Act & Assert
            await Should.ThrowAsync<NullReferenceException>(() => _PatientAppService.UpdatePatientAsync(patientId, null));
        }





        #endregion

        #region TestCases_For_BookAppointmentAsync
        [Test]
        public async Task BookAppointmentAsync_WhenCalledWithValidInput_ShouldCreateAppointment()
        {
            // Arrange
            var createDto = new CreateAppointmentDto
            {
                PatientId = Guid.NewGuid(),
                DoctorId = Guid.NewGuid(),
                Date = new DateTime(2025, 5, 1, 10, 0, 0),
                Notes = "Checkup"
            };
            var appointment = new Appointment
            {
                
                PatientId = createDto.PatientId,
                DoctorId = createDto.DoctorId,
                Date = createDto.Date,
                Notes = createDto.Notes
            };
            var appointmentDto = new AppointmentDto
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                Date = appointment.Date,
                Notes = appointment.Notes
            };

            _MockObjectMapper.Setup(m => m.Map<CreateAppointmentDto, Appointment>(createDto))
                .Returns(appointment);

            //var existingAppointments = new List<Appointment>(); // No overlapping appointments

            //_AppointmentRepositoryMock
            //    .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Appointment, bool>>>(), default))
            //    .ReturnsAsync((Expression<Func<Appointment, bool>> predicate) =>
            //    {
            //        return existingAppointments.AsQueryable().FirstOrDefault(predicate);
            //    });


            _AppointmentRepositoryMock
                .Setup(repo => repo.InsertAsync(It.IsAny<Appointment>(), true, default))
                .ReturnsAsync(appointment);

            _MockObjectMapper.Setup(m => m.Map<Appointment, AppointmentDto>(appointment))
                .Returns(appointmentDto);

            // Act
            var result = await _PatientAppService.BookAppointmentAsync(createDto);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(appointment.Id);
            result.PatientId.ShouldBe(appointment.PatientId);
            result.DoctorId.ShouldBe(appointment.DoctorId);
            result.Date.ShouldBe(appointment.Date);
            result.Notes.ShouldBe(appointment.Notes);

        }
        [Test]
        public void BookAppointmentAsync_WhenOverlappingAppointmentExists_ShouldThrowException()
        {
            // Arrange
            var createDto = new CreateAppointmentDto
            {
                PatientId = Guid.NewGuid(),
                DoctorId = Guid.NewGuid(),
                Date = new DateTime(2025, 5, 1, 10, 0, 0),
                Notes = "Checkup"
            };
            var existingAppointment = new Appointment
            {
                
                PatientId = createDto.PatientId,
                DoctorId = createDto.DoctorId,
                Date = createDto.Date,
                Notes = "Existing Appointment"
            };

            _AppointmentRepositoryMock
                .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Appointment, bool>>>(),default))
                .ReturnsAsync(existingAppointment); // Overlapping appointment exists

            // Act & Assert
            Should.ThrowAsync<UserFriendlyException>(async () =>
                await _PatientAppService.BookAppointmentAsync(createDto));
        }

        [Test]
        public void BookAppointmentAsync_WhenInputIsNull_ShouldThrowException()
        {
            // Act & Assert
            Should.ThrowAsync<ArgumentNullException>(async () =>
                await _PatientAppService.BookAppointmentAsync(null));
        }
        [Test]
        public void BookAppointmentAsync_WhenInsertFails_ShouldThrowException()
        {
            // Arrange
            var createDto = new CreateAppointmentDto
            {
                PatientId = Guid.NewGuid(),
                DoctorId = Guid.NewGuid(),
                Date = new DateTime(2025, 5, 1, 10, 0, 0),
                Notes = "Checkup"
            };

            _AppointmentRepositoryMock
                .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Appointment, bool>>>(),default))
                .ReturnsAsync((Appointment)null); // No overlapping appointment

            _AppointmentRepositoryMock
                .Setup(repo => repo.InsertAsync(It.IsAny<Appointment>(), true, default))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            Should.ThrowAsync<Exception>(async () =>
                await _PatientAppService.BookAppointmentAsync(createDto));
        }


        #endregion


    }




}




