using AutoMapper;
using LarksWebApp.DTOs.DTO_Neighbourhoods;
using LarksWebApp.Models;
using LarksWebApp.Repository.HoodRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NeighbourhoodController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly INeighbourhoodRepository _hoodRepository;
        private readonly ServiceLogger serviceLogger;


        public NeighbourhoodController(IMapper mapper, INeighbourhoodRepository hoodRepository)
        {
            _mapper = mapper;
            _hoodRepository = hoodRepository;
            serviceLogger = new ServiceLogger();
        }


        [HttpPost]
        public async Task<IActionResult> CreateNeighBourhood(Create_neighbourhood_DTO neighbourhood_DTO)
        {

            try
            {

                    if (neighbourhood_DTO == null)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }


                    if (_hoodRepository.VerifyNeighbourhoodExist(neighbourhood_DTO.Designation,neighbourhood_DTO.MunicipalityId))
                    {
                        ModelState.AddModelError("", $"The record {neighbourhood_DTO.Designation} already exists as a country");

                        return BadRequest(ModelState);
                    }

                    var HoodObj = _mapper.Map<NeighbourhoodModel>(neighbourhood_DTO);


                    if (await _hoodRepository.CreateNeighbourhood(HoodObj) == null)
                    {
                        ModelState.AddModelError("", $"Something went wrong while creating the record {HoodObj.Designation}");

                        return StatusCode(500, ModelState);
                    }


                    return StatusCode(StatusCodes.Status201Created, neighbourhood_DTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


        [HttpGet]
        public IActionResult GetNeighbourhood(int id)
        {

            try
            {
                var hoodObj = _hoodRepository.GetNeighbourhood(id);

                if (hoodObj == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                var hoodDTO = _mapper.Map<NeighbourhoodModel>(hoodObj);


                return Ok(hoodDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut]
        public IActionResult UpdateNeighbourhood([FromBody] Create_neighbourhood_DTO hoodDTO, int hoodId)
        {

            try
            {                 
                if (hoodDTO == null || hoodDTO.Id != hoodId) return BadRequest(ModelState);

                var neighbourHoodObj = _mapper.Map<NeighbourhoodModel>(hoodDTO);

                if (!_hoodRepository.UpdateNeighbourhoody(neighbourHoodObj))
                {
                    ModelState.AddModelError("", $"Something went wrong while updatig the record {neighbourHoodObj.Designation}");

                    return StatusCode(StatusCodes.Status500InternalServerError, hoodDTO);
                }


                return Ok(hoodDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
