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
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly PhysioDbContext _context;

        public ExerciseRepository(PhysioDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Exercise>> GetAllAsync()
        {
            return await _context.Exercises.ToListAsync();
        }

        public async Task<Exercise?> GetByIdAsync(int id)
        {
            return await _context.Exercises.FindAsync(id);
        }

        public async Task<IEnumerable<Exercise>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.Exercises
                .Where(e => ids.Contains(e.Id))
                .ToListAsync();
        }

        public async Task<Exercise> AddAsync(Exercise exercise)
        {
            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();
            return exercise;
        }
    }
}