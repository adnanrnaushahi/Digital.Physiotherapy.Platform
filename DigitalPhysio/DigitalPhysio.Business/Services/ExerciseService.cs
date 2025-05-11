using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalPhysio.Business.DTOs;
using DigitalPhysio.Business.Interfaces;
using DigitalPhysio.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace DigitalPhysio.Business.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly ILogger<ExerciseService> _logger;

        public ExerciseService(IExerciseRepository exerciseRepository, ILogger<ExerciseService> logger)
        {
            _exerciseRepository = exerciseRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ExerciseDto>> GetAllExercisesAsync()
        {
            try
            {
                var exercises = await _exerciseRepository.GetAllAsync();

                return exercises.Select(e => new ExerciseDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    RepetitionCount = e.RepetitionCount,
                    Sets = e.Sets
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching exercises.");
                throw;
            }
            
        }
    }
}    
