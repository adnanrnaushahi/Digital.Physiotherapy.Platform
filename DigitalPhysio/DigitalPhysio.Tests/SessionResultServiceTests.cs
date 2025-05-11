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
    public class SessionResultServiceTests
    {
        private readonly Mock<ISessionResultRepository> _mockResultRepository;
        private readonly Mock<IPrescriptionRepository> _mockPrescriptionRepository;
        private readonly Mock<ILogger<ExerciseService>> _mockLogger;
        private readonly SessionResultService _service;

        public SessionResultServiceTests()
        {
            _mockResultRepository = new Mock<ISessionResultRepository>();
            _mockPrescriptionRepository = new Mock<IPrescriptionRepository>();
            _mockLogger = new Mock<ILogger<ExerciseService>>();

            _service = new SessionResultService(_mockResultRepository.Object, _mockPrescriptionRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllResultsAsync_ShouldReturnAllResults()
        {
            // Arrange
            var sessionResults = new List<SessionResult>
            {
                new ()
                {
                    Id = 1,
                    PrescriptionId = 1,
                    SessionDate = DateTime.Now.AddDays(-2),
                    Notes = "First session notes",
                    ExerciseCompletion = new Dictionary<string, int> { {"Exercise 1", 90}, {"Exercise 2", 80} }
                },
                new ()
                {
                    Id = 2,
                    PrescriptionId = 2,
                    SessionDate = DateTime.Now.AddDays(-1),
                    Notes = "Second session notes",
                    ExerciseCompletion = new Dictionary<string, int> { {"Exercise 3", 95}, {"Exercise 4", 89} }
                }
            };

            var patient1 = new Patient { Id = 1, Name = "John Doe" };
            var patient2 = new Patient { Id = 2, Name = "Jane Smith" };

            _mockResultRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(sessionResults);
            _mockPrescriptionRepository.Setup(repo => repo.GetPatientForPrescriptionAsync(1)).ReturnsAsync(patient1);
            _mockPrescriptionRepository.Setup(repo => repo.GetPatientForPrescriptionAsync(2)).ReturnsAsync(patient2);

            // Act
            var results = await _service.GetAllResultsAsync();

            // Assert
            var resultList = results.ToList();
            Assert.Equal(2, resultList.Count);

            Assert.Equal(1, resultList[0].Id);
            Assert.Equal("John Doe", resultList[0].PatientName);

            Assert.Equal(2, resultList[1].Id);
            Assert.Equal("Jane Smith", resultList[1].PatientName);

            _mockResultRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mockPrescriptionRepository.Verify(repo => repo.GetPatientForPrescriptionAsync(It.IsAny<int>()), Times.Exactly(2));
        }

        [Fact]
        public async Task GetAllResultsAsync_WithNoResults_ShouldReturnEmptyList()
        {
            // Arrange
            _mockResultRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<SessionResult>());

            // Act
            var results = await _service.GetAllResultsAsync();

            // Assert
            Assert.Empty(results);
            _mockResultRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mockPrescriptionRepository.Verify(repo => repo.GetPatientForPrescriptionAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetResultByIdAsync_WithValidId_ShouldReturnResult()
        {
            // Arrange
            int resultId = 1;
            var sessionResult = new SessionResult
            {
                Id = resultId,
                PrescriptionId = 1,
                SessionDate = DateTime.Now,
                Notes = "Session notes",
                ExerciseCompletion = new Dictionary<string, int> { { "Exercise 1", 90 }, { "Exercise 2", 80 } }
            };

            var patient = new Patient { Id = 1, Name = "John Doe" };

            _mockResultRepository.Setup(repo => repo.GetByIdAsync(resultId))
                .ReturnsAsync(sessionResult);

            _mockPrescriptionRepository.Setup(repo => repo.GetPatientForPrescriptionAsync(1))
                .ReturnsAsync(patient);

            // Act
            var result = await _service.GetResultByIdAsync(resultId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(resultId, result.Id);
            Assert.Equal("John Doe", result.PatientName);
            Assert.Equal(2, result.ExerciseCompletion.Count);

            _mockResultRepository.Verify(repo => repo.GetByIdAsync(resultId), Times.Once);
            _mockPrescriptionRepository.Verify(repo => repo.GetPatientForPrescriptionAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetResultByIdAsync_WithInvalidId_ShouldThrowArgumentException()
        {
            // Arrange
            int invalidId = 0;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.GetResultByIdAsync(invalidId));
            _mockResultRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetResultByIdAsync_WithNonExistentId_ShouldReturnNull()
        {
            // Arrange
            int nonExistentId = 999;

            _mockResultRepository.Setup(repo => repo.GetByIdAsync(nonExistentId))
                .ReturnsAsync((SessionResult)null);

            // Act
            var result = await _service.GetResultByIdAsync(nonExistentId);

            // Assert
            Assert.Null(result);
            _mockResultRepository.Verify(repo => repo.GetByIdAsync(nonExistentId), Times.Once);
            _mockPrescriptionRepository.Verify(repo => repo.GetPatientForPrescriptionAsync(It.IsAny<int>()), Times.Never);
        }

    }
}