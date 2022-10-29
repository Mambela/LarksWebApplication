using AutoMapper;
using LarksWebApp.DTOs.DTO_City;
using LarksWebApp.DTOs.DTO_Neighbourhoods;
using LarksWebApp.DTOs.DTO_Provinces;
using LarksWebApp.Models;
using LarksWebApp.Repository.HoodRepository;
using LarksWebApp.Repository.ProviceRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Dynamic;

namespace LarksWebApp.Controllers
{
    [Authorize]
    public class NeighbourhoodController : Controller
    {
        private readonly INeighbourhoodRepository _hoodRepository;
        private readonly IProvinceRepository _provinceRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;


        public NeighbourhoodController(INeighbourhoodRepository hoodRepository, IProvinceRepository provinceRepository, IMapper mapper, ILoggerTrace serviceLogger)
        {
            _hoodRepository = hoodRepository;
            _provinceRepository = provinceRepository;
            _mapper = mapper;
            this.serviceLogger = serviceLogger;
        }


        // GET: NeighbourhoodController
        public ActionResult Index(int provinId)
        {
            try
            {
                var NeighbourHoodObj = _hoodRepository.GetNeighbourhoodInMunicipality(provinId);

                var NeighbourHoodDTOList = new List<NeighbourhoodDTO>();

                for (int i = 0; i < NeighbourHoodObj.Count; i++)
                {
                    NeighbourHoodDTOList.Add(_mapper.Map<NeighbourhoodDTO>(NeighbourHoodObj[i]));
                }

                return View(NeighbourHoodDTOList);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return View();
                
            }

           
        }

        // GET: NeighbourhoodController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NeighbourhoodController/Create
        public ActionResult Create()
        {

            try
            {
                int provinceId = 0;
                if (TempData["provinceID"] == null)
                {
                    throw new Exception("There is a provice missing to query this neighbourhood");
                }
                else {
                    provinceId = Convert.ToInt32(TempData["provinceID"].ToString());
                }

                var provinceObj = _provinceRepository.GetProvinceModel(provinceId);

                if (provinceObj == null)
                {

                    return RedirectToAction(nameof(Index),"Country");
                }

                var createNeighbourhood = new Create_neighbourhood_DTO();

                var provinceDTO = _mapper.Map<ProvinceDTO>(provinceObj);

                createNeighbourhood.provinceDTO = provinceDTO;

                

                return View(createNeighbourhood);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Create neighborhood " + ex.Message);

                return RedirectToAction(nameof(Index), "Country");
            }
        }

        // POST: NeighbourhoodController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(IFormCollection neighbourhood)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    throw new Exception("Invalid Model State");
                }

                var neighbourhoodObj = new NeighbourhoodModel()
                {
                    CityId = Convert.ToInt32(neighbourhood["newCityId"]),
                    MunicipalityId = Convert.ToInt32(neighbourhood["newMunicipalityId"]),
                    DistrictId = Convert.ToInt32(neighbourhood["newDistrictId"]),
                    Designation =neighbourhood["Designation"]
                };


                if (await _hoodRepository.CreateNeighbourhood(neighbourhoodObj) == null)
                {
                    return RedirectToAction(nameof(Index), new { msg = $"Neighbourhood {neighbourhoodObj.Designation} saved successfully! " });
                }

                var countryId = TempData["countryID"].ToString();


                return RedirectToAction(nameof(Index), new { msg = $"Neighbourhood  {neighbourhoodObj.Designation} saved successfully" });
 
            }
            catch(Exception ex)
            {

                serviceLogger.LogError("Create a new Neighbourhood "+ex.Message);
                return RedirectToAction(nameof(Index), "Country");
            }
        }

        // GET: NeighbourhoodController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        public ActionResult sectorNeighbourhood(string sector, int id)
        {

            try
            {
                var hoodObjList = _hoodRepository.GetNeighbourhoodModels(sector, id);

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



        // POST: NeighbourhoodController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: NeighbourhoodController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NeighbourhoodController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
