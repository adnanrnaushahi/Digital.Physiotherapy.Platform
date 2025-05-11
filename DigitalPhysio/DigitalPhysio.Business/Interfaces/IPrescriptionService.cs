using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalPhysio.Business.DTOs;

namespace DigitalPhysio.Business.Interfaces
{
    public interface IPrescriptionService
    {
        Task<IEnumerable<PrescriptionDto>> GetAllPrescriptionsAsync();
        Task<PrescriptionDto?> GetPrescriptionByIdAsync(int id);
        Task<PrescriptionDto> CreatePrescriptionAsync(PrescriptionCreateDto createDto);
    }
}
