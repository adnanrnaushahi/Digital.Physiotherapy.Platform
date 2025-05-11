using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalPhysio.Domain.Interfaces;
using DigitalPhysio.Domain.Models;
using DigitalPhysio.Infrastructure.Data.DigitalPhysio.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DigitalPhysio.Infrastructure.Repositories
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly PhysioDbContext _context;

        public PrescriptionRepository(PhysioDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Prescription>> GetAllAsync()
        {
            return await _context.Prescriptions.ToListAsync();
        }

        public async Task<Prescription?> GetByIdAsync(int id)
        {
            return await _context.Prescriptions.FindAsync(id);
        }

        public async Task<IEnumerable<Exercise>> GetExercisesForPrescriptionAsync(int prescriptionId)
        {
            var exerciseIds = await _context.PrescriptionExercises
                .Where(pe => pe.PrescriptionId == prescriptionId)
                .Select(pe => pe.ExerciseId)
                .ToListAsync();

            return await _context.Exercises
                .Where(e => exerciseIds.Contains(e.Id))
                .ToListAsync();
        }

        public async Task<Patient?> GetPatientForPrescriptionAsync(int prescriptionId)
        {
            var prescription = await _context.Prescriptions.FindAsync(prescriptionId);
            if (prescription == null)
            {
                return null;
            }

            return await _context.Patients.FindAsync(prescription.PatientId);
        }

        public async Task<Prescription> AddAsync(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
            return prescription;
        }

        public async Task AddExercisesToPrescriptionAsync(int prescriptionId, IEnumerable<int> exerciseIds)
        {
            foreach (var exerciseId in exerciseIds)
            {
                var prescriptionExercise = new PrescriptionExercise
                {
                    PrescriptionId = prescriptionId,
                    ExerciseId = exerciseId
                };

                _context.PrescriptionExercises.Add(prescriptionExercise);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Prescription>> GetByPatientIdAsync(int patientId)
        {
            return await _context.Prescriptions
                .Where(p => p.PatientId == patientId)
                .ToListAsync();
        }
    }
}
