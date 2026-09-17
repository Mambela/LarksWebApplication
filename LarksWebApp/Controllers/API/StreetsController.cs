using AutoMapper;
using LarksWebApp.DTOs;
using LarksWebApp.DTOs.DTO_Suburbs;
using LarksWebApp.DTOs.DTO_Streets;
using LarksWebApp.DTOs.PaginationDTO;
using LarksWebApp.Models;
using LarksWebApp.Models.CoordinatesModel;
using LarksWebApp.Repository.AddressRepository.StreetRepository;
using LarksWebApp.Repository.SuburbStreetRepository;
using LarksWebApp.Repository.StreetRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
//using Serilog;

namespace LarksWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreetsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStreetRepository _streetRepository;
        private readonly ISuburbStreetRepository _suburbStreetRepository;
        private readonly IStreetCoordinateRepository _streetCoordinateRepo;

        private readonly ServiceLogger serviceLogger;

        public StreetsController(IMapper mapper, IStreetRepository streetRepository, ISuburbStreetRepository suburbStreetRepository , IStreetCoordinateRepository streetCoordinateRepository)
        {
            _streetCoordinateRepo = streetCoordinateRepository;
            _mapper = mapper;
            _streetRepository = streetRepository;
            serviceLogger = new ServiceLogger();
            _suburbStreetRepository = suburbStreetRepository;
        }


        [HttpPut]
        [ProducesResponseType(typeof(Create_Street_DTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Create_Street_DTO), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateStreet([FromBody] Create_Street_DTO street_DTO, Guid streetId)
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteStreet(Guid streetID)
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


        [HttpGet("StreetsInSuburb")]
        [ProducesResponseType(typeof(List<View_Street_DTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetStreetsInSuburb(Guid suburbId)
        {
            try
            {
                var suburbStreetObjList = _streetRepository.GetStreetsInSuburb(suburbId);

                var streetListDTO = new List<View_Street_DTO>();

                foreach (var StreetObj in suburbStreetObjList)
                {

                    var streetCoordinates = _mapper.Map<IList<BaseCoordinatesDTO>>(StreetObj.Streets.StreetCoordinates);

                    var street = new View_Street_DTO {
                        Id = StreetObj.Streets.Id,
                        Designation = StreetObj.Streets.Designation,
                        Coordinates = streetCoordinates
                    };

                    streetListDTO.Add(street);
                   
                }

                return Ok(streetListDTO);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Retrieving streets in a Suburb " + ex.Message);
                return BadRequest();
            }        
        }


        [HttpGet("StreetsInSuburbByName")]
        [ProducesResponseType(typeof(List<View_Street_DTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        public IActionResult GetStreetsInSuburbByValue(Guid suburbId, string streetValue)
        {
            try
            {

                var suburbStreetObjList = _streetRepository.GetStreetsInSuburbByValue(suburbId,streetValue);

                var streetListDTO = new List<View_Street_DTO>();

                foreach (var StreetObj in suburbStreetObjList)
                {

                    var streetCoordinates = _mapper.Map<IList<BaseCoordinatesDTO>>(StreetObj.Streets.StreetCoordinates);

                    var street = new View_Street_DTO
                    {
                        Id = StreetObj.Streets.Id,
                        Designation = StreetObj.Streets.Designation,
                        Coordinates = streetCoordinates
                    };

                    streetListDTO.Add(street);

                }

                return Ok(streetListDTO);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", "There was an error whie trying to retrieve the streets");
                serviceLogger.LogError("Retrieving streets in a Suburb " + ex.Message);
                return BadRequest(ModelState);
            }
        }


        [HttpGet("Streets")]
        [ProducesResponseType(typeof(View_Street_DTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult Getstreet(Guid id)
        {
            try
            {
                var streetObj = _streetRepository.GetStreetModel(id);

                if (streetObj == null)
                {
                    return NotFound();
                }

                var streetDTO =new View_Street_DTO();

                var suburbs = new List<SuburbDTO>();

                var streetCoordinatesList = new List<BaseCoordinatesDTO>();

                

                foreach (var suburbStreetLink in streetObj.SuburbLinks)
                {

                    var suburb = new SuburbDTO
                    {
                        Id = suburbStreetLink.Suburb.Id,
                        MunicipalityId = (Guid)suburbStreetLink.Suburb.MunicipalityId,
                        Designation = suburbStreetLink.Suburb.Designation
                    };

                    suburbs.Add(suburb);
                }


                foreach (var coordinateObj in streetObj.StreetCoordinates)
                {

                    var coordinate = new BaseCoordinatesDTO
                    {
                        lat = coordinateObj.lat,
                        lng = coordinateObj.lng
                    };

                    streetCoordinatesList.Add(coordinate);
                }


                streetDTO.Id = streetObj.Id;
                streetDTO.Suburbs = suburbs;
                streetDTO.Designation = streetObj.Designation;
                streetDTO.Coordinates = streetCoordinatesList;
   

                return Ok(streetDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        // [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(Create_Street_DTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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

                /*
                 * Associate street to the Suburbs
                 * **/

                var suburbLinkList = new List<SuburbStreets>();

                for (int i = 0; i < streetDTO.SuburbID.Count; i++)
                {

                    var suburbLinkObj = new SuburbStreets
                    {
                        SuburbId = streetDTO.SuburbID[i],
                        StreetsID = streetObj.Id
                    };

                    suburbLinkList.Add(suburbLinkObj);
                }

                if (!await _suburbStreetRepository.AssociateStreetToSuburbs(suburbLinkList))
                {
                    ModelState.AddModelError("", $"Something went wrong while associating the street {streetObj.Designation} to suburbs");

                    return StatusCode(201, ModelState);
                }


                var streetCoordinatesList = new List<StreetCoordinates>();



                for (int i = 0; i < streetDTO.Coordinates.Count; i++)
                {
                    var streetCoordinatesObj = new StreetCoordinates
                    {
                        StreetId = streetObj.Id,
                        lat = streetDTO.Coordinates[i].lat,
                        lng = streetDTO.Coordinates[i].lng,
                    };

                    streetCoordinatesList.Add(streetCoordinatesObj);
                }


                if (!await _streetCoordinateRepo.StoreStreetCoordinates(streetCoordinatesList))
                {
                    ModelState.AddModelError("", $"Something went wrong while storing the coordinates for the street {streetObj.Designation} ");

                    return StatusCode(201, ModelState);
                }

                /*
                 *
                 * **/

                streetDTO.Id = streetObj.Id;

                return StatusCode(StatusCodes.Status201Created, streetDTO);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        
        
        // end point for DataTable API
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(DataTableResponse<List<View_Street_DTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult StreetsInSuburb(Guid suburbId)
        {
            try
            {
                ///Consider running paginations in the Entity Framework

                var suburbStreetObjList = _streetRepository.GetStreetsInSuburb(suburbId);

                var streetListDTO = new List<View_Street_DTO>();

                foreach (var StreetObj in suburbStreetObjList)
                {

                    var streetCoordinates = _mapper.Map<IList<BaseCoordinatesDTO>>(StreetObj.Streets.StreetCoordinates);

                    var street = new View_Street_DTO
                    {
                        Id = StreetObj.Streets.Id,
                        Designation = StreetObj.Streets.Designation,
                        Coordinates = streetCoordinates
                    };

                    streetListDTO.Add(street);

                }


                //Declarations for the table 
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][Address]"].FirstOrDefault() + "][Address]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 10;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;


                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    streetListDTO = (List<View_Street_DTO>)streetListDTO.OrderBy(c => c.Designation);
                }


                if (!string.IsNullOrEmpty(searchValue))
                {
                    streetListDTO = (List<View_Street_DTO>)streetListDTO.Where(m => m.Designation.Contains(searchValue));

                }

                recordsTotal = streetListDTO.Count();
                var data = streetListDTO.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };

                return Ok(jsonData);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Datatable attempting to retrieve data " + ex.Message);

                return BadRequest();
            }
        }
    }
}
