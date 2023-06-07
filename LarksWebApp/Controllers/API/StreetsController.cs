using AutoMapper;
using LarksWebApp.DTOs;
using LarksWebApp.DTOs.DTO_Neighbourhoods;
using LarksWebApp.DTOs.DTO_Streets;
using LarksWebApp.Models;
using LarksWebApp.Models.CoordinatesModel;
using LarksWebApp.Repository.AddressRepository.StreetRepository;
using LarksWebApp.Repository.AssoHoodStreetRepository;
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
        private readonly INeighbourhoodStreet _neighbourhoodStreet;
        private readonly IStreetCoordinateRepository _streetCoordinateRepo;

        private readonly ServiceLogger serviceLogger;

        public StreetsController(IMapper mapper, IStreetRepository streetRepository, INeighbourhoodStreet neighbourhoodStreet , IStreetCoordinateRepository streetCoordinateRepository)
        {
            _streetCoordinateRepo = streetCoordinateRepository;
            _mapper = mapper;
            _streetRepository = streetRepository;
            serviceLogger = new ServiceLogger();
            _neighbourhoodStreet = neighbourhoodStreet;
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


        [HttpGet("StreetsInNeighbourhood")]
        public IActionResult GetstreetsInNeighbourhood(int neighbourhoodId) 
        {
            try
            {
                var neighBourhoodstreetObjList = _streetRepository.GetStreetInNeighBourhood(neighbourhoodId);

                var streetListDTO = new List<View_Street_DTO>();

                foreach (var StreetObj in neighBourhoodstreetObjList)
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
                serviceLogger.LogError("Retrieving streets in a Neighbourhood " + ex.Message);
                return BadRequest();
            }        
        }


        [HttpGet("StreetsInNeighbourhoodByName")]
        public IActionResult GetStreetsInNeighbourhoodByValue(int neighbourhoodId, string streetValue)
        {
            try
            {

                var neighBourhoodstreetObjList = _streetRepository.GetStreetInNeighBourhoodByValue(neighbourhoodId,streetValue);

                var streetListDTO = new List<View_Street_DTO>();

                foreach (var StreetObj in neighBourhoodstreetObjList)
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
                serviceLogger.LogError("Retrieving streets in a Neighbourhood " + ex.Message);
                return BadRequest(ModelState);
            }
        }


        [HttpGet("Streets")]
        public IActionResult Getstreet(int id)
        {
            try
            {
                var streetObj = _streetRepository.GetStreetModel(id);

                var streetDTO =new View_Street_DTO();

                var neighbourhoods = new List<NeighbourhoodDTO>();

                var streetCoordinatesList = new List<BaseCoordinatesDTO>();

                

                foreach (var neighbourhoodStreet in streetObj.NeighbourhoodStreets)
                {

                    var neighbourhood = new NeighbourhoodDTO
                    {
                        Id = neighbourhoodStreet.Neighbourhood.Id,
                        MunicipalityId = (int)neighbourhoodStreet.Neighbourhood.MunicipalityId,
                        Designation = neighbourhoodStreet.Neighbourhood.Designation
                    };

                    neighbourhoods.Add(neighbourhood);
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
                streetDTO.Neighbourhoods = neighbourhoods;
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

        [Authorize(Roles = "Admin")]
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

                /*
                 * Associate street to the Neighbourhoods
                 * **/

                var neighbourhoodList = new List<SuburbStreets>();

                for (int i = 0; i < streetDTO.NeighbourhoodID.Count; i++)
                {

                    var neighbourhooObj = new SuburbStreets
                    {
                        SuburbId = streetDTO.NeighbourhoodID[i],
                        StreetsID = streetObj.Id
                    };

                    neighbourhoodList.Add(neighbourhooObj);
                }

                if (!await _neighbourhoodStreet.AssociateStreetToNeighBourhoods(neighbourhoodList))
                {
                    ModelState.AddModelError("", $"Something went wrong while associating the street {streetObj.Designation} to neighbourhoods");

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
        public IActionResult StreetsInNeighbourhood(int neighbourhoodId)
        {
            try
            {
                ///Consider running paginations in the Entity Framework

                var neighBourhoodstreetObjList = _streetRepository.GetStreetInNeighBourhood(neighbourhoodId);

                var streetListDTO = new List<View_Street_DTO>();

                foreach (var StreetObj in neighBourhoodstreetObjList)
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
