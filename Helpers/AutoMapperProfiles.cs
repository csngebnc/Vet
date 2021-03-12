using AutoMapper;
using Vet.Extensions;
using Vet.Models;
using Vet.Models.DTOs;

namespace Vet.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Animal, AnimalDto>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()))
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.RealName))
                .ForMember(dest => dest.SpeciesName, opt => opt.MapFrom(src => src.Species.Name));
            CreateMap<AddAnimalDto, Animal>();
            CreateMap<UpdateAnimalDto, Animal>();
            CreateMap<VetUser, VetUserDto>();
            CreateMap<AnimalSpecies, AnimalSpeciesDto>();
            CreateMap<AnimalSpeciesDto, AnimalSpecies>();

            CreateMap<AddTreatmentDto, Treatment>();
            CreateMap<UpdateTreatmentDto, Treatment>();
            CreateMap<Treatment, TreatmentDto>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.RealName));

            CreateMap<AddTreatmentTimeDto, TreatmentTime>();
            CreateMap<UpdateTreatmentTimeDto, TreatmentTime>();
            CreateMap<TreatmentTime, TreatmentTimeDto>();

            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.TreatmentName, opt => opt.MapFrom(src => src.Treatment.Name))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.RealName))
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.RealName))
                .ForMember(dest => dest.AnimalName, opt => opt.MapFrom(src => src.Animal.Name));
            CreateMap<Appointment, AppointmentTimeDto>();
            CreateMap<AddAppointmentByUserDto, Appointment>();

            CreateMap<AddHolidayDto, Holiday>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToLocalTime()))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToLocalTime()));
            CreateMap<Holiday, HolidayDto>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToLocalTime()))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToLocalTime()));
            CreateMap<HolidayDto, Holiday>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToLocalTime()))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToLocalTime()));


        }
    }
}
