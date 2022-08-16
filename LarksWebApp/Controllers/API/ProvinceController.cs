using AutoMapper;
using LarksWebApp.DTOs.DTO_Provinces;
using LarksWebApp.Models;
using LarksWebApp.Models.CoordinatesModel;
using LarksWebApp.Repository.Coordinates.ProviceCoordinateRepository;
using LarksWebApp.Repository.ProviceRepository;
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

        public ProvinceController(IProvinceRepository provinceRepository, IMapper mapper, IProvinceCoordinatesRepository coordinatesRepository)
        {
            _provinceRepository = provinceRepository;
            _mapper = mapper;
            _coordinatesRepository = coordinatesRepository;
        }

        [HttpGet]
        public IActionResult GetProvince(int provinceID)
        {
            var provinceObj = _provinceRepository.GetProvinceModel(provinceID);

            if (provinceObj == null)
            {
                return NotFound();
            }

            var provinceDTO = _mapper.Map<Create_Province_DTO>(provinceObj);

            return Ok(provinceDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProvince(Create_Province_DTO provinceDTO)
        {

            try
            {


                if (provinceDTO == null)
                {
                    return BadRequest();
                }

                if (_provinceRepository.VerifyProvinceExist(provinceDTO.Designation, provinceDTO.CountryId))
                {
                    ModelState.AddModelError("", $"The record {provinceDTO.Designation} already exists as a country");
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

                provinceDTO.Id = provinceObj.Id;


                return Ok(provinceDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        [HttpDelete]

        public IActionResult DeleteProvince(int ProvinceID)
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


        [HttpPatch]
        public async Task<IActionResult> UpdateProvince(Create_Province_DTO provinceDTO, int ProvinceId)
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
