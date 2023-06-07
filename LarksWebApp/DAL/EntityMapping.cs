using AutoMapper;
using LarksWebApp.DTOs;
using LarksWebApp.DTOs.DTO_Addresses;
using LarksWebApp.DTOs.DTO_Category;
using LarksWebApp.DTOs.DTO_City;
using LarksWebApp.DTOs.DTO_Districts;
using LarksWebApp.DTOs.DTO_Location;
using LarksWebApp.DTOs.DTO_Municipalities;
using LarksWebApp.DTOs.DTO_Neighbourhoods;
using LarksWebApp.DTOs.DTO_Provinces;
using LarksWebApp.DTOs.DTO_Streets;
using LarksWebApp.DTOs.DTO_Subcategory;
using LarksWebApp.DTOs.DTO_Tokens;
using LarksWebApp.Models;
using LarksWebApp.Models.CoordinatesModel;
using LarksWebApp.Models.Users;

namespace LarksWebApp.DAL
{
    public class EntityMapping : Profile
    {

        public EntityMapping()
        {
            CreateMap<CountryModel, CountryDTO>().ReverseMap();
            CreateMap<ProvinceModel, Create_Province_DTO>().ReverseMap();
            CreateMap<MunicipalityModel, Create_Municipality_DTO>().ReverseMap();
            CreateMap<DistrictModel, Create_Districts_DTO>().ReverseMap();
            CreateMap<SuburbModel, Create_suburb_DTO>().ReverseMap();
            CreateMap<StreetModel, Create_Street_DTO>().ReverseMap();
            CreateMap<AddressModel, Create_Addresses_DTO>().ReverseMap();
            CreateMap<CityModel, Create_City_DTO>().ReverseMap();
            CreateMap<CategoryModel, CreateCategoryDTO>().ReverseMap();
            CreateMap<CategoryModel, CategoryDTO>().ReverseMap();
            CreateMap<SubcategoryModel, Create_subcategoryDTO>().ReverseMap();
            CreateMap<SubCategoryDTO, SubcategoryModel>().ReverseMap();
            CreateMap<CityModel, CitiesDTO>().ReverseMap();
            CreateMap<DistrictModel, DistrictDTO>().ReverseMap();
            CreateMap<MunicipalityModel, MunicipalityDTO>().ReverseMap();
            CreateMap<ProvinceModel, ProvinceDTO>().ReverseMap();
            CreateMap<SuburbModel, NeighbourhoodDTO>().ReverseMap();
            CreateMap<StreetModel, View_Street_DTO>().ReverseMap();
            CreateMap<BuildingTypeModel, Create_BuildingType_DTO>().ReverseMap();
            CreateMap<StreetCoordinates, BaseCoordinatesDTO>().ReverseMap();
            CreateMap<LocationModel, LocationDTO>().ReverseMap();
            CreateMap<HooksModel, HooksDTO>().ReverseMap();
            CreateMap<AuthToken, AuthTokenDTO>().ReverseMap();
            CreateMap<ViewLocationDTO, LocationModel>().ReverseMap();
            CreateMap<ViewLocationDTO, LocationObj>().ReverseMap();
            CreateMap<ViewCountryDTO,CountryModel>().ReverseMap();
            CreateMap<ViewProvinceDTO,ProvinceModel>().ReverseMap();
            
       
        }

    }
}
