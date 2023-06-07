using LarksWebApp.Models.Users;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LarksWebApp.Controllers
{
    public class UserRolesController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILoggerTrace _serviceLogger;
        public UserRolesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILoggerTrace serviceLogger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _serviceLogger = serviceLogger;
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Index()
        {

            try
            {
                var users = _userManager.Users.ToList();
                var userRolesViewModel = new List<UserRolesViewModel>();
                foreach (ApplicationUser user in users)
                {
                    var thisViewModel = new UserRolesViewModel();
                    thisViewModel.UserId = user.Id;
                    thisViewModel.Email = user.Email;
                    thisViewModel.FirstName = user.FirstName;
                    thisViewModel.LastName = user.LastName;
                    thisViewModel.Roles = await GetUserRoles(user);
                    userRolesViewModel.Add(thisViewModel);
                }
                return View(userRolesViewModel);
            }
            catch (Exception ex)
            {
                _serviceLogger.LogError("Error to get " + ex.Message);
                return View();
                
            }
          
        }
        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        public async Task<IActionResult> Manage(string userId)
        {
            try
            {

                var claimsIdentity = User.Identity as ClaimsIdentity;

                if (claimsIdentity != null)
                {
                    var userRoleClaim = claimsIdentity.Claims
                   .FirstOrDefault(x => x.Type == ClaimTypes.Role);
               
                
                    ViewBag.userId = userId;

                    //ViewBag.UserName = user.UserName;
                    var model = new List<ManageUserRolesViewModel>();


                    foreach (var role in _roleManager.Roles)
                    {
                        var userRolesViewModel = new ManageUserRolesViewModel
                        {
                            RoleId = role.Id,
                            RoleName = role.Name
                        };
                        if (User.IsInRole(role.Name))
                        {
                            userRolesViewModel.Selected = true;
                        }
                        else
                        {
                            userRolesViewModel.Selected = false;
                        }
                        model.Add(userRolesViewModel);
                    }
                    return View(model);

                }

                //var user = await _userManager.FindByIdAsync(userId);
                //if (user == null)
                //{
                //    ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                //    return View("NotFound");

                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }



        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("Index");
        }
    }

}
