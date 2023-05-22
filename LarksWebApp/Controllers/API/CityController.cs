using LarksWebApp.DTOs.DTO_Addresses;
using LarksWebApp.Repository.CityRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityRepository cityRepository;

        public CityController( ICityRepository _cityRepository)
        {

            cityRepository = _cityRepository;   
        }

        [HttpPost]
        [Route("CreateAddress")]
        public async Task<IActionResult> CreateLocationEntity(NominatimAddressDTO nominatimAddressDTO)
        {

            try
            {


                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
     }
}
