using DigitalPhysio.Domain.Models;
using DigitalPhysio.Infrastructure.Data.DigitalPhysio.Infrastructure.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DigitalPhysio.Infrastructure
{
    namespace DigitalPhysio.Infrastructure.Data
    {
        public class DataSeeder
        {
            private readonly PhysioDbContext _context;
            private readonly IHostEnvironment _environment;
            private readonly ILogger<DataSeeder> _logger;

            public DataSeeder(PhysioDbContext context, IHostEnvironment environment, ILogger<DataSeeder> logger)
            {
                _context = context;
                _environment = environment;
                _logger = logger;
            }

            public async Task SeedDataAsync()
            {
                // Only seed if database is empty
                if (!_context.Patients.Any())
                {
                    _logger.LogInformation("Starting database seeding");

                    var dataPath = Path.Combine(_environment.ContentRootPath, "JsonFiles");

                    _logger.LogInformation("Reading JSON files from: {Path}", dataPath);

                    var jsonOptions = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    try
                    {
                        // Seed Patients
                        var patientsJson = await File.ReadAllTextAsync(Path.Combine(dataPath, "patients.json"));
                        var patients = JsonSerializer.Deserialize<List<Patient>>(patientsJson, jsonOptions);
                        if (patients != null)
                        {
                            await _context.Patients.AddRangeAsync(patients);
                        }

                        // Seed Exercises
                        var exercisesJson = await File.ReadAllTextAsync(Path.Combine(dataPath, "exercises.json"));
                        var exercises = JsonSerializer.Deserialize<List<Exercise>>(exercisesJson, jsonOptions);
                        if (exercises != null)
                        {
                            await _context.Exercises.AddRangeAsync(exercises);
                        }

                        // Save to generate IDs
                        await _context.SaveChangesAsync();

                        // Seed Prescriptions
                        var prescriptionsJson = await File.ReadAllTextAsync(Path.Combine(dataPath, "prescriptions.json"));
                        var prescriptionDtos = JsonSerializer.Deserialize<List<PrescriptionDto>>(prescriptionsJson, jsonOptions);

                        if (prescriptionDtos != null)
                        {
                            foreach (var dto in prescriptionDtos)
                            {
                                var prescription = new Prescription
                                {
                                    Id = dto.Id,
                                    PatientId = dto.PatientId,
                                    CreatedDate = dto.CreatedDate,
                                    Notes = dto.Notes
                                };

                                await _context.Prescriptions.AddAsync(prescription);

                                // Add prescription-exercise associations
                                foreach (var exerciseId in dto.ExerciseIds)
                                {
                                    var prescriptionExercise = new PrescriptionExercise
                                    {
                                        PrescriptionId = prescription.Id,
                                        ExerciseId = exerciseId
                                    };
                                    await _context.PrescriptionExercises.AddAsync(prescriptionExercise);
                                }
                            }
                        }

                        await _context.SaveChangesAsync();

                        // Seed Session Results
                        var resultsJson = await File.ReadAllTextAsync(Path.Combine(dataPath, "sessionResults.json"));
                        var resultDtos = JsonSerializer.Deserialize<List<SessionResultDto>>(resultsJson, jsonOptions);

                        if (resultDtos != null)
                        {
                            foreach (var dto in resultDtos)
                            {
                                var result = new SessionResult
                                {
                                    Id = dto.Id,
                                    PrescriptionId = dto.PrescriptionId,
                                    SessionDate = dto.SessionDate,
                                    Notes = dto.Notes
                                };

                                // Handle exercise completion
                                result.ExerciseCompletion = dto.ExerciseCompletion;

                                await _context.SessionResults.AddAsync(result);
                            }
                        }

                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error seeding data");
                    }
                }
            }
            private class PrescriptionDto
            {
                public int Id { get; set; }
                public int PatientId { get; set; }
                public DateTime CreatedDate { get; set; }
                public List<int> ExerciseIds { get; set; } = new List<int>();
                public string Notes { get; set; } = string.Empty;
            }

            private class SessionResultDto
            {
                public int Id { get; set; }
                public int PrescriptionId { get; set; }
                public DateTime SessionDate { get; set; }
                public string Notes { get; set; } = string.Empty;
                public Dictionary<string, int> ExerciseCompletion { get; set; } = new Dictionary<string, int>();
            }
        }
    }
}