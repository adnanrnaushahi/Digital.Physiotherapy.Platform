using DigitalPhysio.Business.DTOs;
using DigitalPhysio.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalPhysio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        private readonly ISessionResultService _resultService;

        public ResultsController(ISessionResultService resultService)
        {
            _resultService = resultService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionResultDto>>> GetAll()
        {
            var results = await _resultService.GetAllResultsAsync();
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SessionResultDto>> Get(int id)
        {
            var result = await _resultService.GetResultByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("prescription/{prescriptionId}")]
        public async Task<ActionResult<IEnumerable<SessionResultDto>>> GetByPrescription(int prescriptionId)
        {
            var results = await _resultService.GetResultsByPrescriptionIdAsync(prescriptionId);
            return Ok(results);
        }

        [HttpPost]
        public async Task<ActionResult<SessionResultDto>> Create(SessionResultCreateDto createDto)
        {
            try
            {
                var result = await _resultService.CreateResultAsync(createDto);
                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
