using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalPhysio.Business.DTOs;
using DigitalPhysio.Business.Services;
using DigitalPhysio.Domain.Interfaces;
using DigitalPhysio.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DigitalPhysio.Tests.Business.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _mockRepository;
        private readonly Mock<ILogger<PatientService>> _mockLogger;
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _mockRepository = new Mock<IPatientRepository>();
            _mockLogger = new Mock<ILogger<PatientService>>();
            _service = new PatientService(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllPatientsAsync_ShouldReturnAllPatients_WhenPatientsExist()
        {
            // Arrange
            var patientList = new List<Patient>
            {
                new () { Id = 1, Name = "John Doe", Email = "john@example.com" },
                new () { Id = 2, Name = "Jane Smith", Email = "jane@example.com" },
                new () { Id = 3, Name = "Bob Johnson", Email = "bob@example.com" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(patientList);

            // Act
            var result = await _service.GetAllPatientsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(patientList.Count, result.Count());

            var resultList = result.ToList();
            for (int i = 0; i < patientList.Count; i++)
            {
                Assert.Equal(patientList[i].Id, resultList[i].Id);
                Assert.Equal(patientList[i].Name, resultList[i].Name);
                Assert.Equal(patientList[i].Email, resultList[i].Email);
            }

            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllPatientsAsync_ShouldReturnEmptyList_WhenNoPatientsExist()
        {
            // Arrange
            var emptyList = new List<Patient>();

            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(emptyList);

            // Act
            var result = await _service.GetAllPatientsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetPatientByIdAsync_ShouldReturnPatient_WhenPatientExists()
        {
            // Arrange
            int patientId = 1;
            var patient = new Patient { Id = patientId, Name = "John Doe", Email = "john@example.com" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(patientId)).ReturnsAsync(patient);

            // Act
            var result = await _service.GetPatientByIdAsync(patientId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(patient.Id, result.Id);
            Assert.Equal(patient.Name, result.Name);
            Assert.Equal(patient.Email, result.Email);
        }
    }
}