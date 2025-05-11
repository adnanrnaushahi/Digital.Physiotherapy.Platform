using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalPhysio.Domain.Models;

namespace DigitalPhysio.Domain.Interfaces
{
    public interface IPrescriptionRepository
    {
        Task<IEnumerable<Prescription>> GetAllAsync();
        Task<Prescription?> GetByIdAsync(int id);
        Task<IEnumerable<Exercise>> GetExercisesForPrescriptionAsync(int prescriptionId);
        Task<Patient?> GetPatientForPrescriptionAsync(int prescriptionId);
        Task<Prescription> AddAsync(Prescription prescription);
        Task AddExercisesToPrescriptionAsync(int prescriptionId, IEnumerable<int> exerciseIds);
        Task<IEnumerable<Prescription>> GetByPatientIdAsync(int patientId);
    }
}
