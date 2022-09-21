using AutoMapper;
using LarksWebApp.DTOs.DTO_Neighbourhoods;
using LarksWebApp.Models;
using LarksWebApp.Repository.HoodRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;




namespace LarksWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoodController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly INeighbourhoodRepository _hoodRepository;
        private readonly ServiceLogger serviceLogger;


        public HoodController(IMapper mapper, INeighbourhoodRepository hoodRepository)
        {
            _mapper = mapper;
            _hoodRepository = hoodRepository;
            serviceLogger = new ServiceLogger();
        }

        /**
         * https://localhost:44339/api/hood/all
         * **/
        [HttpGet("all")]
        public IActionResult GetHood()
        {
            try
            {
                var hoodListObj = _hoodRepository.GetNeighbourhoodModels();

                var hoodListDTO = new List<NeighbourhoodDTO>();

                foreach(var hood in hoodListObj)
                {
                    hoodListDTO.Add(_mapper.Map<NeighbourhoodDTO>(hood));
                }

                return Ok(hoodListDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Get list of neighbourhood " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("sectorNeighbourhood")]
        public IActionResult sectorNeighbourhood(string sector, int id)
        {
            
            try
            {   
                var hoodObjList = _hoodRepository.GetNeighbourhoodModels(sector,id);

                var hoodDTOList = new List<NeighbourhoodDTO>();

                foreach (var hood in hoodObjList)
                {
                    hoodDTOList.Add(_mapper.Map<NeighbourhoodDTO>(hood));
                }

                
                return Ok(hoodDTOList);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Neighbourhood Sector " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while trying to retrieve neighbourhood ");
            }

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


                if (_hoodRepository.VerifyNeighbourhoodExist(neighbourhood_DTO.Designation, neighbourhood_DTO.MunicipalityId))
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



        /**
         * https://localhost:44339/api/hood/Neighbourhood?id=1
         * **/

        [HttpGet("Neighbourhood")]
        public IActionResult GetNeighbourhood([FromQuery] int id)
        {

            try
            {
                var hoodObj = _hoodRepository.GetNeighbourhood(id);

                var hoodDTO = _mapper.Map<NeighbourhoodDTO>(hoodObj);


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
