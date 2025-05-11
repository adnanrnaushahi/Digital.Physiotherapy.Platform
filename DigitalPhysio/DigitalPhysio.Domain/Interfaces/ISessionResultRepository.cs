using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalPhysio.Domain.Models;

namespace DigitalPhysio.Domain.Interfaces
{
    public interface ISessionResultRepository
    {
        Task<IEnumerable<SessionResult>> GetAllAsync();
        Task<SessionResult?> GetByIdAsync(int id);
        Task<SessionResult> AddAsync(SessionResult sessionResult);
        Task<IEnumerable<SessionResult>> GetByPrescriptionIdAsync(int prescriptionId);
    }
}
