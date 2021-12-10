using AutoMapper;
using Core.Entities.Concrete;
using Entities.Dtos;

namespace Business.Mapping
{
    public class CustomMapping : Profile
    {
        public CustomMapping()
        {
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<Category, CategoryCreateViewModel>().ReverseMap();
            CreateMap<Category, CategoryAndTasksViewModel>().ReverseMap();

            CreateMap<CategoryTaskCreateViewModel, CategoryTask>().ReverseMap();
            CreateMap<CategoryTaskViewModel, CategoryTask>().ReverseMap();

            CreateMap<User, UserViewModel>().ReverseMap();
        }
    }
}
