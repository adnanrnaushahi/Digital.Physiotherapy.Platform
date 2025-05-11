using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalPhysio.Business.DTOs;
using DigitalPhysio.Business.Interfaces;
using DigitalPhysio.Domain.Interfaces;
using DigitalPhysio.Domain.Models;
using Microsoft.Extensions.Logging;

namespace DigitalPhysio.Business.Services
{
    public class SessionResultService : ISessionResultService
    {
        private readonly ISessionResultRepository _resultRepository;
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly ILogger<ExerciseService> _logger;

        public SessionResultService(ISessionResultRepository resultRepository, IPrescriptionRepository prescriptionRepository, ILogger<ExerciseService> logger)
        {
            _resultRepository = resultRepository;
            _prescriptionRepository = prescriptionRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<SessionResultDto>> GetAllResultsAsync()
        {
            try
            {
                var results = await _resultRepository.GetAllAsync();
                var resultDtos = new List<SessionResultDto>();

                foreach (var result in results)
                {
                    var patient = await _prescriptionRepository.GetPatientForPrescriptionAsync(result.PrescriptionId);

                    resultDtos.Add(new SessionResultDto
                    {
                        Id = result.Id,
                        PrescriptionId = result.PrescriptionId,
                        PatientName = patient?.Name ?? "Unknown",
                        SessionDate = result.SessionDate,
                        Notes = result.Notes,
                        ExerciseCompletion = result.ExerciseCompletion
                    });
                }

                return resultDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching session results.");
                throw;
            }            
        }

        public async Task<SessionResultDto?> GetResultByIdAsync(int id)
        {

            if (id <= 0)
            {
                throw new ArgumentException("Invalid session result ID");
            }

            try
            {
                var result = await _resultRepository.GetByIdAsync(id);

                if (result == null)
                {
                    return null;
                }

                var patient = await _prescriptionRepository.GetPatientForPrescriptionAsync(result.PrescriptionId);

                return new SessionResultDto
                {
                    Id = result.Id,
                    PrescriptionId = result.PrescriptionId,
                    PatientName = patient?.Name ?? "Unknown",
                    SessionDate = result.SessionDate,
                    Notes = result.Notes,
                    ExerciseCompletion = result.ExerciseCompletion
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while patient for prescription results.");
                throw;
            }
            
        }

        public async Task<IEnumerable<SessionResultDto>> GetResultsByPrescriptionIdAsync(int prescriptionId)
        {
            try
            {
                if (prescriptionId <= 0)
                {
                    throw new ArgumentException("Invalid prescription ID");
                }

                var results = await _resultRepository.GetByPrescriptionIdAsync(prescriptionId);
                var patient = await _prescriptionRepository.GetPatientForPrescriptionAsync(prescriptionId);

                return results.Select(r => new SessionResultDto
                {
                    Id = r.Id,
                    PrescriptionId = r.PrescriptionId,
                    PatientName = patient?.Name ?? "Unknown",
                    SessionDate = r.SessionDate,
                    Notes = r.Notes,
                    ExerciseCompletion = r.ExerciseCompletion
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching session results by prescription ID.");
                throw;
            }                
        }

        public async Task<SessionResultDto> CreateResultAsync(SessionResultCreateDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    throw new ArgumentNullException(nameof(createDto), "Session result data cannot be null");
                }
                var prescription = await _prescriptionRepository.GetByIdAsync(createDto.PrescriptionId);
                if (prescription == null)
                {
                    throw new ArgumentException($"Prescription with ID {createDto.PrescriptionId} not found");
                }

                var result = new SessionResult
                {
                    PrescriptionId = createDto.PrescriptionId,
                    SessionDate = createDto.SessionDate,
                    Notes = createDto.Notes,
                    ExerciseCompletion = createDto.ExerciseCompletion
                };

                var createdResult = await _resultRepository.AddAsync(result);
                var patient = await _prescriptionRepository.GetPatientForPrescriptionAsync(createdResult.PrescriptionId);

                return new SessionResultDto
                {
                    Id = createdResult.Id,
                    PrescriptionId = createdResult.PrescriptionId,
                    PatientName = patient?.Name ?? "Unknown",
                    SessionDate = createdResult.SessionDate,
                    Notes = createdResult.Notes,
                    ExerciseCompletion = createdResult.ExerciseCompletion
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while validating session result data.");
                throw;
            }            
        }
    }
}