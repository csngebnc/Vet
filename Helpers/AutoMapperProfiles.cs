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
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.RealName));
            CreateMap<AddAnimalDto, Animal>();
            CreateMap<UpdateAnimalDto, Animal>();
            CreateMap<VetUser, VetUserDto>();
            CreateMap<AnimalSpecies, AnimalSpeciesDto>();
            CreateMap<AnimalSpeciesDto, AnimalSpecies>();
        }
    }
}
