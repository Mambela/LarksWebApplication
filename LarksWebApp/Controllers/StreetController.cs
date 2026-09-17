using AutoMapper;
using LarksWebApp.DTOs;
using LarksWebApp.DTOs.DTO_City;
using LarksWebApp.DTOs.DTO_Provinces;
using LarksWebApp.DTOs.DTO_Streets;
using LarksWebApp.Models;
using LarksWebApp.Models.CoordinatesModel;
using LarksWebApp.Repository.AddressRepository.StreetRepository;
using LarksWebApp.Repository.SuburbStreetRepository;
using LarksWebApp.Repository.CountryRepository;
using LarksWebApp.Repository.ProviceRepository;
using LarksWebApp.Repository.StreetRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers
{

    // [Authorize]
    public class StreetController : Controller
    {

        private readonly ICountryRepository _countryRepo;
        private readonly IStreetRepository _streetRepo;
        private readonly IStreetCoordinateRepository _streetCoordinateRepo;
        private readonly ISuburbStreetRepository _suburbStreetRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;
        private readonly IProvinceRepository _provinceRepository;
        public StreetController(ICountryRepository countryRepo, IStreetRepository streetRepo, IMapper mapper, ILoggerTrace loggerTrace, IStreetCoordinateRepository streetCoordinateRepo, IProvinceRepository provinceRepository)
        {
            _countryRepo = countryRepo;
            _streetRepo = streetRepo;
            _mapper = mapper;
            serviceLogger = loggerTrace;
            _streetCoordinateRepo = streetCoordinateRepo;
            _provinceRepository = provinceRepository;   
        }
        // GET: StreetController
        public ActionResult Index()
        {

            try
            {

                var countryList = _countryRepo.GetAll();

                var countryListDTO = new List<CountryDTO>();

                foreach (var country in countryList)
                {
                    countryListDTO.Add(_mapper.Map<CountryDTO>(country));

                }

                var viewCountryListDTO = new Create_City_DTO();

                viewCountryListDTO.ListofCountryDTO = countryListDTO;

                return View(viewCountryListDTO);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Error while attempting to fetch countries for address" + ex.Message);


                return BadRequest();
            }
        }

        // GET: StreetController/Details/5
        public ActionResult Details(Guid id)
        {
            return View();
        }

        public ActionResult StreetsInSuburb(Guid suburbId)
        {
            try
            {
                var suburbStreetObjList = _streetRepo.GetStreetsInSuburb(suburbId);

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
                serviceLogger.LogError("Retrieving streets in a Suburb " + ex.Message);
                return BadRequest();
            }
        }
        // GET: StreetController/Create
        public ActionResult Create()
        {

            try
            {
                var listofCountry = _countryRepo.GetAll();

                var listofCountryDTO = new List<CountryDTO>();

                listofCountryDTO = _mapper.Map<List<CountryDTO>>(listofCountry);

                var createStreetDTO = new Create_Street_DTO();

                createStreetDTO.ListofCountryDTO = listofCountryDTO;

                return View(createStreetDTO);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Street Controller getting the data" + ex.Message);

                return View();
            }
         
        }



        public ActionResult CountryProvinces(Guid CountryId)
        {
            try
            {
                var provincesObj = _provinceRepository.GetProvincesInCountry(CountryId);

                var proviceDTOList = new List<ProvinceDTO>();

                proviceDTOList = _mapper.Map<List<ProvinceDTO>>(provincesObj);

                return Ok(proviceDTOList);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Get Provinces in Country " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
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


                if (await _streetRepo.CreateStreetModel(streetObj) == null)
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


                return StatusCode(StatusCodes.Status201Created, streetDTO);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // POST: StreetController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StreetController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var streetObj = _streetRepo.GetStreetModel(id);

            if (streetObj == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return View(streetObj);
        }

        // POST: StreetController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                var streetObj = _streetRepo.GetStreetModel(id);

                if (streetObj == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                streetObj.Designation = collection["Designation"];

                _streetRepo.UpdateStreet(streetObj);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Edit street " + ex.Message);
                return View();
            }
        }

        // GET: StreetController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var streetObj = _streetRepo.GetStreetModel(id);

            if (streetObj == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return View(streetObj);
        }

        // POST: StreetController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                var streetObj = _streetRepo.GetStreetModel(id);

                if (streetObj != null)
                {
                    _streetRepo.DeleteStreet(streetObj);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Delete street " + ex.Message);
                return View();
            }
        }
    }
}
