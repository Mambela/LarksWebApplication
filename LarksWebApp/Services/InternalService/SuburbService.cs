using LarksWebApp.DTOs.DTO_Addresses;
using LarksWebApp.Models;
using LarksWebApp.Models.CoordinatesModel;
using LarksWebApp.Repository.CityRepository;
using LarksWebApp.Repository.CountryRepository;
using LarksWebApp.Repository.DistrictRepository;
using LarksWebApp.Repository.SuburbRepository;
using LarksWebApp.Repository.MunicipalityRepository;
using LarksWebApp.Repository.ProviceRepository;
using LarksWebApp.Services.ExternalService;
using Newtonsoft.Json;
using Serilog;
using System.Text.Json;
using System.Xml.Serialization;

namespace LarksWebApp.Services.InternalService
{
    public class SuburbServiceForQueue
    {
        private readonly IConfiguration config;
        private ExternalHttpService externalHttpService;
        private readonly ICountryRepository countryRepository;
        private readonly IProvinceRepository provinceRepository;
        private readonly ICityRepository cityRepository;
        private readonly ISuburbRepository suburbRepository;
        private readonly IDistrictRepository districtRepository;
        private readonly IMunicipalityRepository municipalityRepository;

        

        public SuburbServiceForQueue(IConfiguration configuration,ICountryRepository countryRepo,
            IProvinceRepository provinceRepo,
            ICityRepository cityRepo,
            ISuburbRepository suburbRepo,
            IDistrictRepository districtRepo,
            IMunicipalityRepository municipalityRepo)
        {
            config = configuration;
            countryRepository = countryRepo;
            provinceRepository = provinceRepo;
            cityRepository = cityRepo;
            suburbRepository = suburbRepo;
            districtRepository = districtRepo;
            municipalityRepository = municipalityRepo;

        }

        public async Task<bool> ExtractLocationPayload(List<string> messageList,QueueService queueService) {
            try
            {

                if(messageList.Count <= 0)
                {
                    return true;
                }


                //Create an Empty  
                List<ReturnCoordinatesModel> coordinatesList = new List<ReturnCoordinatesModel>();

                foreach (var message in messageList)
                {

                    var tempLocationObjectList = JsonConvert.DeserializeObject<List<ReceivedCoordinatesModel>>(message);
                    

                    foreach (var itemLocation in tempLocationObjectList)
                    {
                       

                        var mappedLocation = await MapCoordinates(Decimal.Parse(itemLocation.Lat.ToString()), Decimal.Parse(itemLocation.Lng.ToString()),itemLocation.Id);

                        //Reconstruct the items to a new Object List 
                        coordinatesList.Add(new ReturnCoordinatesModel
                        {
                            Id = itemLocation.Id,
                            SuburbId = mappedLocation.SuburbId,
                            Lat = itemLocation.Lat,
                            Lng = itemLocation.Lng

                        });
                        //await Task.Delay(2000);
                    }


                    

                    

                    //string payload = System.Text.Json.JsonSerializer.Serialize<List<ReturnCoordinatesModel>>(coordinatesList);


                    //queueService.SendService(payload, "update_station", "Update_Test_PoliceStations_Queue");

                    //Declare a queueName

                    //Declare a payload

                    //Routing Key

                    //  queueService.SendService();

                }


                string payload = System.Text.Json.JsonSerializer.Serialize<List<ReturnCoordinatesModel>>(coordinatesList);
                queueService = new QueueService();

                queueService.SendService(payload, "update_station", "q.updatePoliceStations");


                return true;

            }
            catch (Exception ex)
            {

                //Log the error 

                return false;
            }
        }
       
        public async void ExtractLocationPayload(string receivedPayload) {
			try
			{
				//Clean the Project and Create a OBJECT
				var tempObjectList = JsonConvert.DeserializeObject<List<ReceivedCoordinatesModel>>(receivedPayload);


                //Pass the list to the  coordinates 
                List<ReturnCoordinatesModel> coordinatesList = new List<ReturnCoordinatesModel>();

                foreach (var item in tempObjectList)
                {
                    //Maps the Coordinates and Saves the items To the Database
                    var mappedLocation = await MapCoordinates(Decimal.Parse(item.Lat.ToString()), Decimal.Parse(item.Lng.ToString()), item.Id);

                    //Reconstruct the items to a new Object List 
                    coordinatesList.Add(new ReturnCoordinatesModel
                    {
                        Id = item.Id,
                        SuburbId = mappedLocation.SuburbId,
                        Lat = item.Lat,
                        Lng = item.Lng

                    });

                }


                var queueService = new QueueService();

                string payload = System.Text.Json.JsonSerializer.Serialize<List<ReturnCoordinatesModel>>(coordinatesList);


                queueService.SendService(payload,"update_station", "q.updatePoliceStations");


                //Send the new Object list to a new Queue


            }
			catch (Exception ex)
			{

				throw;
			}


		}
        
