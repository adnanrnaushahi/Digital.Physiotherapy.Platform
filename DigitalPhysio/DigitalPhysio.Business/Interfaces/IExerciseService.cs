using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalPhysio.Business.DTOs;

namespace DigitalPhysio.Business.Interfaces
{
    public interface IExerciseService
    {
        Task<IEnumerable<ExerciseDto>> GetAllExercisesAsync();
    }
}
