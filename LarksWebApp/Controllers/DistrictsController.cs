using AutoMapper;
using LarksWebApp.DTOs.DTO_Districts;
using LarksWebApp.Models;
using LarksWebApp.Repository.DistrictRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers
{
    public class DistrictsController : Controller
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;

        public DistrictsController(IDistrictRepository districtRepository, IMapper mapper,ILoggerTrace logger)
        {
            _districtRepository = districtRepository;
            _mapper = mapper;   
            serviceLogger = logger; 
        }

        // GET: DisctrictController
        public ActionResult Index(Guid provinceId)
        {
            try
            {
                var districtListObj = _districtRepository.GetDistrictModels(provinceId);

                var disctrictListDTO = new List<DistrictDTO>();

                for (int i = 0; i < districtListObj.Count; i++)
                {
                    disctrictListDTO.Add(_mapper.Map<DistrictDTO>(districtListObj[i]));
                }

                TempData["provinceID"] = provinceId;

                return View(disctrictListDTO);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return View();
            }
           
        }

        // GET: DisctrictController/Details/5
        public ActionResult Details(Guid id)
        {
            return View();
        }

        // GET: DisctrictController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DisctrictController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    throw new Exception("Invalid Model State");
                }

                var districtModel = new DistrictModel()
                {
                    ProvinceId = Guid.Parse(TempData["provinceID"].ToString()),
                    Designation = collection["Designation"]
                };

                if (_districtRepository.CreateDistrictModel(districtModel) != null)
                {
                    return RedirectToAction(nameof(Index), new { msg = $"District {districtModel.Designation} saved successfully! " });
                }


                return RedirectToAction(nameof(Index), new { msg = $"Error while saving District {districtModel.Designation} " });

            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return View();
            }
        }

        // GET: DisctrictController/Edit/5
        public ActionResult Edit(Guid id)
        {
            return View();
        }

        // POST: DisctrictController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
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

        // GET: DisctrictController/Delete/5
        public ActionResult Delete(Guid id)
        {
            return View();
        }

        // POST: DisctrictController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
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
