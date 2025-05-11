using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalPhysio.Business.DTOs;
using DigitalPhysio.Business.Interfaces;
using DigitalPhysio.Business.Services;
using DigitalPhysio.Domain.Interfaces;
using DigitalPhysio.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DigitalPhysio.Tests.Business.Services
{
    public class ExerciseServiceTests
    {
        private readonly Mock<IExerciseRepository> _mockRepository;
        private readonly Mock<ILogger<ExerciseService>> _mockLogger;
        private readonly ExerciseService _service;

        public ExerciseServiceTests()
        {
            _mockRepository = new Mock<IExerciseRepository>();
            _mockLogger = new Mock<ILogger<ExerciseService>>();
            _service = new ExerciseService(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllExercisesAsync_ShouldReturnAllExercises_WhenExercisesExist()
        {
            // Arrange
            var exerciseList = new List<Exercise>
            {
                new () { Id = 1, Name = "Exercise 1", Description = "Description 1", RepetitionCount = 10, Sets = 3 },
                new () { Id = 2, Name = "Exercise 2", Description = "Description 2", RepetitionCount = 15, Sets = 2 },
                new () { Id = 3, Name = "Exercise 3", Description = "Description 3", RepetitionCount = 8, Sets = 4 }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(exerciseList);

            // Act
            var result = await _service.GetAllExercisesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(exerciseList.Count, result.Count());

            var resultList = result.ToList();
            for (int i = 0; i < exerciseList.Count; i++)
            {
                Assert.Equal(exerciseList[i].Id, resultList[i].Id);
                Assert.Equal(exerciseList[i].Name, resultList[i].Name);
                Assert.Equal(exerciseList[i].Description, resultList[i].Description);
                Assert.Equal(exerciseList[i].RepetitionCount, resultList[i].RepetitionCount);
                Assert.Equal(exerciseList[i].Sets, resultList[i].Sets);
            }

            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllExercisesAsync_ShouldReturnEmptyList_WhenNoExercisesExist()
        {
            // Arrange
            var emptyList = new List<Exercise>();

            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(emptyList);

            // Act
            var result = await _service.GetAllExercisesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllExercisesAsync_ShouldLogErrorAndThrow_WhenRepositoryThrowsException()
        {
            // Arrange
            var expectedException = new Exception("Database connection error");

            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.GetAllExercisesAsync());
            Assert.Same(expectedException, exception);
        }

        [Fact]
        public async Task GetAllExercisesAsync_ShouldMapAllProperties_WhenExercisesExist()
        {
            // Arrange
            var testExercise = new Exercise
            {
                Id = 42,
                Name = "Test Exercise",
                Description = "Test Description",
                RepetitionCount = 12,
                Sets = 3
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Exercise> { testExercise });

            // Act
            var result = await _service.GetAllExercisesAsync();

            // Assert
            var resultDto = result.Single();
            Assert.Equal(testExercise.Id, resultDto.Id);
            Assert.Equal(testExercise.Name, resultDto.Name);
            Assert.Equal(testExercise.Description, resultDto.Description);
            Assert.Equal(testExercise.RepetitionCount, resultDto.RepetitionCount);
            Assert.Equal(testExercise.Sets, resultDto.Sets);
        }
    }
}