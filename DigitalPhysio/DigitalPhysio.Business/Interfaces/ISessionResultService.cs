using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalPhysio.Business.DTOs;

namespace DigitalPhysio.Business.Interfaces
{
    public interface ISessionResultService
    {
        Task<IEnumerable<SessionResultDto>> GetAllResultsAsync();
        Task<SessionResultDto?> GetResultByIdAsync(int id);
        Task<IEnumerable<SessionResultDto>> GetResultsByPrescriptionIdAsync(int prescriptionId);
        Task<SessionResultDto> CreateResultAsync(SessionResultCreateDto createDto);
    }
}
