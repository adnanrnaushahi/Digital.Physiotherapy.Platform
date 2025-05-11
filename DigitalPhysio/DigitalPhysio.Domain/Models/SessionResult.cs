using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DigitalPhysio.Domain.Models
{
    public class SessionResult
    {
        public int Id { get; set; }
        public int PrescriptionId { get; set; }
        public Prescription Prescription { get; set; } = null!;
        public DateTime SessionDate { get; set; }
        public int CompletionPercentage { get; set; }
        public int AccuracyScore { get; set; }
        public string Notes { get; set; } = string.Empty;

        // For database storage
        public string ExerciseCompletionJson { get; set; } = string.Empty;

        // Helper property (not mapped to database)
        public Dictionary<string, int> ExerciseCompletion
        {
            get => string.IsNullOrEmpty(ExerciseCompletionJson)
                ? new Dictionary<string, int>()
                : JsonSerializer.Deserialize<Dictionary<string, int>>(ExerciseCompletionJson) ?? new Dictionary<string, int>();
            set => ExerciseCompletionJson = JsonSerializer.Serialize(value);
        }
    }
}
