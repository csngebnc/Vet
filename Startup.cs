using AutoMapper;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vet.BL;
using Vet.BL.Exceptions;
using Vet.BL.ProblemDetails;
using Vet.BL.ProblemDetails.Exceptions;
using Vet.Controllers;
using Vet.Data;
using Vet.Data.Repositories;
using Vet.Helpers;
using Vet.Interfaces;
using Vet.Models;

namespace Vet
{
    public class Startup
    {
        private string _corsPolicy = "DevCorsPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(_corsPolicy, builder => builder
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (ctx, ex) => false;
                options.Map<EntityNotFoundException>(
                  (ctx, ex) =>
                  {
                      var pd = StatusCodeProblemDetails.Create(StatusCodes.Status404NotFound);
                      pd.Title = ex.Message;
                      return pd;
                  }
                );

                options.Map<ExistingEntityException>(
                  (ctx, ex) =>
                  {
                      var pd = StatusCodeProblemDetails.Create(StatusCodes.Status400BadRequest);
                      pd.Title = ex.Message;
                      return pd;
                  }
                );

                options.Map<PermissionDeniedException>(
                  (ctx, ex) =>
                  {
                      var pd = StatusCodeProblemDetails.Create(StatusCodes.Status401Unauthorized);
                      pd.Title = ex.Message;
                      return pd;
                  }
                );

                options.Map<DataErrorException>(
                  (ctx, ex) =>
                  {
                      var pd = new DataErrorProblemDetails();
                      pd.Status = 400;
                      pd.Title = ex.Message;
                      pd.Errors = ex.Errors;
                      pd.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                      return pd;
                  }
                );
            });

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAnimalRepository, AnimalRepository>();
            services.AddScoped<ISpeciesRepository, SpeciesRepository>();
            services.AddScoped<ITreatmentRepository, TreatmentRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<ITherapiaRepository, TherapiaRepository>();
            services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
            services.AddScoped<IVaccineRepository, VaccineRepository>();
            services.AddScoped<IPhotoManager, PhotoManager>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            services.AddTransient<AnimalManager>();
            services.AddTransient<SpeciesManager>();
            services.AddTransient<TreatmentManager>();
            services.AddTransient<AppointmentManager>();
            services.AddTransient<DoctorManager>();
            services.AddTransient<TherapiaManager>();
            services.AddTransient<MedicalRecordManager>();
            services.AddTransient<VaccineManager>();
            services.AddTransient<UsersManager>();

            services.AddTransient<PdfManager>();

            services.AddDbContext<VetDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<VetUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<VetDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<VetUser, VetDbContext>();


            services.AddAuthentication()
                .AddIdentityServerJwt();
            services.AddControllersWithViews();
            services.AddRazorPages();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors(_corsPolicy);
                //app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseProblemDetails();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start");
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
