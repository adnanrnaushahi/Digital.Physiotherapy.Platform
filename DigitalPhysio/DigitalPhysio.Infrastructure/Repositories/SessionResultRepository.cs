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
    public class SessionResultRepository : ISessionResultRepository
    {
        private readonly PhysioDbContext _context;

        public SessionResultRepository(PhysioDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SessionResult>> GetAllAsync()
        {
            return await _context.SessionResults
                .Include(r => r.Prescription)
                .ThenInclude(p => p.Patient)
                .ToListAsync();
        }

        public async Task<SessionResult?> GetByIdAsync(int id)
        {
            return await _context.SessionResults
                .Include(r => r.Prescription)
                .ThenInclude(p => p.Patient)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<SessionResult> AddAsync(SessionResult sessionResult)
        {
            _context.SessionResults.Add(sessionResult);
            await _context.SaveChangesAsync();
            return sessionResult;
        }

        public async Task<IEnumerable<SessionResult>> GetByPrescriptionIdAsync(int prescriptionId)
        {
            return await _context.SessionResults
                .Include(r => r.Prescription)
                .ThenInclude(p => p.Patient)
                .Where(r => r.PrescriptionId == prescriptionId)
                .ToListAsync();
        }
    }
}