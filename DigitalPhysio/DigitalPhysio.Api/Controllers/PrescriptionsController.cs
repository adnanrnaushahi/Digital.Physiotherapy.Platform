using DigitalPhysio.Business.DTOs;
using DigitalPhysio.Business.Interfaces;
using DigitalPhysio.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigitalPhysio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IPatientService _patientService;
        private readonly IExerciseService _exerciseService;

        public PrescriptionsController(
            IPrescriptionService prescriptionService,
            IPatientService patientService,
            IExerciseService exerciseService)
        {
            _prescriptionService = prescriptionService;
            _patientService = patientService;
            _exerciseService = exerciseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetAll()
        {
            var prescriptions = await _prescriptionService.GetAllPrescriptionsAsync();
            return Ok(prescriptions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PrescriptionDto>> Get(int id)
        {
            var prescription = await _prescriptionService.GetPrescriptionByIdAsync(id);

            if (prescription == null)
            {
                return NotFound();
            }

            return Ok(prescription);
        }

        [HttpPost]
        public async Task<ActionResult<PrescriptionDto>> Create(PrescriptionCreateDto createDto)
        {
            try
            {
                var prescription = await _prescriptionService.CreatePrescriptionAsync(createDto);
                return CreatedAtAction(nameof(Get), new { id = prescription.Id }, prescription);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("patients")]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatients()
        {
            var patients = await _patientService.GetAllPatientsAsync();
            return Ok(patients);
        }

        [HttpGet("exercises")]
        public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetExercises()
        {
            var exercises = await _exerciseService.GetAllExercisesAsync();
            return Ok(exercises);
        }
    }
}