using System.Collections.Generic;
using System.Reflection.Emit;
using DigitalPhysio.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalPhysio.Infrastructure.Data
{
    namespace DigitalPhysio.Infrastructure.Data
    {
        public class PhysioDbContext : DbContext
        {
            public PhysioDbContext(DbContextOptions<PhysioDbContext> options) : base(options)
            {
            }

            public DbSet<Patient> Patients { get; set; } = null!;
            public DbSet<Exercise> Exercises { get; set; } = null!;
            public DbSet<Prescription> Prescriptions { get; set; } = null!;
            public DbSet<PrescriptionExercise> PrescriptionExercises { get; set; } = null!;
            public DbSet<SessionResult> SessionResults { get; set; } = null!;

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Configure PrescriptionExercise
                modelBuilder.Entity<PrescriptionExercise>()
                    .HasKey(pe => new { pe.PrescriptionId, pe.ExerciseId });

                // Ignore the non-mapped property
                modelBuilder.Entity<SessionResult>()
                    .Ignore(s => s.ExerciseCompletion);
            }
        }
    }
}