using AutoMapper;
using LarksWebApp.DTOs.DTO_Addresses;
using LarksWebApp.Models;
using LarksWebApp.Models.Users;
using LarksWebApp.Repository.AddressRepository.HooksRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LarksWebApp.Controllers
{
    [Authorize]
    public class ManageWebHooksController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private IHooksRepository hooksRepository;
        private IMapper mapper;

        public ManageWebHooksController(IHooksRepository hooksRepo,IMapper _mapper)
        {
            hooksRepository = hooksRepo;
            mapper = _mapper;

           
         }

                // GET: ManageWebHooksController
        public ActionResult Index()
        {

            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                // the principal identity is a claims identity.
                // now we need to find the NameIdentifier claim
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                string userId = "";

                if (userIdClaim != null)
                {

                    // var userIdValue = userIdClaim.Value;
                   userId= userIdClaim.Value.ToString();

                    var hooksList =mapper.Map<IList<HooksDTO>>(hooksRepository.GetHooksList(userId));



                    return View(hooksList);
                }

            }
            return null;
        }

        // GET: ManageWebHooksController/Details/5
        public ActionResult Details(Guid id)
        {
            try
            {
                var hookObj = hooksRepository.GetHook(id);

                if (hookObj == null)
                {
                    return View();
                }

                var hookDTO = mapper.Map<HooksDTO>(hookObj);

                return View(hookDTO);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: ManageWebHooksController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManageWebHooksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(IFormCollection collection)
        {
            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                string userId = "";

                if (claimsIdentity != null)
                {
                    // the principal identity is a claims identity.
                    // now we need to find the NameIdentifier claim
                    var userIdClaim = claimsIdentity.Claims
                        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);


                    if (userIdClaim != null)
                    {

                        // var userIdValue = userIdClaim.Value;
                        userId = userIdClaim.Value.ToString();

                    }
                }


                //var activeColle = collection["isActive"];

                HooksDTO hooksDTO = new HooksDTO();

                hooksDTO.Designation = collection["designation"];
                hooksDTO.Owner = collection["owner"];
                hooksDTO.EndPoint = collection["endPoint"];
                hooksDTO.Method = collection["method"];
                hooksDTO.IsActive =true;
                hooksDTO.ApplicationUserId = userId;

                var hookObj = mapper.Map<HooksModel>(hooksDTO);

                if (!await hooksRepository.CreateHookAsync(hookObj))
                {
                    ModelState.AddModelError("", "Failed to create a new webhook");
                    return View();
                }

                //now Create a new hook from the object.


                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: ManageWebHooksController/Edit/5
        public ActionResult Edit(Guid id)
        {
            try
            {
                var hookObj = hooksRepository.GetHook(id);

                if (hookObj == null)
                {
                    return View();
                }

                var hookDTO = mapper.Map<HooksDTO>(hookObj);

                return View(hookDTO);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: ManageWebHooksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                string userId = "";

                if (claimsIdentity != null)
                {
                    // the principal identity is a claims identity.
                    // now we need to find the NameIdentifier claim
                    var userIdClaim = claimsIdentity.Claims
                        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);


                    if (userIdClaim != null)
                    {

                        // var userIdValue = userIdClaim.Value;
                        userId = userIdClaim.Value.ToString();

                    }
                }


                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
 

                var updateHookObj = new HooksModel
                {
                    Id = id,
                    ApplicationUserId = userId,
                    Designation = collection["designation"],   
                    IsActive = true,
                    Owner = collection["owner"],
                    EndPoint = collection["endPoint"],
                    Method = collection["method"]
            };

                if (! hooksRepository.UpdateWebHook(updateHookObj))
                {
                    return BadRequest();
                }


                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: ManageWebHooksController/Delete/5
        public ActionResult Delete(Guid id)
        {
            try
            {
                var hookObj = hooksRepository.GetHook(id);

                if (hookObj == null)
                {
                    return View();
                }

                var hookDTO = mapper.Map<HooksDTO>(hookObj);

                return View(hookDTO);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: ManageWebHooksController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                hooksRepository.DeleteWebHook(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
