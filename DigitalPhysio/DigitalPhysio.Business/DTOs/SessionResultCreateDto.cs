namespace DigitalPhysio.Business.DTOs
{
    public class SessionResultCreateDto
    {
        public int PrescriptionId { get; set; }
        public DateTime SessionDate { get; set; }
        public int CompletionPercentage { get; set; }
        public int AccuracyScore { get; set; }
        public string Notes { get; set; } = string.Empty;
        public Dictionary<string, int> ExerciseCompletion { get; set; } = new Dictionary<string, int>();
    }
}
