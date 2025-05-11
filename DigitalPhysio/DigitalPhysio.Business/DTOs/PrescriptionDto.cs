namespace DigitalPhysio.Business.DTOs
{
    public class PrescriptionDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public List<ExerciseDto> Exercises { get; set; } = new List<ExerciseDto>();
        public string Notes { get; set; } = string.Empty;
    }
}
