using AutoMapper;
using LarksWebApp.DTOs;
using LarksWebApp.DTOs.DTO_Provinces;
using LarksWebApp.Repository.CountryRepository;
using LarksWebApp.Services.Loggers;
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
        public IActionResult GetCountry(int id)
        {

            try
            {
                var countryObj = _countryRepository.GetCountry(id);

                if (countryObj == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                var countryDTO = _mapper.Map<CountryDTO>(countryObj);

                var provincesListDTO = new List<ProvinceDTO>();

                foreach (var province in countryObj.Provinces)
                {
                    provincesListDTO.Add(_mapper.Map<ProvinceDTO>(province));
                }

                countryDTO.ProvincesDTO = provincesListDTO;

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
