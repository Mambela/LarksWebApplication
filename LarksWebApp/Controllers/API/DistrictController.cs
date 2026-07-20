using AutoMapper;
using LarksWebApp.DTOs.DTO_Districts;
using LarksWebApp.Models;
using LarksWebApp.Repository.DistrictRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LarksWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly IMapper _mapper;

        private readonly ServiceLogger serviceLogger;


        public DistrictController(IDistrictRepository districtRepository,IMapper mapper)
        {
            _districtRepository = districtRepository;
            _mapper = mapper;
            serviceLogger = new ServiceLogger();
        }

        [HttpGet]
        [ProducesResponseType(typeof(Create_Districts_DTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult GetDistrict(Guid districtID)
        {
            try
            {

                var disctrictObj = _districtRepository.GetDistrictModel(districtID);


                if (disctrictObj == null)
                {

                    return NotFound();
                }

                var districtDTO = _mapper.Map<Create_Districts_DTO>(disctrictObj);
                return Ok(districtDTO);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("districtsinprovice")]
        [ProducesResponseType(typeof(List<DistrictDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult GetDistrictsInProvince(Guid provinceID) {

            try
            {
                var districtObjList = _districtRepository.GetDistrictModels(provinceID);

                var districtDTOList = _mapper.Map<List<DistrictDTO>>(districtObjList);

                return Ok(districtDTOList);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return BadRequest("Failed to retrieve districts in province");
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(Create_Districts_DTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateDistrict(Create_Districts_DTO createDistrictDTO)
        {

            try
            {

                if (createDistrictDTO == null) {
                    return BadRequest(ModelState);
                }


                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                if (_districtRepository.VerifyDistrictExist(createDistrictDTO.Designation, createDistrictDTO.ProvinceId) !=null)
                {
                    ModelState.AddModelError("", $"The record {createDistrictDTO.Designation} already exists in this province");

                    return BadRequest(ModelState);
                }



                var districtObj = _mapper.Map<DistrictModel>(createDistrictDTO);

                if (await _districtRepository.CreateDistrictModel(districtObj) == null)
                {
                    ModelState.AddModelError("",$"Something went wrong while creating the record {districtObj.Designation}");


                    return StatusCode(500, ModelState);
                }

                createDistrictDTO.Id = districtObj.Id;

                return StatusCode(StatusCodes.Status201Created, createDistrictDTO);
               
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPatch]
        [ProducesResponseType(typeof(Create_Districts_DTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Create_Districts_DTO), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateDistrict([FromBody] Create_Districts_DTO updateDTO, Guid districtId)
        {
            try
            {

                if (updateDTO == null || updateDTO.Id != districtId)
                {
                    return BadRequest(ModelState);
                }

                var districtObj = _mapper.Map<DistrictModel>(updateDTO);

                if (!_districtRepository.UpdateDistrict(districtObj))
                {
                    ModelState.AddModelError("", $"Something went wrong while updating the record {districtObj.Designation}");

                    return StatusCode(StatusCodes.Status500InternalServerError, updateDTO);
                }

                return Ok(updateDTO);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }




    }
}
