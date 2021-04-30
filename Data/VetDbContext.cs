using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Vet.Models;

namespace Vet.Data
{
    public class VetDbContext : ApiAuthorizationDbContext<VetUser>
    {
        public VetDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<AnimalSpecies> AnimalSpecies { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<TreatmentTime> TreatmentTimes { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Therapia> Therapias { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<MedicalRecordPhoto> MedicalRecordPhotos { get; set; }
        public DbSet<TherapiaRecord> TherapiaRecords { get; set; }
        public DbSet<Vaccine> Vaccines { get; set; }
        public DbSet<VaccineRecord> VaccinationRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VetUser>().HasData(JsonDataLoader.LoadJson<VetUser>("./Data/Seed/VetUser"));
            modelBuilder.Entity<AnimalSpecies>().HasData(JsonDataLoader.LoadJson<AnimalSpecies>("./Data/Seed/AnimalSpecies"));
            modelBuilder.Entity<Animal>().HasData(JsonDataLoader.LoadJson<Animal>("./Data/Seed/Animal"));
            modelBuilder.Entity<Treatment>().HasData(JsonDataLoader.LoadJson<Treatment>("./Data/Seed/Treatment"));
            modelBuilder.Entity<TreatmentTime>().HasData(JsonDataLoader.LoadJson<TreatmentTime>("./Data/Seed/TreatmentTime"));
            modelBuilder.Entity<Appointment>().HasData(JsonDataLoader.LoadJson<Appointment>("./Data/Seed/Appointment"));
            modelBuilder.Entity<Holiday>().HasData(JsonDataLoader.LoadJson<Holiday>("./Data/Seed/Holiday"));
            modelBuilder.Entity<Vaccine>().HasData(JsonDataLoader.LoadJson<Vaccine>("./Data/Seed/Vaccine"));
            modelBuilder.Entity<VaccineRecord>().HasData(JsonDataLoader.LoadJson<VaccineRecord>("./Data/Seed/VaccineRecord"));
            modelBuilder.Entity<Therapia>().HasData(JsonDataLoader.LoadJson<Therapia>("./Data/Seed/Therapia"));
            modelBuilder.Entity<MedicalRecord>().HasData(JsonDataLoader.LoadJson<MedicalRecord>("./Data/Seed/MedicalRecord"));
            modelBuilder.Entity<TherapiaRecord>().HasData(JsonDataLoader.LoadJson<TherapiaRecord>("./Data/Seed/TherapiaRecord"));
            modelBuilder.Entity<MedicalRecordPhoto>().HasData(JsonDataLoader.LoadJson<MedicalRecordPhoto>("./Data/Seed/MedicalRecordPhoto"));
        }
    }
}


