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
    public class PrescriptionServiceTests
    {
        private readonly Mock<IPrescriptionRepository> _mockPrescriptionRepository;
        private readonly Mock<IPatientRepository> _mockPatientRepository;
        private readonly Mock<IExerciseRepository> _mockExerciseRepository;
        private readonly Mock<ILogger<PrescriptionService>> _mockLogger;
        private readonly PrescriptionService _service;

        public PrescriptionServiceTests()
        {
            _mockPrescriptionRepository = new Mock<IPrescriptionRepository>();
            _mockPatientRepository = new Mock<IPatientRepository>();
            _mockExerciseRepository = new Mock<IExerciseRepository>();
            _mockLogger = new Mock<ILogger<PrescriptionService>>();

            _service = new PrescriptionService(
                _mockPrescriptionRepository.Object,
                _mockPatientRepository.Object,
                _mockExerciseRepository.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task GetAllPrescriptionsAsync_ShouldReturnAllPrescriptions()
        {
            // Arrange
            var prescriptions = new List<Prescription>
            {
                new () { Id = 1, PatientId = 1, CreatedDate = DateTime.Now.AddDays(-5), Notes = "Notes 1" },
                new () { Id = 2, PatientId = 2, CreatedDate = DateTime.Now.AddDays(-3), Notes = "Notes 2" }
            };

            var patient1 = new Patient { Id = 1, Name = "John Doe", Email = "john@example.com" };
            var patient2 = new Patient { Id = 2, Name = "Jane Smith", Email = "jane@example.com" };

            var exercises1 = new List<Exercise>
            {
                new () { Id = 1, Name = "Exercise 1", Description = "Description 1", RepetitionCount = 10, Sets = 3 }
            };

            var exercises2 = new List<Exercise>
            {
                new () { Id = 2, Name = "Exercise 2", Description = "Description 2", RepetitionCount = 15, Sets = 2 },
                new () { Id = 3, Name = "Exercise 3", Description = "Description 3", RepetitionCount = 8, Sets = 4 }
            };

            _mockPrescriptionRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(prescriptions);

            _mockPrescriptionRepository.Setup(repo => repo.GetPatientForPrescriptionAsync(1)).ReturnsAsync(patient1);
            _mockPrescriptionRepository.Setup(repo => repo.GetPatientForPrescriptionAsync(2)).ReturnsAsync(patient2);
            _mockPrescriptionRepository.Setup(repo => repo.GetExercisesForPrescriptionAsync(1)).ReturnsAsync(exercises1);
            _mockPrescriptionRepository.Setup(repo => repo.GetExercisesForPrescriptionAsync(2)).ReturnsAsync(exercises2);

            // Act
            var result = await _service.GetAllPrescriptionsAsync();

            // Assert
            var resultList = result.ToList();
            Assert.Equal(2, resultList.Count);

            // Verify first prescription
            Assert.Equal(1, resultList[0].Id);
            Assert.Equal("John Doe", resultList[0].PatientName);
            Assert.Single(resultList[0].Exercises);
            Assert.Equal("Exercise 1", resultList[0].Exercises[0].Name);

            // Verify second prescription
            Assert.Equal(2, resultList[1].Id);
            Assert.Equal("Jane Smith", resultList[1].PatientName);
            Assert.Equal(2, resultList[1].Exercises.Count);

            _mockPrescriptionRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllPrescriptionsAsync_WithEmptyRepository_ShouldReturnEmptyList()
        {
            // Arrange
            _mockPrescriptionRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Prescription>());

            // Act
            var result = await _service.GetAllPrescriptionsAsync();

            // Assert
            Assert.Empty(result);
            _mockPrescriptionRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mockPrescriptionRepository.Verify(repo => repo.GetPatientForPrescriptionAsync(It.IsAny<int>()), Times.Never);
            _mockPrescriptionRepository.Verify(repo => repo.GetExercisesForPrescriptionAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetPrescriptionByIdAsync_WithValidId_ShouldReturnPrescription()
        {
            // Arrange
            int prescriptionId = 1;
            var prescription = new Prescription
            {
                Id = prescriptionId,
                PatientId = 1,
                CreatedDate = DateTime.Now,
                Notes = "Test notes"
            };

            var patient = new Patient { Id = 1, Name = "John Doe", Email = "john@example.com" };

            var exercises = new List<Exercise>
            {
                new () { Id = 1, Name = "Exercise 1", Description = "Description 1", RepetitionCount = 10, Sets = 3 },
                new () { Id = 2, Name = "Exercise 2", Description = "Description 2", RepetitionCount = 15, Sets = 2 }
            };

            _mockPrescriptionRepository.Setup(repo => repo.GetByIdAsync(prescriptionId)).ReturnsAsync(prescription);
            _mockPrescriptionRepository.Setup(repo => repo.GetPatientForPrescriptionAsync(prescriptionId)).ReturnsAsync(patient);
            _mockPrescriptionRepository.Setup(repo => repo.GetExercisesForPrescriptionAsync(prescriptionId)).ReturnsAsync(exercises);

            // Act
            var result = await _service.GetPrescriptionByIdAsync(prescriptionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(prescriptionId, result.Id);
            Assert.Equal(patient.Name, result.PatientName);
            Assert.Equal(prescription.CreatedDate, result.CreatedDate);
            Assert.Equal(prescription.Notes, result.Notes);
            Assert.Equal(2, result.Exercises.Count);
        }

        [Fact]
        public async Task CreatePrescriptionAsync_WithValidData_ShouldCreateAndReturnPrescription()
        {
            // Arrange
            var createDto = new PrescriptionCreateDto
            {
                PatientId = 1,
                ExerciseIds = new List<int> { 1, 2 },
                Notes = "Test prescription notes"
            };

            var patient = new Patient { Id = 1, Name = "John Doe", Email = "john@example.com" };

            var createdPrescription = new Prescription
            {
                Id = 1,
                PatientId = createDto.PatientId,
                CreatedDate = DateTime.Now,
                Notes = createDto.Notes
            };

            var exercises = new List<Exercise>
            {
                new () { Id = 1, Name = "Exercise 1", Description = "Description 1", RepetitionCount = 10, Sets = 3 },
                new () { Id = 2, Name = "Exercise 2", Description = "Description 2", RepetitionCount = 15, Sets = 2 }
            };

            _mockPatientRepository.Setup(repo => repo.GetByIdAsync(createDto.PatientId)).ReturnsAsync(patient);
            _mockPrescriptionRepository.Setup(repo => repo.AddAsync(It.IsAny<Prescription>())).ReturnsAsync(createdPrescription);
            _mockPrescriptionRepository.Setup(repo => repo.AddExercisesToPrescriptionAsync(createdPrescription.Id, createDto.ExerciseIds)).Returns(Task.CompletedTask);
            _mockExerciseRepository.Setup(repo => repo.GetByIdsAsync(createDto.ExerciseIds)).ReturnsAsync(exercises);

            // Act
            var result = await _service.CreatePrescriptionAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createdPrescription.Id, result.Id);
            Assert.Equal(patient.Name, result.PatientName);
            Assert.Equal(createDto.Notes, result.Notes);
            Assert.Equal(2, result.Exercises.Count);
        }
        
        [Fact]
        public async Task CreatePrescriptionAsync_ShouldSetCreatedDateToCurrentTime()
        {
            // Arrange
            var createDto = new PrescriptionCreateDto
            {
                PatientId = 1,
                ExerciseIds = new List<int> { 1 },
                Notes = "Test notes"
            };

            var patient = new Patient { Id = 1, Name = "John Doe", Email = "john@example.com" };
            var exercise = new Exercise { Id = 1, Name = "Exercise 1", Description = "Desc", RepetitionCount = 10, Sets = 3 };

            Prescription capturedPrescription = null;

            _mockPatientRepository.Setup(repo => repo.GetByIdAsync(createDto.PatientId)).ReturnsAsync(patient);
            _mockPrescriptionRepository.Setup(repo => repo.AddAsync(It.IsAny<Prescription>())).Callback<Prescription>(p => capturedPrescription = p)
                .ReturnsAsync((Prescription p) => p);
            _mockPrescriptionRepository.Setup(repo => repo.AddExercisesToPrescriptionAsync(It.IsAny<int>(), It.IsAny<IEnumerable<int>>()))
                .Returns(Task.CompletedTask);
            _mockExerciseRepository.Setup(repo => repo.GetByIdsAsync(createDto.ExerciseIds)).ReturnsAsync(new List<Exercise> { exercise });

            var beforeDate = DateTime.Now.AddSeconds(-1);

            // Act
            await _service.CreatePrescriptionAsync(createDto);

            var afterDate = DateTime.Now.AddSeconds(1);

            // Assert
            Assert.NotNull(capturedPrescription);
            Assert.True(capturedPrescription.CreatedDate >= beforeDate);
            Assert.True(capturedPrescription.CreatedDate <= afterDate);
        }
    }
}