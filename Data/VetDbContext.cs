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

        public DbSet<VetUser> Users { get; set; }
        public DbSet<AnimalSpecies> AnimalSpecies { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<TreatmentTime> TreatmentTimes { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
    }
}
