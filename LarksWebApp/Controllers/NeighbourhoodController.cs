using AutoMapper;
using LarksWebApp.DTOs.DTO_Neighbourhoods;
using LarksWebApp.Repository.HoodRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers
{
    public class NeighbourhoodController : Controller
    {
        private readonly INeighbourhoodRepository _hoodRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerTrace serviceLogger;
        // GET: NeighbourhoodController
        public ActionResult Index(int provinId)
        {
            try
            {
                var NeighbourHoodObj = _hoodRepository.GetNeighbourhoodModels(provinId);

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

            return View();
        }

        // GET: NeighbourhoodController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NeighbourhoodController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NeighbourhoodController/Create
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

        // GET: NeighbourhoodController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
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
