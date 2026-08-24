using AutoMapper;
using LarksWebApp.DTOs.DTO_Provinces;
using LarksWebApp.Models;
using LarksWebApp.Models.CoordinatesModel;
using LarksWebApp.Repository.Coordinates.ProviceCoordinateRepository;
using LarksWebApp.Repository.ProviceRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private readonly IProvinceRepository _provinceRepository;
        private readonly IMapper _mapper;
        private readonly IProvinceCoordinatesRepository _coordinatesRepository;
        private readonly ILoggerTrace serviceLogger;

        public ProvinceController(IProvinceRepository provinceRepository, IMapper mapper, IProvinceCoordinatesRepository coordinatesRepository,ILoggerTrace ServiceLogger)
        {
            _provinceRepository = provinceRepository;
            _mapper = mapper;
            _coordinatesRepository = coordinatesRepository;
            serviceLogger = ServiceLogger;
        }

        [HttpGet("CountryProvinces")]
        [ProducesResponseType(typeof(List<ProvinceDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetCountryProvince(Guid CountryId)
        {
            try
            {
                var provincesObj = _provinceRepository.GetProvincesInCountry(CountryId);

                var proviceDTOList = new List<ProvinceDTO>();

                proviceDTOList = _mapper.Map<List<ProvinceDTO>>(provincesObj);

                return Ok(proviceDTOList);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Get Provinces in Country " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
          
        }

        [HttpGet("ProvincesInCountry")]
        [ProducesResponseType(typeof(List<ProvinceDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult GetProvincesinCountry(string countryname)
        {
            try
            {
                var provincesObj = _provinceRepository.GetProvincesInCountry(countryname);

                var proviceDTO= new List<ProvinceDTO>();

                proviceDTO= _mapper.Map<List<ProvinceDTO>>(provincesObj);

                return Ok(proviceDTO);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return BadRequest("Failed to retrieve coutry");
            }
        }

        [HttpGet("Province")]
        [ProducesResponseType(typeof(Create_Province_DTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetProvince(Guid provinceID)
        {
            var provinceObj = _provinceRepository.GetProvinceModel(provinceID);

            if (provinceObj == null)
            {
                return NotFound();
            }

            var provinceDTO = _mapper.Map<Create_Province_DTO>(provinceObj);

            return Ok(provinceDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(Create_Province_DTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Create_Province_DTO), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProvince(Create_Province_DTO provinceDTO)
        {

            try
            {


                if (provinceDTO == null)
                {
                    return BadRequest();
                }

                var existingProvince = _provinceRepository.VerifyProvinceExist(provinceDTO.Designation, provinceDTO.CountryId);

                if (existingProvince != null)
                {
                    return Ok(_mapper.Map<Create_Province_DTO>(existingProvince));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var provinceObj = _mapper.Map<ProvinceModel>(provinceDTO);

                if (await _provinceRepository.CreateProvinceModel(provinceObj) == null)
                {
                    ModelState.AddModelError("", $" Something went wrong while creating the record {provinceObj.Designation}");


                    return StatusCode(StatusCodes.Status500InternalServerError, provinceDTO);
                }

                if (provinceDTO.Points.Count == null)
                {
                    if (provinceDTO.Points.Count > 2)
                    {
                        var provinceCoordinatesList = new List<ProvinceCoordinates>();

                        foreach (var point in provinceDTO.Points)
                        {
                            var provinceCoordinate = new ProvinceCoordinates
                            {
                                ProvinceId = provinceObj.Id,
                                lat = point.lat,
                                lng = point.lng
                            };

                            provinceCoordinatesList.Add(provinceCoordinate);

                        }


                        if (!await _coordinatesRepository.CreateProvinceCoordinates(provinceCoordinatesList))
                        {
                            ModelState.AddModelError("", $" Something went wrong while creating the record {provinceObj.Designation} coordinates");

                            return StatusCode(StatusCodes.Status500InternalServerError, provinceDTO);
                        }

                    }
                    else
                    {

                        string message = "Province has been saved, please note that the coordinates have not been saved. You need at least 3 coordinates to make a polygon.";

                        return StatusCode(StatusCodes.Status201Created, message);

                    }
                }
                provinceDTO.Id = provinceObj.Id;


                return Ok(provinceDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteProvince(Guid ProvinceID)
        {

            if (!_provinceRepository.VerifyProvinceExist(ProvinceID))
            {
                return NotFound();
            }


            var provinceObj = _provinceRepository.GetProvinceModel(ProvinceID);

            if (!_provinceRepository.DeleteProvince(provinceObj))
            {

                ModelState.AddModelError("", $"Something went wrong while trying to  delete {provinceObj.Designation}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [ProducesResponseType(typeof(ProvinceModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Create_Province_DTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProvince(Create_Province_DTO provinceDTO, Guid ProvinceId)
        {
            try
            {
                if (provinceDTO == null || provinceDTO.Id == ProvinceId)
                {

                    return BadRequest(ModelState);
                }


                var provinceObj = _mapper.Map<ProvinceModel>(provinceDTO);

                if (!_provinceRepository.UpdateProvince(provinceObj))
                {

                    ModelState.AddModelError("", $"Something went wrong while updating the record {provinceObj.Designation}");

                    return StatusCode(StatusCodes.Status500InternalServerError, provinceDTO);
                }


                //check to vei=rify if the coordinates needs updates. 
                if (provinceDTO.IsUpdateCoordinates)
                {

                    //Check to verify if there are coordinates 
                    if (provinceDTO.Points.Count > 0)
                    {
                        var provinceCoordinatesList = new List<ProvinceCoordinates>();

                        foreach (var point in provinceDTO.Points)
                        {
                            var provinceCoordinate = new ProvinceCoordinates
                            {
                                ProvinceId = provinceObj.Id,
                                lat = point.lat,
                                lng = point.lng
                            };

                            provinceCoordinatesList.Add(provinceCoordinate);

                        }


                        if (!await _coordinatesRepository.CreateProvinceCoordinates(provinceCoordinatesList))
                        {
                            ModelState.AddModelError("", $" Something went wrong while creating the record {provinceObj.Designation} coordinates");

                            return StatusCode(StatusCodes.Status500InternalServerError, provinceDTO);
                        }

                    }

                }

                return Ok(provinceObj);


            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
