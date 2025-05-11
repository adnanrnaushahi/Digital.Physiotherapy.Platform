namespace DigitalPhysio.Business.DTOs
{
    public class PrescriptionCreateDto
    {
        public int PatientId { get; set; }
        public List<int> ExerciseIds { get; set; } = new List<int>();
        public string Notes { get; set; } = string.Empty;
    }
}