        async Task<MappedLocation> MapCoordinates(Decimal Lat, Decimal Lng,Guid Id) {
            try
			{

                if (Lat == 0 || Lng == 0) {
                    return null;

                }

                //SEND A HTTP CALL WITH THE COORDINATES
                externalHttpService = new ExternalHttpService(config);
                var nominatimAddressDTO = await externalHttpService.RetrieveLocationByGeoLocationAsync(Lat, Lng);

                Guid activeCountryId = Guid.Empty, activeProvinceId = Guid.Empty, activeMunicipalityId = Guid.Empty, activeCityId = Guid.Empty, activeDisctrictId = Guid.Empty, activeSuburb = Guid.Empty;

                if (nominatimAddressDTO.Address == null) {
                    
                    return new MappedLocation{
                    };
                 
                }

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

                var currentProvinceOBJ = (nominatimAddressDTO.Address.Province != null) ? nominatimAddressDTO.Address.Province : (nominatimAddressDTO.Address.State != null) ? nominatimAddressDTO.Address.State : nominatimAddressDTO.Address.Country;

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

                var currentMunicipality = (nominatimAddressDTO.Address.Municipality != null) ? nominatimAddressDTO.Address.Municipality : (nominatimAddressDTO.Address.County !=null) ? nominatimAddressDTO.Address.County:(nominatimAddressDTO.Address.State !=null) ? nominatimAddressDTO.Address.State :(nominatimAddressDTO.Address.Province != null) ? nominatimAddressDTO.Address.Province : nominatimAddressDTO.Address.Country;

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


                var nominatimSuburb = (nominatimAddressDTO.Address.Suburb != null) ? nominatimAddressDTO.Address.Suburb : (nominatimAddressDTO.Address.City_District != null) ? nominatimAddressDTO.Address.City_District : (nominatimAddressDTO.Address.City != null) ? nominatimAddressDTO.Address.City : (nominatimAddressDTO.Address.Town != null) ? nominatimAddressDTO.Address.Town: (nominatimAddressDTO.Address.Municipality != null) ? nominatimAddressDTO.Address.Municipality:(nominatimAddressDTO.Address.County !=null) ? nominatimAddressDTO.Address.County : (nominatimAddressDTO.Address.State!=null)? nominatimAddressDTO.Address.State : (nominatimAddressDTO.Address.Province != null)? nominatimAddressDTO.Address.Province: nominatimAddressDTO.Address.Country;

                //if ((nominatimSuburb == null && activeCityId == 0 ) || (nominatimSuburb ==null || activeMunicipalityId==0) || (nominatimSuburb ==null || activeDisctrictId==0)) {
                //        return new MappedLocation
                //        {
                //        };
 
                //}


                //Verify that Suburb Exists.
                //var suburb = suburbRepository.VerifySuburbExist(nominatimAddressDTO.Address.Suburb, activeMunicipalityId, activeCityId, activeDisctrictId);
                var suburb = suburbRepository.VerifySuburbExist(nominatimSuburb, activeMunicipalityId, activeCityId, activeDisctrictId);

                if (suburb == null)
                {
                    suburb = new SuburbModel
                    {
                        CityId = (activeCityId == Guid.Empty) ? null : activeCityId,
                        DistrictId = (activeDisctrictId == Guid.Empty) ? null : activeDisctrictId,
                        MunicipalityId = activeMunicipalityId,
                        Designation = nominatimSuburb

                    };

                    if (activeMunicipalityId != Guid.Empty && await suburbRepository.CreateSuburb(suburb) == null)
                    {
                        //Log something here. Information Logs For a particular suburb.
                    }
                }



                activeSuburb = suburb.Id;

                var mappedLocation = new MappedLocation
                {
                    Id = Guid.Empty,
                    CityId =activeCityId,
                    SuburbId = activeSuburb,
                    MunicipalityId = activeMunicipalityId
                };
                #endregion

                return mappedLocation;
            }
            catch (Exception ex)
            {
                throw ex;
			}
        }
 

    }
}
