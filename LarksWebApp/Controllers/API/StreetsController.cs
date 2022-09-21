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
                    var street = new View_Street_DTO {
                        Id = StreetObj.Streets.Id,
                        Designation = StreetObj.Streets.Designation
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
                        MunicipalityId = neighbourhoodStreet.Neighbourhood.MunicipalityId,
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

                var neighbourhoodList = new List<NeighbourhoodStreets>();

                for (int i = 0; i < streetDTO.NeighbourhoodID.Count; i++)
                {

                    var neighbourhooObj = new NeighbourhoodStreets
                    {
                        NeighbourhoodID = streetDTO.NeighbourhoodID[i],
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

    }
}
