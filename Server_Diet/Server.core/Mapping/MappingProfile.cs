using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.Models;
using Core.Resource;

namespace Core.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {
            CreateMap<User, UserResource>();
            CreateMap<UserResource, User>();
            CreateMap<Food, FoodResource>().ReverseMap();
            CreateMap<UserMeal, UserMealResource>().ReverseMap();
        }
     

    }
}
