using AutoMapper;
using LarksWebApp.DTOs.DTO_Addresses;
using LarksWebApp.DTOs.DTO_City;
using LarksWebApp.Models;
using LarksWebApp.Repository.CityRepository;
using LarksWebApp.Services.Loggers;
using LarksWebApp.Services.PaginationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityRepository cityRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace logger;
        private readonly IUriService uriService;
        public CityController( ICityRepository _cityRepository,IMapper mapper,ILoggerTrace loggerTrace)
        {
            _mapper = mapper;
            cityRepository = _cityRepository;   
            logger = loggerTrace;   
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCity(Create_City_DTO createCityDTO)
        {
            try
            {
                if(createCityDTO == null)
                {
                    return BadRequest();
                }

                if(!ModelState.IsValid) 
                {
                    return BadRequest(ModelState);
                }
                

                var cityObj = _mapper.Map<CityModel>(createCityDTO);

                if(await cityRepository.CreateCity(cityObj) ==null) 
                {

                    ModelState.AddModelError("", "Error occurred while trying to create a new city");
                    return BadRequest(ModelState);
                }


                createCityDTO.Id = cityObj.Id;

                return Ok(createCityDTO);
            }
            catch (Exception ex)
            {

                logger.LogError("Error while trying to create City " + ex.Message);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("city")]
        public IActionResult RetrieveCity(int cityId)
        {
            try
            {

                var cityObj = cityRepository.RetrieveCity(cityId);

                var cityDTO = _mapper.Map<CitiesDTO>(cityObj);


                return Ok(cityDTO);
            }
            catch (Exception ex)
            {

                logger.LogError("Error while retrieving a city" + ex.Message);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("citiesinprovince")]
        public IActionResult RetrieveCitiesInProvince(int provinceId)
        {
            try
            {
                var cityObjList = cityRepository.GetAllCitiesInProvince(provinceId);


                var cityDTOList = _mapper.Map<IList<CitiesDTO>>(cityObjList);
                

                return Ok(cityDTOList);
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to update city " + ex.Message);
                return BadRequest("Failed to update City");

            }
        }
        
        [HttpGet]
        [Route("citiesincountry")]
        public IActionResult RetrieveCitiesInCountry(int countryId)
        {
            try
            {
                var cityObjList = cityRepository.GetAllCitiesInCountry(countryId);


                var cityDTOList = _mapper.Map<IList<CitiesDTO>>(cityObjList);
                

                return Ok(cityDTOList);
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to update city " + ex.Message);
                return BadRequest("Failed to update City");

            }
        }

        [HttpPatch]
        public IActionResult Update(Create_City_DTO create_City_DTO, int  cityId)
        {
            try
            {
                if (create_City_DTO == null || create_City_DTO.Id != cityId)
                {
                    return BadRequest();
                }


                var cityObj = _mapper.Map<CityModel>(create_City_DTO);

                if (! cityRepository.UpdateCity(cityObj)) {

                    return BadRequest("Failed to Update City");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to update city "+ex.Message);
                return BadRequest("Failed to update City");
                
            }
        }

        [HttpDelete]
        public IActionResult DeleteCity(int cityId)
        {
            try
            {

                var citymodel = cityRepository.GetCity(cityId);

                if (citymodel == null)
                {

                    return NotFound();
                }

                if (cityRepository.DeleteCity(citymodel))
                {
                    ModelState.AddModelError("error", "Failed to delete city ");
                    return BadRequest();
                }


                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting city {ex.Message}");
                return BadRequest("An error occurred while trying to delete a City");
            }
        }

    }
}
