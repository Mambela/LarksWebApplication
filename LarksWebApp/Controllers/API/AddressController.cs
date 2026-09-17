using AutoMapper;
using LarksWebApp.DTOs;
using LarksWebApp.DTOs.DTO_Addresses;
using LarksWebApp.Models;
using LarksWebApp.Models.CoordinatesModel;
using LarksWebApp.Repository.AddressRepository.Address_Repository;
using LarksWebApp.Repository.AddressRepository.HooksRepository;
using LarksWebApp.Repository.CityRepository;
using LarksWebApp.Repository.CountryRepository;
using LarksWebApp.Repository.DistrictRepository;
using LarksWebApp.Repository.SuburbRepository;
using LarksWebApp.Repository.MunicipalityRepository;
using LarksWebApp.Repository.ProviceRepository;
using LarksWebApp.Repository.StreetRepository;
using LarksWebApp.Services.ExternalService;
using LarksWebApp.Services.Loggers;
using LarksWebApp.Services.WebHooks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.CompilerServices;

namespace LarksWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;
        private readonly ICountryRepository countryRepository;
        private readonly IProvinceRepository provinceRepository;
        private readonly ICityRepository cityRepository;
        private readonly ISuburbRepository suburbRepository;
        private readonly IDistrictRepository districtRepository;
        private readonly IMunicipalityRepository municipalityRepository;
        private readonly ErrorResponses errorResponses;
        private readonly IHooksRepository hookRepository;
        private readonly IWebhookService webhookService;
        private ExternalHttpService externalHttpService;
        private readonly IConfiguration config;
        private readonly IStreetRepository streetRepository;

        public AddressController(
            IAddressRepository addressRepository, IMapper mapper,
            ILoggerTrace serviceLogger, ICountryRepository _countryRepo,
            IProvinceRepository _provinceRepository, ICityRepository _cityRepository,
            ISuburbRepository _suburb, IDistrictRepository _districtRepository,
            IMunicipalityRepository _municipalityRepo,
            IHooksRepository _hookRepository, IWebhookService _webHookService,
            IConfiguration configuration, IStreetRepository _streetRepository)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            this.serviceLogger = serviceLogger;
            countryRepository = _countryRepo;
            provinceRepository = _provinceRepository;
            cityRepository = _cityRepository;
            suburbRepository = _suburb;
            municipalityRepository = _municipalityRepo;
            districtRepository = _districtRepository;
            hookRepository = _hookRepository;
            config = configuration;
            streetRepository = _streetRepository;
            errorResponses = new ErrorResponses();


        }

        [HttpGet("GetAddress")]
        [ProducesResponseType(typeof(ViewAddressDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        public IActionResult GetAddress(Guid addressId)
        {
            try
            {
                var addressObj = _addressRepository.GetAddress(addressId);

                if (addressObj == null)
                {
                    return NotFound();
                }

                var addressDTO = new ViewAddressDTO
                {
                    Id = addressObj.Id,
                    Country = addressObj.Country?.Designation,
                    CountryID = addressObj.CountryID ?? Guid.Empty,
                    Province = addressObj.Province?.Designation,
                    ProvinceID = addressObj.ProvinceID ?? Guid.Empty,
                    Municipality = addressObj.Municipality?.Designation,
                    MunicipalityID = addressObj.MunicipalityID ?? Guid.Empty,
                    Suburb = addressObj.Suburb?.Designation,
                    SuburbId = addressObj.SuburbId ?? Guid.Empty,
                    StreetName = addressObj.Street?.Designation,
                    StreetID = addressObj.StreetID ?? Guid.Empty,
                    StreetNumber = addressObj.StreetNumber,
                    FullAddress = addressObj.StreetNumber + " " + addressObj.Street?.Designation + ", " + addressObj.Suburb?.Designation + ", " + addressObj.Province?.Designation
                };

                return Ok(addressDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Error while retrieving address " + ex.Message);
                ModelState.AddModelError("error", "failed to retrieve address");
                return BadRequest(ModelState);
            }
        }


        [HttpGet("StreetAddress")]
        [ProducesResponseType(typeof(List<ViewAddressDataTableDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetStreetAddress(Guid streetId)
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
                        Suburb = addressObj.Suburb.Designation,
                        StreetName = addressObj.Street.Designation,
                        Province = addressObj.Province.Designation,
                        FullAddress = addressObj.StreetNumber + " " + addressObj.Street.Designation + ", " + addressObj.Suburb.Designation + ", " + addressObj.Province.Designation
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


        [HttpGet("SuburbAddress")]
        [ProducesResponseType(typeof(List<ViewAddressDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetSuburbAddress(Guid suburbId)
        {

            try
            {
                var addressObjList = _addressRepository.GetAddressesInSuburb(suburbId);

                var viewAddressDTOList = new List<ViewAddressDTO>();



                foreach (var addressObj in addressObjList)
                {

                    var addressDTO = new ViewAddressDTO
                    {
                        Id = addressObj.Id,
                        Municipality = addressObj.Municipality.Designation,
                        Suburb = addressObj.Suburb.Designation,
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
        [ProducesResponseType(typeof(List<ViewAddressDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
                        Suburb = addressObj.Suburb.Designation,
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


        [HttpGet("GetGeolocationAddress")]
        [ProducesResponseType(typeof(FullAddress_DTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGeolocationAddress(int distance, decimal latitude, decimal longitude)
        {
            try
            {
                externalHttpService = new ExternalHttpService(config);
                var nominatimAddressDTO = await externalHttpService.RetrieveLocationByGeoLocationAsync(latitude, longitude);

                var fullAddress = new FullAddress_DTO();

                fullAddress.City = (nominatimAddressDTO.Address.City != null) ? nominatimAddressDTO.Address.City : (nominatimAddressDTO.Address.Town != null) ? nominatimAddressDTO.Address.Town: "";
                fullAddress.Suburb= (nominatimAddressDTO.Address.Suburb!= null) ? nominatimAddressDTO.Address.Suburb: "";
                fullAddress.District = (nominatimAddressDTO.Address.City_District!= null) ? nominatimAddressDTO.Address.City_District: (nominatimAddressDTO.Address.Suburb != null) ? nominatimAddressDTO.Address.Suburb :"";
                fullAddress.Country= (nominatimAddressDTO.Address.Country!= null) ? nominatimAddressDTO.Address.Country: "";
                fullAddress.Province = (nominatimAddressDTO.Address.State != null) ? nominatimAddressDTO.Address.State : "";
                fullAddress.CountryCode = (nominatimAddressDTO.Address.CountryCode != null) ? nominatimAddressDTO.Address.CountryCode : "";
                fullAddress.FullAddress = nominatimAddressDTO.DisplayName;

                return Ok(fullAddress);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        ///Looks up a lat/lng against Nominatim and returns the result shaped as a ViewAddressDTO.
        ///This does not touch the database - it's a preview only, nothing is created or linked.
        [HttpGet("PreviewAddressFromNominatim")]
        [ProducesResponseType(typeof(ViewAddressDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PreviewAddressFromNominatim(decimal latitude, decimal longitude)
        {
            try
            {
                externalHttpService = new ExternalHttpService(config);
                var nominatimAddressDTO = await externalHttpService.RetrieveLocationByGeoLocationAsync(latitude, longitude);

                if (nominatimAddressDTO?.Address == null)
                {
                    return BadRequest("No address could be resolved for the given coordinates");
                }

                var addressDTO = new ViewAddressDTO
                {
                    Country = nominatimAddressDTO.Address.Country ?? "",
                    Province = nominatimAddressDTO.Address.Province ?? nominatimAddressDTO.Address.State ?? "",
                    Municipality = nominatimAddressDTO.Address.Municipality ?? nominatimAddressDTO.Address.County ?? "",
                    Suburb = nominatimAddressDTO.Address.Suburb ?? nominatimAddressDTO.Address.City_District ?? "",
                    StreetName = nominatimAddressDTO.Address.Road ?? "",
                    StreetNumber = nominatimAddressDTO.Address.HouseNumber ?? "",
                    FullAddress = nominatimAddressDTO.DisplayName,
                    Coordinates = new BaseCoordinatesDTO { lat = latitude, lng = longitude }
                };

                return Ok(addressDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Error while previewing address from Nominatim " + ex.Message);
                return BadRequest("Failed to retrieve address from Nominatim");
            }
        }

        // [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("CreateAddress")]
        [ProducesResponseType(typeof(CreateAddressDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAddress(CreateAddressDTO addressDTO)
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

                if (!await ResolveStreetIdAsync(addressDTO))
                {
                    return StatusCode(500, ModelState);
                }

                var addressObj = _mapper.Map<AddressModel>(addressDTO);


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

        // [Authorize(Roles = "Admin")]
        [HttpPatch]
        [Route("UpdateAddress")]
        [ProducesResponseType(typeof(CreateAddressDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAddress([FromBody] CreateAddressDTO addressDTO, Guid addressId)
        {
            try
            {
                if (addressDTO == null || addressDTO.Id != addressId)
                {
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var addressObj = _addressRepository.GetAddress(addressId);

                if (addressObj == null)
                {
                    return NotFound();
                }

                if (!await ResolveStreetIdAsync(addressDTO))
                {
                    return StatusCode(500, ModelState);
                }

                addressObj.CountryID = addressDTO.CountryId;
                addressObj.ProvinceID = addressDTO.ProvinceID;
                addressObj.MunicipalityID = addressDTO.MunicipalityID;
                addressObj.SuburbId = addressDTO.SuburbId;
                addressObj.StreetID = addressDTO.StreetID;
                addressObj.StreetNumber = addressDTO.StreetNumber;
                addressObj.AdditionalInfo = addressDTO.AdditionalInfo;
                addressObj.BuildingTypeId = addressDTO.BuildingTypeId;

                if (!_addressRepository.UpdateAddress(addressObj))
                {
                    ModelState.AddModelError("", "Something went wrong while updating the address");

                    return StatusCode(500, ModelState);
                }

                return Ok(addressDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Error while updating address " + ex.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// When a streetName is present, resolves it to a street (reusing an existing one if
        /// the trimmed, case-insensitive name already exists, otherwise creating it), links it
        /// to the address's suburb if not already linked, and writes the resolved id into
        /// addressDTO.StreetID. Shared by CreateAddress and UpdateAddress so both follow the
        /// same "never duplicate a street name, especially within one suburb" rule. Returns
        /// false (with a ModelState error added) if a create/link step fails.
        /// </summary>
        private async Task<bool> ResolveStreetIdAsync(CreateAddressDTO addressDTO)
        {
            if (string.IsNullOrWhiteSpace(addressDTO.StreetName))
            {
                return true;
            }

            var streetName = addressDTO.StreetName.Trim();

            var street = streetRepository.VerifyStreetExistance(streetName);

            if (street == null)
            {
                street = new StreetModel { Designation = streetName };

                if (await streetRepository.CreateStreetModel(street) == null)
                {
                    ModelState.AddModelError("", "Something went wrong while creating the street");
                    return false;
                }
            }

            if (!streetRepository.VerifyStreetSuburbLink(street.Id, addressDTO.SuburbId))
            {
                var suburbStreetLink = new SuburbStreets { StreetsID = street.Id, SuburbId = addressDTO.SuburbId };

                if (!await streetRepository.CreateSuburbStreetsLink(suburbStreetLink))
                {
                    ModelState.AddModelError("", "Something went wrong while linking the street to the suburb");
                    return false;
                }
            }

            addressDTO.StreetID = street.Id;
            return true;
        }


        [HttpPost]
        [Route("CreateCountryDemographicDivision")]
        [ProducesResponseType(typeof(NominatimAddressDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateLocationEntity(NominatimAddressDTO nominatimAddressDTO)
        {

            try
            {
                Guid activeCountryId = Guid.Empty, activeProvinceId = Guid.Empty, activeMunicipalityId = Guid.Empty, activeCityId = Guid.Empty, activeDisctrictId = Guid.Empty, activeSuburb = Guid.Empty;

                var country = countryRepository.CountryExistis(nominatimAddressDTO.Address.Country);

                //
                //COUNTRY
                //Create Country if it doesn't exist
                //If it does exist select it.
                #region Country manipulation
                if (country == null)
                {
                    country = new CountryModel
                    {
                        Designation = nominatimAddressDTO.Address.Country
                    };

                    if (await countryRepository.CreateCountry(country) == null)
                    {
                        throw new Exception("Failed to create location attributes", new Exception("Failed to create a new country in "));
                    }
                }
                #endregion

                #region Province

                var currentProvinceOBJ = (nominatimAddressDTO.Address.Province != null) ? nominatimAddressDTO.Address.Province : (nominatimAddressDTO.Address.State != null) ? nominatimAddressDTO.Address.State : nominatimAddressDTO.Address.City;

                var province = provinceRepository.VerifyProvinceExist(currentProvinceOBJ, country.Id);

                if (province == null)
                {
                    //First need to check which of the subdivision of a province was provided.
                    /*
                     * if all were provided then use all elese just use the ones provided as othere could be kept as null.
                     * #CITY or STATE
                     * #Municipality or County
                     * #District
                     * **/

                    province = new ProvinceModel
                    {
                        CountryId = country.Id,
                        Designation = currentProvinceOBJ,
                    };

                    if (await provinceRepository.CreateProvinceModel(province) == null)
                    {
                        //Add Error.
                    }
                }
                #endregion

                #region City Manipulation
                if (nominatimAddressDTO.Address.City != null)
                {
                    //Check and Create City Existence.
                    //
                    var city = cityRepository.CityExists(province.Id, nominatimAddressDTO.Address.City);

                    //Check If this city doesn't exist create it

                    if (city == null)
                    {
                        var newCity = new CityModel
                        {
                            Designation = nominatimAddressDTO.Address.City,
                            ProvinceId = province.Id
                        };

                        if (await cityRepository.CreateCity(newCity) != null)
                        {
                            activeCityId = newCity.Id;
                        }
                        else
                        {
                            // report the error to the some log

                        }
                    }
                    else
                    {
                        //City does exist
                        activeCityId = city.Id;
                    }
                }
                #endregion

                #region District Manipulation 
                if (nominatimAddressDTO.Address.City_District != null)
                {
                    var district = districtRepository.VerifyDistrictExist(nominatimAddressDTO.Address.City_District, country.Id);

                    if (district == null)
                    {
                        var newDistrict = new DistrictModel
                        {
                            Designation = nominatimAddressDTO.Address.City_District,
                            ProvinceId = province.Id
                        };

                        if (await districtRepository.CreateDistrictModel(newDistrict) != null)
                        {
                            activeDisctrictId = newDistrict.Id;
                        }
                        else
                        {
                            // report the error to the some log

                        }
                    }
                    else
                    {
                        activeDisctrictId = district.Id;
                    }
                }
                #endregion


                #region Municipality Manipulation

                var currentMunicipality = (nominatimAddressDTO.Address.Municipality != null) ? nominatimAddressDTO.Address.Municipality : nominatimAddressDTO.Address.County;

                if (currentMunicipality != null)
                {
                    var municipality = municipalityRepository.VerifyMunicipalityExist(currentMunicipality, province.Id);

                    if (municipality == null)
                    {
                        municipality = new MunicipalityModel
                        {
                            Designation = currentMunicipality,
                            ProvinceId = province.Id
                        };

                        if (await municipalityRepository.CreateMunicipalityModel(municipality) == null)
                        {
                            // Log an error
                        }

                    }
                    activeMunicipalityId = municipality.Id;
                }
                #endregion

                #region UpdateSuburb

                var suburb = new SuburbModel
                {
                    CityId = (activeCityId == Guid.Empty) ? null : activeCityId,
                    DistrictId = (activeDisctrictId == Guid.Empty) ? null : activeDisctrictId,
                    MunicipalityId = activeMunicipalityId,
                    Designation = (nominatimAddressDTO.Address.Suburb == null) ? nominatimAddressDTO.Address.Suburb : (nominatimAddressDTO.Address.City_District != null) ? nominatimAddressDTO.Address.City_District : (nominatimAddressDTO.Address.Town == null) ? nominatimAddressDTO.Address.City : nominatimAddressDTO.Address.Town

                };


                //Check if this Suburb doesn't exist
                if (activeMunicipalityId != Guid.Empty && suburbRepository.VerifySuburbExist(nominatimAddressDTO.Address.Suburb, activeMunicipalityId, activeCityId, activeDisctrictId) == null)
                {
                    //suburb doesn't exist let's create it
                    if (await suburbRepository.CreateSuburb(suburb) == null)
                    {

                    }
                }




                #endregion


                #region Update Client Web Hook
                //Update the client webhook
                //var clientWebHook = hookRepository.GetHookByDesignation(nominatimAddressDTO.HookName, "");

                //var hookObj = new 
                //{
                //    suburbId=activeSuburb,
                //    municipalityId =activeMunicipalityId,

                //};

                #endregion

                return Ok(nominatimAddressDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Error while creating new address " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("CreateDivisionsByGeolocation")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDemographicByGeolocation(AddressGeolocationDTO addressGeolocationDTO)
        {
            try
            {

                
                //Make Nominati API CALL
                externalHttpService = new ExternalHttpService(config);
                var nominatimAddressDTO = await externalHttpService.RetrieveLocationByGeoLocationAsync(addressGeolocationDTO.Lat, addressGeolocationDTO.Lng);

                Guid activeCountryId = Guid.Empty, activeProvinceId = Guid.Empty, activeMunicipalityId = Guid.Empty, activeCityId = Guid.Empty, activeDisctrictId = Guid.Empty, activeSuburb = Guid.Empty;


                var country = countryRepository.CountryExistis(nominatimAddressDTO.Address.Country);

                if (addressGeolocationDTO == null)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {

                    return BadRequest(ModelState);
                }


                //
                //COUNTRY
                //Create Country if it doesn't exist
                //If it does exist select it.
                #region Country manipulation
                if (country == null)
                {
                    country = new CountryModel
                    {
                        Designation = nominatimAddressDTO.Address.Country
                    };

                    if (await countryRepository.CreateCountry(country) == null)
                    {
                        throw new Exception("Failed to create location attributes", new Exception("Failed to create a new country in "));
                    }
                }
                #endregion

                #region Province

                var currentProvinceOBJ = (nominatimAddressDTO.Address.Province != null) ? nominatimAddressDTO.Address.Province : (nominatimAddressDTO.Address.State != null) ? nominatimAddressDTO.Address.State : nominatimAddressDTO.Address.City;

                var province = provinceRepository.VerifyProvinceExist(currentProvinceOBJ, country.Id);

                if (province == null)
                {
                    //First need to check which of the subdivision of a province was provided.
                    /*
                     * if all were provided then use all elese just use the ones provided as othere could be kept as null.
                     * #CITY or STATE
                     * #Municipality or County
                     * #District
                     * **/

                    province = new ProvinceModel
                    {
                        CountryId = country.Id,
                        Designation = currentProvinceOBJ,
                    };

                    if (await provinceRepository.CreateProvinceModel(province) == null)
                    {
                        //Add Error.
                    }
                }
                #endregion
                //Check if the City exists. That is binded to this endpoint. 

                #region City Manipulation
                if (nominatimAddressDTO.Address.City != null)
                {
                    //Check and Create City Existence.
                    //
                    var city = cityRepository.CityExists(province.Id, nominatimAddressDTO.Address.City);

                    //Check If this city doesn't exist create it

                    if (city == null)
                    {
                        var newCity = new CityModel
                        {
                            Designation = nominatimAddressDTO.Address.City,
                            ProvinceId = province.Id
                        };

                        if (await cityRepository.CreateCity(newCity) != null)
                        {
                            activeCityId = newCity.Id;
                        }
                        else
                        {
                            // report the error to the some log

                        }
                    }
                    else
                    {
                        //City does exist
                        activeCityId = city.Id;
                    }
                }
                #endregion

                #region District Manipulation 
                if (nominatimAddressDTO.Address.City_District != null)
                {
                    var district = districtRepository.VerifyDistrictExist(nominatimAddressDTO.Address.City_District, country.Id);

                    if (district == null)
                    {
                        var newDistrict = new DistrictModel
                        {
                            Designation = nominatimAddressDTO.Address.City_District,
                            ProvinceId = province.Id
                        };

                        if (await districtRepository.CreateDistrictModel(newDistrict) != null)
                        {
                            activeDisctrictId = newDistrict.Id;
                        }
                        else
                        {
                            // report the error to the some log

                        }
                    }
                    else
                    {
                        activeDisctrictId = district.Id;
                    }
                }
                #endregion


                #region Municipality Manipulation

                var currentMunicipality = (nominatimAddressDTO.Address.Municipality != null) ? nominatimAddressDTO.Address.Municipality : nominatimAddressDTO.Address.County;

                if (currentMunicipality != null)
                {
                    var municipality = municipalityRepository.VerifyMunicipalityExist(currentMunicipality, province.Id);

                    if (municipality == null)
                    {
                        municipality = new MunicipalityModel
                        {
                            Designation = currentMunicipality,
                            ProvinceId = province.Id
                        };

                        if (await municipalityRepository.CreateMunicipalityModel(municipality) == null)
                        {
                            // Log an error
                        }

                    }
                    activeMunicipalityId = municipality.Id;
                }
                #endregion

                #region UpdateSuburb

                //Verify that Suburb Exists.
                var suburb = suburbRepository.VerifySuburbExist(nominatimAddressDTO.Address.Suburb, activeMunicipalityId, activeCityId, activeDisctrictId);

                if (suburb == null)
                {
                    suburb = new SuburbModel
                    {
                        CityId = (activeCityId == Guid.Empty) ? null : activeCityId,
                        DistrictId = (activeDisctrictId == Guid.Empty) ? null : activeDisctrictId,
                        MunicipalityId = activeMunicipalityId,
                        Designation = (nominatimAddressDTO.Address.Suburb != null) ? nominatimAddressDTO.Address.Suburb : (nominatimAddressDTO.Address.City_District != null) ? nominatimAddressDTO.Address.City_District : (nominatimAddressDTO.Address.Town == null) ? nominatimAddressDTO.Address.City : nominatimAddressDTO.Address.Town

                    };

                    if (activeMunicipalityId != Guid.Empty && await suburbRepository.CreateSuburb(suburb) == null)
                    {
                        //Log something here. Information Logs For a particular suburb.
                    }
                }



                activeSuburb = suburb.Id;


                #endregion


                #region Update Client Web Hook
                //Update the client webhook
                var clientWebHook = hookRepository.GetHookByDesignation(addressGeolocationDTO.HookName);

                if (clientWebHook == null)
                {
                    serviceLogger.LogError($"No webhook registered under the name '{addressGeolocationDTO.HookName}'");
                    return BadRequest($"No webhook registered under the name '{addressGeolocationDTO.HookName}'");
                }

                var hookObj = new
                {
                    id = addressGeolocationDTO.responseId,
                    suburbId = activeSuburb,
                    municipalityId = activeMunicipalityId,
                    city = activeCityId,
                    SuburbName = nominatimAddressDTO.Address.Suburb

                };

                // If the registered endpoint ends with a dangling '=' (e.g. "...&id="), it's expecting an id appended to complete the URL.
                if (clientWebHook.EndPoint.EndsWith('=') && addressGeolocationDTO.OptionalId != null)
                {
                    clientWebHook.EndPoint += addressGeolocationDTO.OptionalId;
                }


                //Update the
                var result = await externalHttpService.ApiRequest(hookObj, clientWebHook.EndPoint, clientWebHook.Method);


                return Ok(result);

                #endregion

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Error creating divisions by geolocation " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("CreateLocationByGeoposition")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDemographicByGeolocation([FromBody] BaseCoordinatesDTO baseCordinates)
        {
            try
            {
                externalHttpService = new ExternalHttpService(config);
                var nominatimAddressDTO = await externalHttpService.RetrieveLocationByGeoLocationAsync(baseCordinates.lat, baseCordinates.lng);

                Guid activeCountryId = Guid.Empty, activeProvinceId = Guid.Empty, activeMunicipalityId = Guid.Empty, activeCityId = Guid.Empty, activeDisctrictId = Guid.Empty, activeSuburb = Guid.Empty;


                var country = countryRepository.CountryExistis(nominatimAddressDTO.Address.Country);

                if (baseCordinates == null)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {

                    return BadRequest(ModelState);
                }


                //
                //COUNTRY
                //Create Country if it doesn't exist
                //If it does exist select it.
                #region Country manipulation
                if (country == null)
                {
                    country = new CountryModel
                    {
                        Designation = nominatimAddressDTO.Address.Country
                    };

                    if (await countryRepository.CreateCountry(country) == null)
                    {
                        throw new Exception("Failed to create location attributes", new Exception("Failed to create a new country in "));
                    }
                }
                #endregion

                #region Province

                var currentProvinceOBJ = (nominatimAddressDTO.Address.Province != null) ? nominatimAddressDTO.Address.Province : (nominatimAddressDTO.Address.State != null) ? nominatimAddressDTO.Address.State : nominatimAddressDTO.Address.City;

                var province = provinceRepository.VerifyProvinceExist(currentProvinceOBJ, country.Id);

                if (province == null)
                {
                    //First need to check which of the subdivision of a province was provided.
                    /*
                     * if all were provided then use all elese just use the ones provided as othere could be kept as null.
                     * #CITY or STATE
                     * #Municipality or County
                     * #District
                     * **/

                    province = new ProvinceModel
                    {
                        CountryId = country.Id,
                        Designation = currentProvinceOBJ,
                    };

                    if (await provinceRepository.CreateProvinceModel(province) == null)
                    {
                        //Add Error.
                    }
                }
                #endregion

                #region City Manipulation

                var extractCity = (nominatimAddressDTO.Address.City != null) ? nominatimAddressDTO.Address.City : nominatimAddressDTO.Address.State;

                

                if (extractCity != null)
                {
                    //Check and Create City Existence.
                    //
                    var city = cityRepository.CityExists(province.Id, extractCity);

                    //Check If this city doesn't exist create it

                    if (city == null)
                    {
                        var newCity = new CityModel
                        {
                            Designation = extractCity,
                            ProvinceId = province.Id
                        };

                        if (await cityRepository.CreateCity(newCity) != null)
                        {
                            activeCityId = newCity.Id;
                        }
                        else
                        {
                            // report the error to the some log

                        }
                    }
                    else
                    {
                        //City does exist
                        activeCityId = city.Id;
                    }
                }
                else {

                    //Use it to create a town 

                   var  city = cityRepository.CityExists(province.Id, nominatimAddressDTO.Address.Town);

                    //Check If this city doesn't exist create it

                    if (city == null)
                    {
                        var newCity = new CityModel
                        {
                            Designation = nominatimAddressDTO.Address.Town,
                            ProvinceId = province.Id
                        };

                        if (await cityRepository.CreateCity(newCity) != null)
                        {
                            activeCityId = newCity.Id;
                        }
                        else
                        {
                            // report the error to the some log

                        }
                    }
                    else
                    {
                        //City does exist
                        activeCityId = city.Id;
                    }

                }
                #endregion

                #region District Manipulation 
                if (nominatimAddressDTO.Address.City_District != null)
                {
                    var district = districtRepository.VerifyDistrictExist(nominatimAddressDTO.Address.City_District, country.Id);

                    if (district == null)
                    {
                        var newDistrict = new DistrictModel
                        {
                            Designation = nominatimAddressDTO.Address.City_District,
                            ProvinceId = province.Id
                        };

                        if (await districtRepository.CreateDistrictModel(newDistrict) != null)
                        {
                            activeDisctrictId = newDistrict.Id;
                        }
                        else
                        {
                            // report the error to the some log

                        }
                    }
                    else
                    {
                        activeDisctrictId = district.Id;
                    }
                }
                #endregion


                #region Municipality Manipulation

                var currentMunicipality = (nominatimAddressDTO.Address.Municipality != null) ? nominatimAddressDTO.Address.Municipality : nominatimAddressDTO.Address.County;

                if (currentMunicipality != null)
                {
                    var municipality = municipalityRepository.VerifyMunicipalityExist(currentMunicipality, province.Id);

                    if (municipality == null)
                    {
                        municipality = new MunicipalityModel
                        {
                            Designation = currentMunicipality,
                            ProvinceId = province.Id
                        };

                        if (await municipalityRepository.CreateMunicipalityModel(municipality) == null)
                        {
                            // Log an error
                        }

                    }
                    activeMunicipalityId = municipality.Id;
                }
                #endregion

                #region UpdateSuburb

                var extractSuburb = (nominatimAddressDTO.Address.Suburb != null) ? nominatimAddressDTO.Address.Suburb : (nominatimAddressDTO.Address.Town != null)? 
                    nominatimAddressDTO.Address.Town: (nominatimAddressDTO.Address.County != null) ? nominatimAddressDTO.Address.County : (nominatimAddressDTO.Address.Municipality != null) ? nominatimAddressDTO.Address.Municipality :nominatimAddressDTO.Address.Suburb;

                extractSuburb = (extractSuburb == null)? nominatimAddressDTO.Address.City : extractSuburb;

                //Verify that Suburb Exists.
                var suburb = suburbRepository.VerifySuburbExist(extractSuburb, activeMunicipalityId, activeCityId, activeDisctrictId);

                if (suburb == null)
                {
                    suburb = new SuburbModel
                    {
                        CityId = (activeCityId == Guid.Empty) ? null : activeCityId,
                        DistrictId = (activeDisctrictId == Guid.Empty) ? null : activeDisctrictId,
                        MunicipalityId = activeMunicipalityId,
                        Designation = extractSuburb,
                        ReviewName = true

                    };

                    if (activeMunicipalityId != Guid.Empty && await suburbRepository.CreateSuburb(suburb) == null)
                    {
                        //Log something here. Information Logs For a particular suburb.
                    }
                }

                #endregion

                return StatusCode(StatusCodes.Status201Created,"Coordinates mapped out successfully");

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Error creating location by position " + ex.Message);
                return BadRequest();
            }


        }
    }
     
}
