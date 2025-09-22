namespace Security.Application.Mapping;

using AutoMapper;
using DTOs;
using Domain.Entities;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>();
    }
}
