using AutoMapper;
using RolexWatches.Application.Dto;
using RolexWatches.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RolexWatches.Application.Mapper
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            // Outbound: entity -> DTO, always safe.
            CreateMap<User, UserDto>()
                .ForMember(d => d.Role, opt => opt.MapFrom(s => s.Role.ToString()));

            // Inbound: RegisterDto -> new User. This is only ever used with
            // mapper.Map<User>(dto) to CREATE a fresh entity, never
            // mapper.Map(dto, existingUser) — doing the latter later (e.g. for
            // a "update profile" feature) would blow away Id/PasswordHash/Role,
            // per the AutoMapper PUT pitfall from the other projects. If you add
            // an UpdateProfileDto later, ignore Id, PasswordHash, PasswordSalt,
            // Role and CreatedAt explicitly on that map.
            CreateMap<RegisterDto, User>()
                .ForMember(d => d.PasswordHash, opt => opt.Ignore())
                .ForMember(d => d.PasswordSalt, opt => opt.Ignore())
                .ForMember(d => d.Role, opt => opt.Ignore())
                .ForMember(d => d.Id, opt => opt.Ignore());
        }
    }
}
