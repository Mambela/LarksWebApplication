using AutoMapper;
using LarksWebApp.DTOs;
using LarksWebApp.Models;
using LarksWebApp.Repository.AddressRepository.BuildingTypeRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingTypeController : ControllerBase
    {

        /*
         * Building type referes to the Type of Building if its a Building House, 
         * 
         * ***/

        private readonly IBuildingTypeRepository buildingTypeRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;

        public BuildingTypeController(IBuildingTypeRepository buildingTypeRepository, IMapper mapper, ILoggerTrace serviceLogger)
        {
            this.buildingTypeRepository = buildingTypeRepository;
            _mapper = mapper;
            this.serviceLogger = serviceLogger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Create_BuildingType_DTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBuildingType(Create_BuildingType_DTO buildingType_DTO)
        {
            try
            {
                if (buildingType_DTO == null)
                {
                    return BadRequest();
                }


                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var buildingObj = _mapper.Map<BuildingTypeModel>(buildingType_DTO);

                if (await buildingTypeRepository.CreateBuildingType(buildingObj) == null)
                {

                    ModelState.AddModelError("", $"An error occurred while attempting to create {buildingObj.Designation}");

                    return BadRequest(ModelState);
                }

                buildingType_DTO.Id = buildingObj.Id;

                return StatusCode(StatusCodes.Status201Created,buildingType_DTO);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return BadRequest();
            }
        }


        [HttpGet]
        [ProducesResponseType(typeof(Create_BuildingType_DTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetBuildingType(Guid id)
        {
            try
            {
                var buildingTypeObj = buildingTypeRepository.GetBuildingType(id);

                var buildingTypeDTO = _mapper.Map<Create_BuildingType_DTO>(buildingTypeObj);


                return Ok(buildingTypeDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(IList<Create_BuildingType_DTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult GetBuildingTypes()
        {
            try
            {
                var buildingTypeObjList = buildingTypeRepository.GetAll();

                var buildingDTOList = _mapper.Map<IList<Create_BuildingType_DTO>>(buildingTypeObjList);

                return Ok(buildingDTOList);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("buildingTypesbyname")]
        [ProducesResponseType(typeof(IList<Create_BuildingType_DTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAllBuildingTypes(string lng)
        {
            try
            {

                var buildingTypeObjList = buildingTypeRepository.GetAll(lng);


                var buildingDTOList = _mapper.Map<IList<Create_BuildingType_DTO>>(buildingTypeObjList);


                return Ok(buildingDTOList);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return BadRequest();
            }
        }

    }
}
