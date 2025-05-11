using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalPhysio.Domain.Models;

namespace DigitalPhysio.Domain.Interfaces
{
    public interface IExerciseRepository
    {
        Task<IEnumerable<Exercise>> GetAllAsync();
        Task<Exercise?> GetByIdAsync(int id);
        Task<IEnumerable<Exercise>> GetByIdsAsync(IEnumerable<int> ids);
        Task<Exercise> AddAsync(Exercise exercise);
    }
}
