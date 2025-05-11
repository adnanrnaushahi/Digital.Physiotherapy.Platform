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
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly ILogger<PrescriptionService> _logger;

        public PrescriptionService(IPrescriptionRepository prescriptionRepository, IPatientRepository patientRepository, IExerciseRepository exerciseRepository, ILogger<PrescriptionService> logger)
        {
            _prescriptionRepository = prescriptionRepository;
            _patientRepository = patientRepository;
            _exerciseRepository = exerciseRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<PrescriptionDto>> GetAllPrescriptionsAsync()
        {
            try
            {
                var prescriptions = await _prescriptionRepository.GetAllAsync();
                var prescriptionDtos = new List<PrescriptionDto>();

                foreach (var prescription in prescriptions)
                {
                    var patient = await _prescriptionRepository.GetPatientForPrescriptionAsync(prescription.Id);
                    var exercises = await _prescriptionRepository.GetExercisesForPrescriptionAsync(prescription.Id);

                    prescriptionDtos.Add(new PrescriptionDto
                    {
                        Id = prescription.Id,
                        PatientId = prescription.PatientId,
                        PatientName = patient?.Name ?? "Unknown",
                        CreatedDate = prescription.CreatedDate,
                        Notes = prescription.Notes,
                        Exercises = exercises.Select(e => new ExerciseDto
                        {
                            Id = e.Id,
                            Name = e.Name,
                            Description = e.Description,
                            RepetitionCount = e.RepetitionCount,
                            Sets = e.Sets
                        }).ToList()
                    });
                }

                return prescriptionDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all prescriptions.");
                throw;
            }

        }

        public async Task<PrescriptionDto?> GetPrescriptionByIdAsync(int id)
        {
            try
            {
                var prescription = await _prescriptionRepository.GetByIdAsync(id);

                if (prescription == null)
                {
                    return null;
                }

                var patient = await _prescriptionRepository.GetPatientForPrescriptionAsync(prescription.Id);
                var exercises = await _prescriptionRepository.GetExercisesForPrescriptionAsync(prescription.Id);

                return new PrescriptionDto
                {
                    Id = prescription.Id,
                    PatientId = prescription.PatientId,
                    PatientName = patient?.Name ?? "Unknown",
                    CreatedDate = prescription.CreatedDate,
                    Notes = prescription.Notes,
                    Exercises = exercises.Select(e => new ExerciseDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Description = e.Description,
                        RepetitionCount = e.RepetitionCount,
                        Sets = e.Sets
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching prescription with ID: {Id}", id);
                throw;
            }

        }

        public async Task<PrescriptionDto> CreatePrescriptionAsync(PrescriptionCreateDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    throw new ArgumentNullException(nameof(createDto), "Prescription create DTO cannot be null");
                }

                var patient = await _patientRepository.GetByIdAsync(createDto.PatientId);
                if (patient == null)
                {
                    throw new ArgumentException($"Patient with ID {createDto.PatientId} not found");
                }

                var prescription = new Prescription
                {
                    PatientId = createDto.PatientId,
                    CreatedDate = DateTime.Now,
                    Notes = createDto.Notes
                };

                var createdPrescription = await _prescriptionRepository.AddAsync(prescription);

                // Add exercises as separate relationships
                await _prescriptionRepository.AddExercisesToPrescriptionAsync(createdPrescription.Id, createDto.ExerciseIds);

                // Get the exercises to return in the DTO
                var exercises = await _exerciseRepository.GetByIdsAsync(createDto.ExerciseIds);

                return new PrescriptionDto
                {
                    Id = createdPrescription.Id,
                    PatientId = createdPrescription.PatientId,
                    PatientName = patient.Name,
                    CreatedDate = createdPrescription.CreatedDate,
                    Notes = createdPrescription.Notes,
                    Exercises = exercises.Select(e => new ExerciseDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Description = e.Description,
                        RepetitionCount = e.RepetitionCount,
                        Sets = e.Sets
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a prescription.");
                throw;
            }
        }
    }
}