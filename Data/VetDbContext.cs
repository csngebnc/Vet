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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("Host=localhost;Database=Vet;Username=postgres;Password=postgres");
        }
    }
}
