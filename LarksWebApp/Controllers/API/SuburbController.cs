using AutoMapper;
using LarksWebApp.DTOs.DTO_Addresses;
using LarksWebApp.DTOs.DTO_Neighbourhoods;
using LarksWebApp.Models;
using LarksWebApp.Repository.AddressRepository.Address_Repository;
using LarksWebApp.Repository.HoodRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;




namespace LarksWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuburbController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISuburbRepository _hoodRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ServiceLogger serviceLogger;


        public SuburbController(IMapper mapper, ISuburbRepository hoodRepository,IAddressRepository addressRepository)
        {
            _mapper = mapper;
            _hoodRepository = hoodRepository;
            serviceLogger = new ServiceLogger();
            _addressRepository = addressRepository;
        }

        /**
         * https://localhost:44339/api/hood/all
         * **/
        [HttpGet("all")]
        public IActionResult GetHood()
        {
            try
            {
                var hoodListObj = _hoodRepository.GetNeighbourhoodModels();

                var hoodListDTO = new List<NeighbourhoodDTO>();

                foreach(var hood in hoodListObj)
                {
                    hoodListDTO.Add(_mapper.Map<NeighbourhoodDTO>(hood));
                }

                return Ok(hoodListDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Get list of neighbourhood " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("sectorNeighbourhood")]
        public IActionResult sectorNeighbourhood(string sector, int id)
        {
            
            try
            {   
                var hoodObjList = _hoodRepository.GetNeighbourhoodModels(sector,id);

                var hoodDTOList = new List<NeighbourhoodDTO>();

                foreach (var hood in hoodObjList)
                {
                    hoodDTOList.Add(_mapper.Map<NeighbourhoodDTO>(hood));
                }

                
                return Ok(hoodDTOList);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Neighbourhood Sector " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while trying to retrieve neighbourhood ");
            }

        }




        [HttpPost]
        public async Task<IActionResult> CreateSuburb(Create_neighbourhood_DTO neighbourhood_DTO)
        {

            try
            {

                if (neighbourhood_DTO == null)
                {
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                if (_hoodRepository.VerifySuburbExist(neighbourhood_DTO.Designation, neighbourhood_DTO.MunicipalityId) ==null)
                {
                    ModelState.AddModelError("", $"The record {neighbourhood_DTO.Designation} already exists as a country");

                    return BadRequest(ModelState);
                }

                var HoodObj = _mapper.Map<SuburbModel>(neighbourhood_DTO);


                if (await _hoodRepository.CreateSuburb(HoodObj) == null)
                {
                    ModelState.AddModelError("", $"Something went wrong while creating the record {HoodObj.Designation}");

                    return StatusCode(500, ModelState);
                }


                return StatusCode(StatusCodes.Status201Created, neighbourhood_DTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult AddressesInNeighbourhood(int neighbourhoodId)
        {
            try
            {
                //fetching data 
                var addressObjList = _addressRepository.GetAddressesInNeighbourhood(neighbourhoodId);

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
                    viewAddressDTOList = (List<ViewAddressDataTableDTO>)viewAddressDTOList.OrderBy(c => c.FullAddress);
                }


                if (!string.IsNullOrEmpty(searchValue))
                {
                    viewAddressDTOList = (List<ViewAddressDataTableDTO>)viewAddressDTOList.Where(m => m.FullAddress.Contains(searchValue)
                                                || m.StreetName.Contains(searchValue));

                }

                recordsTotal = viewAddressDTOList.Count();
                var data = viewAddressDTOList.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };

                return Ok(jsonData);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Fetch Address in Neighbourhood " + ex.Message);

                return BadRequest(ex.Message);
            }
        }



        /**
         * https://localhost:44339/api/hood/Neighbourhood?id=1
         * **/

        [HttpGet("Neighbourhood")]
        public IActionResult GetNeighbourhood([FromQuery] int id)
        {

            try
            {
                var hoodObj = _hoodRepository.GetNeighbourhood(id);

                var hoodDTO = _mapper.Map<NeighbourhoodDTO>(hoodObj);


                return Ok(hoodDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut]
        public IActionResult UpdateNeighbourhood([FromBody] Create_neighbourhood_DTO hoodDTO, int hoodId)
        {

            try
            {
                if (hoodDTO == null || hoodDTO.Id != hoodId) return BadRequest(ModelState);

                var neighbourHoodObj = _mapper.Map<SuburbModel>(hoodDTO);

                if (!_hoodRepository.UpdateNeighbourhoody(neighbourHoodObj))
                {
                    ModelState.AddModelError("", $"Something went wrong while updatig the record {neighbourHoodObj.Designation}");

                    return StatusCode(StatusCodes.Status500InternalServerError, hoodDTO);
                }


                return Ok(hoodDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
