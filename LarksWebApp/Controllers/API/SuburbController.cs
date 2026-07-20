using AutoMapper;
using LarksWebApp.DTOs.DTO_Addresses;
using LarksWebApp.DTOs.DTO_Suburbs;
using LarksWebApp.DTOs.PaginationDTO;
using LarksWebApp.Models;
using LarksWebApp.Repository.AddressRepository.Address_Repository;
using LarksWebApp.Repository.SuburbRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;




namespace LarksWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuburbController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISuburbRepository suburbRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ServiceLogger serviceLogger;


        public SuburbController(IMapper mapper, ISuburbRepository hoodRepository,IAddressRepository addressRepository)
        {
            _mapper = mapper;
            suburbRepository = hoodRepository;
            serviceLogger = new ServiceLogger();
            _addressRepository = addressRepository;
        }

        /**
         * https://localhost:44339/api/Suburb/all
         * **/
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<DTOs.DTO_Suburbs.SuburbDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetHood()
        {
            try
            {
                var hoodListObj = suburbRepository.GetSuburbs();

                var hoodListDTO = new List<DTOs.DTO_Suburbs.SuburbDTO>();

                foreach(var hood in hoodListObj)
                {
                    hoodListDTO.Add(_mapper.Map<DTOs.DTO_Suburbs.SuburbDTO>(hood));
                }

                return Ok(hoodListDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Get list of suburbs " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("sectorSuburb")]
        [ProducesResponseType(typeof(List<DTOs.DTO_Suburbs.SuburbDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult sectorSuburb(string sector, Guid id)
        {
            
            try
            {   
                var hoodObjList = suburbRepository.GetSuburbs(sector,id);


                if (hoodObjList == null)
                {
                    return Ok(hoodObjList);
                }
                var hoodDTOList = new List<DTOs.DTO_Suburbs.SuburbDTO>();

                foreach (var hood in hoodObjList)
                {
                    hoodDTOList.Add(_mapper.Map<DTOs.DTO_Suburbs.SuburbDTO>(hood));
                }

                
                return Ok(hoodDTOList);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Suburb Sector " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while trying to retrieve suburbs ");
            }

        }

        [HttpGet("suburbInAnySector")]
        [ProducesResponseType(typeof(List<DTOs.DTO_Suburbs.SuburbDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult suburbInAnySector(Guid districtId,Guid municipalityId, Guid cityId)
        {
            try
            {   
                var suburbObjList = suburbRepository.GetSuburbs(districtId,municipalityId,cityId);

                var suburbDTOList = new List<DTOs.DTO_Suburbs.SuburbDTO>();

                foreach (var hood in suburbObjList)
                {
                    suburbDTOList.Add(_mapper.Map<DTOs.DTO_Suburbs.SuburbDTO>(hood));
                }

                
                return Ok(suburbDTOList);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Suburb Sector " + ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while trying to retrieve suburbs ");
            }

        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(Create_suburb_DTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSuburb(Create_suburb_DTO suburb_DTO)
        {

            try
            {

                if (suburb_DTO == null)
                {
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                if (suburbRepository.VerifySuburbExist(suburb_DTO.Designation, suburb_DTO.MunicipalityId) ==null)
                {
                    ModelState.AddModelError("", $"The record {suburb_DTO.Designation} already exists as a country");

                    return BadRequest(ModelState);
                }

                var HoodObj = _mapper.Map<SuburbModel>(suburb_DTO);


                if (await suburbRepository.CreateSuburb(HoodObj) == null)
                {
                    ModelState.AddModelError("", $"Something went wrong while creating the record {HoodObj.Designation}");

                    return StatusCode(500, ModelState);
                }


                return StatusCode(StatusCodes.Status201Created, suburb_DTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(DataTableResponse<List<ViewAddressDataTableDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult AddressesInSuburb(Guid suburbId)
        {
            try
            {
                //fetching data 
                var addressObjList = _addressRepository.GetAddressesInSuburb(suburbId);

                var viewAddressDTOList = new List<ViewAddressDataTableDTO>();



                foreach (var addressObj in addressObjList)
                {

                    var addressDTO = new ViewAddressDataTableDTO
                    {
                        Id = addressObj.Id,
                        Municipality = addressObj.Municipality.Designation,
                        Suburb = addressObj.Suburb.Designation,
                        StreetName = addressObj.Street.Designation,                        
                        Province = addressObj.Province.Designation,
                        FullAddress = addressObj.StreetNumber + ", " + addressObj.Street.Designation + ", " + addressObj.Suburb.Designation + ", " + addressObj.Province.Designation

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
                serviceLogger.LogError("Fetch Address in Suburb " + ex.Message);

                return BadRequest(ex.Message);
            }
        }


        [HttpGet("ListOfSuburb")]
        [ProducesResponseType(typeof(IList<DTOs.DTO_Addresses.SuburbDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetListSububurb([FromQuery] string suburbsQuery)
        {
            try
            {

                var suburbList = suburbsQuery.Split(',')?.Select(Guid.Parse)?.ToList();

                var suburbObjList = suburbRepository.GetSuburbs(suburbList);


                var suburbDTOList = _mapper.Map<IList<DTOs.DTO_Addresses.SuburbDTO>>(suburbObjList);
                return Ok(suburbDTOList);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("error " + ex.Message);

                return BadRequest();
            }
        }


        [HttpGet("SuburbList")]
        [ProducesResponseType(typeof(IList<DTOs.DTO_Addresses.SuburbDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetSuburbList([FromQuery] Guid []suburbList)
        {
            try
            {
                var suburbObjList = suburbRepository.GetSuburbs(suburbList);


                var suburbDTOList = _mapper.Map<IList<DTOs.DTO_Addresses.SuburbDTO>>(suburbObjList);
                return Ok(suburbDTOList);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("error " + ex.Message);

                return BadRequest();
            }
        }

        /**
         * https://localhost:44339/api/Suburb/Suburb?id=1
         * **/

        [HttpGet("Suburb")]
        [ProducesResponseType(typeof(DTOs.DTO_Suburbs.SuburbDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult GetSuburb([FromQuery] Guid id)
        {

            try
            {
                var hoodObj = suburbRepository.GetSuburb(id);

                var hoodDTO = _mapper.Map<DTOs.DTO_Suburbs.SuburbDTO>(hoodObj);


                return Ok(hoodDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("SuburbInProvince")]
        [ProducesResponseType(typeof(List<DTOs.DTO_Suburbs.SuburbDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetSuburbsInProvince([FromQuery] Guid provinceId)
        {
            try { 

                var suburbObjList = suburbRepository.GetSuburbsInProvince(provinceId);

                var suburbDTOList = new List<DTOs.DTO_Suburbs.SuburbDTO>();

                foreach (var hood in suburbObjList)
                {
                    suburbDTOList.Add(_mapper.Map<DTOs.DTO_Suburbs.SuburbDTO>(hood));
                }

                return Ok(suburbDTOList);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("error " + ex.Message);
                return BadRequest();                
            }
        }

        [HttpGet("SuburbByName")]
        [ProducesResponseType(typeof(IList<DTOs.DTO_Suburbs.SuburbDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult GetSuburbByName([FromQuery] string suburbname)
        {

            try
            {
                if(suburbname.Length < 3)
                {
                    ModelState.AddModelError("errors", "Suburb name has to be more than 2 characters");
                  
                    return BadRequest(ModelState);
                }

                var hoodObj = suburbRepository.GetSuburbsByName(suburbname);

                var hoodDTO = _mapper.Map<IList<DTOs.DTO_Suburbs.SuburbDTO>>(hoodObj);

                return Ok(hoodDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [ProducesResponseType(typeof(Create_suburb_DTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Create_suburb_DTO), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateSuburb([FromBody] Create_suburb_DTO hoodDTO, Guid hoodId)
        {

            try
            {
                if (hoodDTO == null || hoodDTO.Id != hoodId) return BadRequest(ModelState);

                var suburbObj = _mapper.Map<SuburbModel>(hoodDTO);

                if (!suburbRepository.UpdateSuburb(suburbObj))
                {
                    ModelState.AddModelError("", $"Something went wrong while updatig the record {suburbObj.Designation}");

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

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteSuburb(Guid suburbId)
        {
            try
            {
                var suburbObj = suburbRepository.GetSuburb(suburbId);

                if (suburbObj == null)
                {
                    return NotFound();
                }

                if (!suburbRepository.DeleteSuburb(suburbObj))
                {
                    ModelState.AddModelError("", $"Something went wrong while deleting the record {suburbObj.Designation}");

                    return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
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
