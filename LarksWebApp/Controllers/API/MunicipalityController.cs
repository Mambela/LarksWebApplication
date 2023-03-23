using AutoMapper;
using LarksWebApp.DTOs.DTO_Municipalities;
using LarksWebApp.Models;
using LarksWebApp.Models.CoordinatesModel;
using LarksWebApp.Repository.Coordinates.MunicipalityCoordinateRepository;
using LarksWebApp.Repository.MunicipalityRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers.API
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MunicipalityController : ControllerBase
    {
        private readonly IMunicipalityRepository _municipalityRepository;
        private readonly IMunicipalityCoordinateRepository _municipalityCoordinates;
        private readonly IMapper _mapper;
        private readonly ServiceLogger serviceLogger;


        public MunicipalityController(IMunicipalityRepository municipalityRepository, IMapper mapper, IMunicipalityCoordinateRepository municipalityCoordinates)
        {
            _municipalityRepository = municipalityRepository;
            _municipalityCoordinates = municipalityCoordinates;
            _mapper = mapper;
            serviceLogger = new ServiceLogger();
        }

        [HttpGet]
        public IActionResult GetMunicipality(int MuncipalityId)
        {
            try
            {         
                var municipalityObj = _municipalityRepository.GetMuncipality(MuncipalityId);

                if (municipalityObj == null) return NotFound();

                var municipalityDTO = _mapper.Map<Create_Municipality_DTO>(municipalityObj);

                return Ok(municipalityDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("MunicipalitiesInProvince")]
        public IActionResult GetProvinceMunicipalities(int provinceId)
        {
            try
            {
                var municipalityObjList =  _municipalityRepository.GetMunicipalityModels(provinceId);

                if (municipalityObjList == null) return NotFound();

                var municipalityDTOList  = _mapper.Map<IList<Create_Municipality_DTO>>(municipalityObjList);

                return Ok(municipalityDTOList);
            }
            catch (Exception ex)
            {

                serviceLogger.LogError(ex.Message);

                ModelState.AddModelError("error", "failed to retrieve municipalities");

                return BadRequest(ModelState);
            }

        }

        [HttpPost]

        public async Task<IActionResult> CreateMuncipality(Create_Municipality_DTO municipalityDTO)
        {
            try
            {
        
                if (municipalityDTO == null) return BadRequest();

                if (_municipalityRepository.VerifyMunicipalityExist(municipalityDTO.Designation, municipalityDTO.ProvinceId))
                {
                    ModelState.AddModelError("", $"The record {municipalityDTO.Designation} already exists as a country");

                    return BadRequest(ModelState);

                }



                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var muncipalityObj = _mapper.Map<MunicipalityModel>(municipalityDTO);

                if (await _municipalityRepository.CreateMunicipalityModel(muncipalityObj) == null)
                {
                    ModelState.AddModelError("", $"Something went wrong while creating the record {muncipalityObj.Designation}");

                    return StatusCode(500, ModelState);
                }


                if (!municipalityDTO.IsUpdateCoordinates)
                {
                    if (municipalityDTO.Points.Count > 2)
                    {
                        var municipalityList = new List<MunicipalityCoordinates>();

                        foreach (var point in municipalityDTO.Points)
                        {

                            var municipalityCoordinate = new MunicipalityCoordinates
                            {
                                MunicipalityID = muncipalityObj.Id,
                                lat = point.lat,
                                lng = point.lng
                            };

                            municipalityList.Add(municipalityCoordinate);
                        }


                        if (!await _municipalityCoordinates.CreateMunicipalityCoordinates(municipalityList))
                        {

                            ModelState.AddModelError("", $" Something went wrong while creating the record {muncipalityObj.Designation} coordinates");

                            return StatusCode(StatusCodes.Status500InternalServerError, municipalityDTO);

                        }
                    }
                    else
                    {
                        string message = "Municipality has been saved, please note that the coordinates have not been saved. You need at least 3 coordinates to make a polygon.";
                        return StatusCode(StatusCodes.Status201Created, message);
                    }
                }


                municipalityDTO.Id = muncipalityObj.Id;

                return StatusCode(StatusCodes.Status201Created, municipalityDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpPatch]
        public async Task<IActionResult> UpdateMunicipality([FromBody] Create_Municipality_DTO municipality_DTO, int municipalityID)
        {

            try
            {

                if (municipality_DTO == null || municipality_DTO.ProvinceId != municipalityID)
                {

                    return BadRequest(ModelState);
                }

                var municipalityObj = _mapper.Map<MunicipalityModel>(municipality_DTO);

                if (!_municipalityRepository.UpdateMunicipality(municipalityObj))
                {
                    ModelState.AddModelError("", $"Something went wrong while updating the record {municipality_DTO.Designation}");


                    return StatusCode(StatusCodes.Status500InternalServerError, municipality_DTO);
                }


                if (municipality_DTO.IsUpdateCoordinates)
                {

                    if (municipality_DTO.Points.Count > 0)
                    {
                        var municipalityList = new List<MunicipalityCoordinates>();


                        foreach (var point in municipality_DTO.Points)
                        {

                            var municipality = new MunicipalityCoordinates
                            {
                                MunicipalityID = municipalityObj.Id,
                                lat = point.lat,
                                lng = point.lng
                            };

                            municipalityList.Add(municipality);
                        }


                        if (!await _municipalityCoordinates.CreateMunicipalityCoordinates(municipalityList))
                        {
                            ModelState.AddModelError("", $" Something went wrong while creating the record {municipalityObj.Designation} coordinates");

                            return StatusCode(StatusCodes.Status500InternalServerError, municipality_DTO);
                        }
                    }

                }

                return Ok(municipality_DTO);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpDelete]
        public IActionResult DeleteMunicipality(int MunicipalityID)
        {
            try
            {
                if (!_municipalityRepository.VerifyMunicipalityExist(MunicipalityID))
                {
                    return NotFound();
                }


                var municiaplityObj = _municipalityRepository.GetMuncipality(MunicipalityID);


                if (!_municipalityRepository.DeleteMunicipality(municiaplityObj))
                {

                    ModelState.AddModelError("", $"Somehting went Wrong while trying to delete {municiaplityObj.Designation}");

                    return StatusCode(500, ModelState);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
