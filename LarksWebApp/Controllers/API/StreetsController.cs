using AutoMapper;
using LarksWebApp.DTOs.DTO_Streets;
using LarksWebApp.Models;
using LarksWebApp.Repository.StreetRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
//using Serilog;

namespace LarksWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreetsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStreetRepository _streetRepository;

        private readonly ServiceLogger serviceLogger;

        public StreetsController(IMapper mapper, IStreetRepository streetRepository)
        {
            _mapper = mapper;
            _streetRepository = streetRepository;
            serviceLogger = new ServiceLogger();
        }


        [HttpPut]

        public IActionResult UpdateStreet([FromBody] Create_Street_DTO street_DTO, int streetId)
        {
            try
            {
                if (street_DTO == null || street_DTO.Id != streetId)
                {
                    serviceLogger.Warning($"The id {street_DTO.Id} is not found");
                    return BadRequest(ModelState);
                }

                var streetObj = _mapper.Map<StreetModel>(street_DTO);

                if (!_streetRepository.UpdateStreet(streetObj))
                {
                    ModelState.AddModelError("", $"Something went wrong while updating the record {streetObj.Designation}");

                    return StatusCode(StatusCodes.Status500InternalServerError, street_DTO);
                }


                return Ok(street_DTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpDelete]
        public IActionResult DeleteStreet(int streetID)
        {
            try
            {


                if (!_streetRepository.VerifyStreetExist(streetID))
                {
                    return NotFound();
                }

                var streetObj = _streetRepository.GetStreetModel(streetID);

                if (!_streetRepository.DeleteStreet(streetObj))
                {
                    ModelState.AddModelError("", $"Somehting went Wrong while trying to delete {streetObj.Designation}");

                    return StatusCode(500, ModelState);
                }


                return NoContent();

            }
            catch (Exception ex)
            {


                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }


        [HttpGet]
        public IActionResult Getstreet(int id)
        {
            try
            {
                var streetObj = _streetRepository.GetStreetModel(id);

                if (streetObj == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                var streetDTO = _mapper.Map<Create_Street_DTO>(streetObj);


                return Ok(streetDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateStreet(Create_Street_DTO streetDTO)
        {

            try
            {
                    if (streetDTO == null)
                    {
                        return BadRequest(ModelState);
                    }

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }


                    var streetObj = _mapper.Map<StreetModel>(streetDTO);

                    if (await _streetRepository.CreateStreetModel(streetObj) == null)
                    {
                        ModelState.AddModelError("", $"Something went wrong while creating the record {streetObj.Designation}");

                        return StatusCode(500, ModelState);
                    }


                    return StatusCode(StatusCodes.Status201Created, streetDTO);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
