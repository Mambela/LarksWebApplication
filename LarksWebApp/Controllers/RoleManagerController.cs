using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers
{
    public class RoleManagerController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILoggerTrace serviceLogger;

        public RoleManagerController(RoleManager<IdentityRole> roleManager,ILoggerTrace _servicelogger)
        {
            serviceLogger = _servicelogger;
            _roleManager = roleManager;

        }


        // GET: RoleManagerController
        public ActionResult Index()
        {
             
            var roles = _roleManager.Roles.ToList();


            return View(roles);
        }

        // GET: RoleManagerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RoleManagerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoleManagerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRole(string  roleName)
        {
            try
            {
                if (roleName != null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
                }


                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                serviceLogger.LogError("Creating a role error" + ex.Message);
                return View();

            }
        }

        // GET: RoleManagerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RoleManagerController/Edit/5
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

        // GET: RoleManagerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RoleManagerController/Delete/5
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
