using AutoMapper;
using LarksWebApp.DTOs.DTO_Addresses;
using LarksWebApp.Models;
using LarksWebApp.Repository.AddressRepository.Address_Repository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;



        public AddressController(IAddressRepository addressRepository, IMapper mapper, ILoggerTrace serviceLogger)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            this.serviceLogger = serviceLogger;
        }


        [HttpGet("StreetAddress")]
        public IActionResult GetStreetAddress(int streetId)
        {

            try
            {
                var addressObjList = _addressRepository.GetAddressesInStreets(streetId);

                var viewAddressDTOList = new List<ViewAddressDataTableDTO>();



                foreach (var addressObj in addressObjList)
                {

                    var addressDTO = new ViewAddressDataTableDTO
                    {
                        Id = addressObj.Id,
                        Municipality = addressObj.Municipality.Designation,
                        Neighbourhood = addressObj.Neighbourhood.Designation,
                        StreetName = addressObj.Street.Designation,
                        Province = addressObj.Province.Designation,
                        FullAddress = addressObj.StreetNumber + ", " + addressObj.Street.Designation + ", " + addressObj.Neighbourhood.Designation + ", " + addressObj.Province.Designation
                    };

                    viewAddressDTOList.Add(addressDTO);

                }

                return Ok(viewAddressDTOList);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Error while retrieving address " + ex.Message);
                return BadRequest();
            }


        }


        [HttpGet("NeighbourhoodAddress")]
        public IActionResult GetNeighbourhoodAddress(int neighbourhoodId)
        {

            try
            {
                var addressObjList = _addressRepository.GetAddressesInStreets(neighbourhoodId);

                var viewAddressDTOList = new List<ViewAddressDTO>();



                foreach (var addressObj in addressObjList)
                {

                    var addressDTO = new ViewAddressDTO
                    {
                        Id = addressObj.Id,
                        Municipality = addressObj.Municipality.Designation,
                        Neighbourhood = addressObj.Neighbourhood.Designation,
                        StreetName = addressObj.Street.Designation,
                        StreetNumber = addressObj.StreetNumber,
                        Province = addressObj.Province.Designation,

                    };

                    viewAddressDTOList.Add(addressDTO);

                }

                return Ok(viewAddressDTOList);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Error while retrieving address " + ex.Message);
                return BadRequest();
            }
        }



        [HttpGet("AddressByCoordinates")]
        public IActionResult Get(int Limit, double latitude, double longitude)
        {

            try
            {
                var addressObjList = _addressRepository.GetAddressesNearLocation(latitude, longitude);

                var viewAddressDTOList = new List<ViewAddressDTO>();



                foreach (var addressObj in addressObjList)
                {

                    var addressDTO = new ViewAddressDTO
                    {
                        Id = addressObj.Id,
                        Municipality = addressObj.Municipality.Designation,
                        Neighbourhood = addressObj.Neighbourhood.Designation,
                        StreetName = addressObj.Street.Designation,
                        StreetNumber = addressObj.StreetNumber,
                        Province = addressObj.Province.Designation,

                    };

                    viewAddressDTOList.Add(addressDTO);

                }

                return Ok(viewAddressDTOList);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Error while retrieving address " + ex.Message);
                return BadRequest();
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateAddress(Create_Addresses_DTO addressDTO)
        {
            try
            {
                if (addressDTO == null)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var addressObj = _mapper.Map<AddressModel>(addressDTO);
                addressObj.lat = addressDTO.Coordinates.lat;
                addressObj.lng = addressDTO.Coordinates.lng;

                if (await _addressRepository.CreateAddress(addressObj) == null)
                {
                    ModelState.AddModelError("", $"Something went wrong while creating the address");

                    return StatusCode(500, ModelState);
                }


                addressDTO.Id = addressObj.Id;

                return StatusCode(StatusCodes.Status201Created, addressDTO);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Error while retrieving address " + ex.Message);
                return BadRequest();
            }
        }

    }
}
