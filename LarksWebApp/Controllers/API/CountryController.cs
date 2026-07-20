using AutoMapper;
using LarksWebApp.DTOs;
using LarksWebApp.DTOs.DTO_City;
using LarksWebApp.DTOs.DTO_Country;
using LarksWebApp.DTOs.DTO_Districts;
using LarksWebApp.DTOs.DTO_Municipalities;
using LarksWebApp.DTOs.DTO_Provinces;
using LarksWebApp.Repository.CountryRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;

        public CountriesController(ICountryRepository countryRepository, IMapper mapper, ILoggerTrace serviceLogger)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
            this.serviceLogger = serviceLogger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ViewCountryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult GetCountry(Guid id)
        {

            try
            {
                var countryObj = _countryRepository.GetCountry(id);

                if (countryObj == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                var countryDTO = _mapper.Map<ViewCountryDTO>(countryObj);

                var provincesListDTO = new List<ViewProvinceDTO>();

                var municipalitiesDTO = new List<MunicipalityDTO>();
                var districts = new List<DistrictDTO>();
                var cities = new List<CitiesDTO>();

                foreach (var province in countryObj.Provinces)
                {
                    var provinceDTO = _mapper.Map<ViewProvinceDTO>(province);

                    if (province.Districts!=null)
                    {
                        provinceDTO.Districts = _mapper.Map<List<DistrictDTO>>(province.Districts);  
                    }

                    if (province.Municipalities != null)
                    {
                        provinceDTO.Municipalities = _mapper.Map<List<MunicipalityDTO>>(province.Municipalities);
                    }

                    if (province.Cities != null)
                    {
                        provinceDTO.Cities = _mapper.Map<List<CitiesDTO>>(province.Cities);
                    }

                    provincesListDTO.Add(provinceDTO);
                }

                countryDTO.Provinces = provincesListDTO;

                return Ok(countryDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


        [HttpGet("allcountries")]
        [ProducesResponseType(typeof(List<CountryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult GetCountries()
        {
            try
            {
                var countryOBjList = _countryRepository.GetAll();

                var countryOBJList = _mapper.Map<List<CountryDTO>>(countryOBjList);    

                return Ok(countryOBJList);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("countryByName")]
        [ProducesResponseType(typeof(ViewCountryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult GetCountries(string countryName)
        {
            try
            {
                var countryObj = _countryRepository.CountryExistis(countryName);

                var countryDTO = _mapper.Map<ViewCountryDTO>(countryObj);

                return Ok(countryDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return BadRequest("Failed to retrieve country");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [ProducesResponseType(typeof(Create_Country_DTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateCountry([FromBody] Create_Country_DTO countryDTO, Guid countryId)
        {
            try
            {
                if (countryDTO == null || countryDTO.Id != countryId)
                {
                    return BadRequest(ModelState);
                }

                var countryObj = _countryRepository.GetCountry(countryId);

                if (countryObj == null)
                {
                    return NotFound();
                }

                countryObj.Designation = countryDTO.Designation;

                if (!_countryRepository.UpdateCountry(countryObj))
                {
                    ModelState.AddModelError("", $"Something went wrong while updating the record {countryObj.Designation}");

                    return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
                }

                return Ok(countryDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
