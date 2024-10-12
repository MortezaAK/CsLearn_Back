using ApplicationCore.DTOs;
using ApplicationCore.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Utilitis
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        { 
            CreateMap<Categories , CategoryDTO>();
            CreateMap<CategoryDTO, Categories>();
        }
    }
}
