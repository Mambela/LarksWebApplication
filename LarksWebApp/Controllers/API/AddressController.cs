using AutoMapper;
using LarksWebApp.DTOs;
using LarksWebApp.DTOs.DTO_Addresses;
using LarksWebApp.Models;
using LarksWebApp.Repository.AddressRepository.Address_Repository;
using LarksWebApp.Repository.AddressRepository.HooksRepository;
using LarksWebApp.Repository.CityRepository;
using LarksWebApp.Repository.CountryRepository;
using LarksWebApp.Repository.DistrictRepository;
using LarksWebApp.Repository.HoodRepository;
using LarksWebApp.Repository.MunicipalityRepository;
using LarksWebApp.Repository.ProviceRepository;
using LarksWebApp.Services.ExternalService;
using LarksWebApp.Services.Loggers;
using LarksWebApp.Services.WebHooks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public AddressController(
            IAddressRepository addressRepository, IMapper mapper, 
            ILoggerTrace serviceLogger,ICountryRepository _countryRepo,
            IProvinceRepository _provinceRepository,ICityRepository _cityRepository,
            ISuburbRepository _neighbourhood, IDistrictRepository _districtRepository,
            IMunicipalityRepository  _municipalityRepo,
            IHooksRepository _hookRepository,IWebhookService _webHookService,
            IConfiguration configuration)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            this.serviceLogger = serviceLogger;
            countryRepository = _countryRepo;
            provinceRepository = _provinceRepository;
            cityRepository = _cityRepository;
            suburbRepository = _neighbourhood;
            municipalityRepository = _municipalityRepo;
            districtRepository = _districtRepository;
            hookRepository = _hookRepository;
            config = configuration;
            errorResponses = new ErrorResponses();


        }

        [HttpGet("GetAddress")]
        public IActionResult GetAddress(int addressId)
        {
            try
            { 

                var addressObj = _addressRepository.GetAddress(addressId);
                if (addressObj == null)
                {
                    return NotFound();
                }

                var addressDTO = new ViewAddressDataTableDTO
                {
                    Id = addressObj.Id,
                    Municipality = addressObj.Municipality.Designation,
                    Neighbourhood = addressObj.Neighbourhood.Designation,
                    StreetName = addressObj.Street.Designation,
                    Province = addressObj.Province.Designation,
                    FullAddress = addressObj.StreetNumber + " " + addressObj.Street.Designation + ", " + addressObj.Neighbourhood.Designation + ", " + addressObj.Province.Designation
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
                        FullAddress = addressObj.StreetNumber + " " + addressObj.Street.Designation + ", " + addressObj.Neighbourhood.Designation + ", " + addressObj.Province.Designation
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
                var addressObjList = _addressRepository.GetAddressesInNeighbourhood(neighbourhoodId);

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
        [Route("CreateAddress")]
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


        [HttpPost]
        [Route("CreateCountryDemographicDivision")]
        public async Task<IActionResult> CreateLocationEntity(NominatimAddressDTO nominatimAddressDTO)
        {
             
            try
            {
                int activeCountryId = 0, activeProvinceId = 0, activeMunicipalityId = 0, activeCityId = 0, activeDisctrictId = 0, activeSuburb = 0;

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

                var currentProvinceOBJ = (nominatimAddressDTO.Address.Province != null) ? nominatimAddressDTO.Address.Province : (nominatimAddressDTO.Address.State !=null) ? nominatimAddressDTO.Address.State : nominatimAddressDTO.Address.City;

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
                }
                #endregion


                #region Municipality Manipulation

                var currentMunicipality = (nominatimAddressDTO.Address.Municipality != null) ? nominatimAddressDTO.Address.Municipality : nominatimAddressDTO.Address.county;

                if (currentMunicipality != null)
                {
                    var municipality = municipalityRepository.VerifyMunicipalityExist(currentMunicipality, country.Id);

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
                    CityId = (activeCityId == 0) ? null : activeCityId,
                    DistrictId = (activeDisctrictId == 0) ? null : activeDisctrictId,
                    MunicipalityId = (activeMunicipalityId == 0) ? null : activeMunicipalityId,
                    Designation = (nominatimAddressDTO.Address.Suburb == null) ? nominatimAddressDTO.Address.Suburb : (nominatimAddressDTO.Address.City_District !=null) ? nominatimAddressDTO.Address.City_District:(nominatimAddressDTO.Address.Town == null) ? nominatimAddressDTO.Address.City : nominatimAddressDTO.Address.Town

                };


                //Check if this Suburb doesn't exist 
                if (suburbRepository.VerifySuburbExist(nominatimAddressDTO.Address.Suburb,activeMunicipalityId,activeCityId,activeDisctrictId) ==null)
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
        public async Task<IActionResult> CreateDemographicByGeolocation(AddressGeolocationDTO addressGeolocationDTO)
        {
            try
            {

                //Make Nominati API CALL
                externalHttpService = new ExternalHttpService(config);
                var nominatimAddressDTO = await externalHttpService.RetrieveLocationByGeoLocationAsync(addressGeolocationDTO.Lat, addressGeolocationDTO.Lng);

                int activeCountryId = 0, activeProvinceId = 0, activeMunicipalityId = 0, activeCityId = 0, activeDisctrictId = 0, activeSuburb = 0;

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
                }
                #endregion


                #region Municipality Manipulation

                var currentMunicipality = (nominatimAddressDTO.Address.Municipality != null) ? nominatimAddressDTO.Address.Municipality : nominatimAddressDTO.Address.county;

                if (currentMunicipality != null)
                {
                    var municipality = municipalityRepository.VerifyMunicipalityExist(currentMunicipality, country.Id);

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
                    CityId = (activeCityId == 0) ? null : activeCityId,
                    DistrictId = (activeDisctrictId == 0) ? null : activeDisctrictId,
                    MunicipalityId = (activeMunicipalityId == 0) ? null : activeMunicipalityId,
                    Designation = (nominatimAddressDTO.Address.Suburb == null) ? nominatimAddressDTO.Address.Suburb : (nominatimAddressDTO.Address.City_District != null) ? nominatimAddressDTO.Address.City_District : (nominatimAddressDTO.Address.Town == null) ? nominatimAddressDTO.Address.City : nominatimAddressDTO.Address.Town

                };


                //Check if this Suburb doesn't exist 
                if (suburbRepository.VerifySuburbExist(nominatimAddressDTO.Address.Suburb, activeMunicipalityId, activeCityId, activeDisctrictId) == null)
                {
                    //suburb doesn't exist let's create it
                    if (await suburbRepository.CreateSuburb(suburb) == null)
                    {

                    }
                }

                activeSuburb = suburb.Id;


                #endregion


                #region Update Client Web Hook
                //Update the client webhook
                var clientWebHook = hookRepository.GetHookByDesignation(addressGeolocationDTO.HookName);

                var hookObj = new
                {
                    suburbId = activeSuburb,
                    municipalityId = activeMunicipalityId,

                };

                #endregion



                return Ok(nominatimAddressDTO);
            }
            catch (Exception ex)
            { 
                return BadRequest(); 
            }
        }

    }
}
