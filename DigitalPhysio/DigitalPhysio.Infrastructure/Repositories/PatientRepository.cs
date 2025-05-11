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
    public class PatientRepository : IPatientRepository
    {
        private readonly PhysioDbContext _context;

        public PatientRepository(PhysioDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _context.Patients.ToListAsync();
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _context.Patients.FindAsync(id);
        }

        public async Task<Patient> AddAsync(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }
    }
}