using AutoMapper;
using LarksWebApp.DTOs;
using LarksWebApp.DTOs.DTO_Addresses;
using LarksWebApp.DTOs.DTO_Category;
using LarksWebApp.DTOs.DTO_City;
using LarksWebApp.DTOs.DTO_Districts;
using LarksWebApp.DTOs.DTO_Location;
using LarksWebApp.DTOs.DTO_Municipalities;
using LarksWebApp.DTOs.DTO_Suburbs;
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
            CreateMap<SuburbModel, CreateSuburbDTO>().ReverseMap();
            CreateMap<StreetModel, Create_Street_DTO>().ReverseMap();
            CreateMap<AddressModel, Create_Addresses_DTO>().ReverseMap();
            CreateMap<AddressModel, CreateAddressDTO>().ReverseMap();
            CreateMap<CityModel, Create_City_DTO>().ReverseMap();
            CreateMap<CategoryModel, CreateCategoryDTO>().ReverseMap();
            CreateMap<CategoryModel, CategoryDTO>().ReverseMap();
            CreateMap<SubcategoryModel, Create_subcategoryDTO>().ReverseMap();
            CreateMap<SubCategoryDTO, SubcategoryModel>().ReverseMap();
            CreateMap<CityModel, CitiesDTO>().ReverseMap();
            CreateMap<DistrictModel, DistrictDTO>().ReverseMap();
            CreateMap<MunicipalityModel, MunicipalityDTO>().ReverseMap();
            CreateMap<ProvinceModel, ProvinceDTO>().ReverseMap();
            CreateMap<SuburbModel, DTOs.DTO_Suburbs.SuburbDTO>()
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City != null ? src.City.Designation : null))
                .ForMember(dest => dest.MunicipalityName, opt => opt.MapFrom(src => src.Municipality != null ? src.Municipality.Designation : null))
                .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.District != null ? src.District.Designation : null))
                .ForMember(dest => dest.ProvinceName, opt => opt.MapFrom(src => src.Municipality != null && src.Municipality.Province != null ? src.Municipality.Province.Designation : null))
                .ReverseMap();
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
            CreateMap<DTOs.DTO_Addresses.SuburbDTO, SuburbModel>()
                .ReverseMap()
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City != null ? src.City.Designation : null))
                .ForMember(dest => dest.MunicipalityName, opt => opt.MapFrom(src => src.Municipality != null ? src.Municipality.Designation : null));
            CreateMap<Create_BuildingType_DTO, BuildingTypeModel>().ReverseMap();
            
       
        }

    }
}
