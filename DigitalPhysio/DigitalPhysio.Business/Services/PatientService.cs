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
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly ILogger<PatientService> _logger;

        public PatientService(IPatientRepository patientRepository, ILogger<PatientService> logger)
        {
            _patientRepository = patientRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            try
            {
                var patients = await _patientRepository.GetAllAsync();

                return patients.Select(p => new PatientDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Email = p.Email
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all patients");
                throw;
            }
        }

        public async Task<PatientDto?> GetPatientByIdAsync(int id)
        {
            try
            {
                var patient = await _patientRepository.GetByIdAsync(id);

                if (patient == null)
                {
                    _logger.LogWarning("Patient with ID {PatientId} not found", id);
                    return null;
                }

                return new PatientDto
                {
                    Id = patient.Id,
                    Name = patient.Name,
                    Email = patient.Email
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patient with ID {PatientId}", id);
                throw;
            }
        }
    }
}
