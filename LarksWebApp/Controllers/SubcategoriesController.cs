using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers
{
    public class SubcategoriesController : Controller
    {
        // GET: SubcategoriesController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SubcategoriesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SubcategoriesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SubcategoriesController/Create
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

        // GET: SubcategoriesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SubcategoriesController/Edit/5
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

        // GET: SubcategoriesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SubcategoriesController/Delete/5
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
