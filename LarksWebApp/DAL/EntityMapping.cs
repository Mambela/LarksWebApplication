using AutoMapper;
using LarksWebApp.DTOs;
using LarksWebApp.DTOs.DTO_Addresses;
using LarksWebApp.DTOs.DTO_Category;
using LarksWebApp.DTOs.DTO_City;
using LarksWebApp.DTOs.DTO_Districts;
using LarksWebApp.DTOs.DTO_Municipalities;
using LarksWebApp.DTOs.DTO_Neighbourhoods;
using LarksWebApp.DTOs.DTO_Provinces;
using LarksWebApp.DTOs.DTO_Streets;
using LarksWebApp.DTOs.DTO_Subcategory;
using LarksWebApp.Models;

namespace LarksWebApp.Data
{
    public class EntityMapping:Profile
    {

        public EntityMapping()
        {
            CreateMap<CountryModel,CountryDTO>().ReverseMap();
            CreateMap<ProvinceModel,Create_Province_DTO>().ReverseMap();
            CreateMap<MunicipalityModel,Create_Municipality_DTO>().ReverseMap();
            CreateMap<DistrictModel,Create_Districts_DTO>().ReverseMap();
            CreateMap<NeighbourhoodModel,Create_neighbourhood_DTO>().ReverseMap();
            CreateMap<StreetModel,Create_Street_DTO>().ReverseMap();
            CreateMap<AddressModel,Create_Addresses_DTO>().ReverseMap();
            CreateMap<CityModel,Create_City_DTO>().ReverseMap();
            CreateMap<CategoryModel, CreateCategoryDTO>().ReverseMap();
            CreateMap<CategoryModel, CategoryDTO>().ReverseMap();
            CreateMap<SubcategoryModel, Create_subcategoryDTO>().ReverseMap();

        }

    }
}
