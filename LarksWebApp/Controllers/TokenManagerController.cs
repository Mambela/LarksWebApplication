using AutoMapper;
using LarksWebApp.DTOs.DTO_Tokens;
using LarksWebApp.Models.Users;
using LarksWebApp.Repository.TokenRepository;
using LarksWebApp.Services.UserManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LarksWebApp.Controllers
{
    [Authorize]
    public class TokenManagerController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenManager tokenManager;
        private readonly IMapper mapper;
        private readonly IUserService userService;

        public TokenManagerController(
            UserManager<ApplicationUser> _userManager,
            ITokenManager _tokenManager,IMapper _mapper,
            IUserService _userService)
        {
            userManager = _userManager;
            tokenManager = _tokenManager;
            mapper = _mapper;
            userService = _userService;
        }
        // GET: TokenManagerController
        public async Task<ActionResult> Index()
        {
            try
            {
                var user = await userManager.GetUserAsync(User);

                if (user == null)
                {
                    return BadRequest();
                }

                var tokenList = tokenManager.GetAllTokens(user.Id);

                var tokenDTOList  = mapper.Map<IList<AuthTokenDTO>>(tokenList);

               
                return View(tokenDTOList);
            }
            catch (Exception)
            {

                throw;
            }

        }

        // GET: TokenManagerController/Details/5
        public ActionResult Details(int id)
        {
            var tokenObj = tokenManager.GetUserToken(id);


            var tokenDTO = mapper.Map<AuthTokenDTO>(tokenObj);  

            return View(tokenDTO);
        }

        // GET: TokenManagerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TokenManagerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                //Attributes required to associate attributes
                var user = await userManager.GetUserAsync(User);

                //Generate Token 
                var token = new JwtSecurityTokenHandler().WriteToken( await userService.GenerateUserTokenAsync(user));


                var name = collection["name"].ToString();
                var expiryDate = DateTime.Now.AddDays(14);

                if (token == null)
                {
                    return RedirectToAction(nameof(Create), "Failed to generate a token for you");
                }

                if (tokenManager.TokenCount(user.Id) >3)
                {
                    return RedirectToAction(nameof(Create),"You have reached the maximum number of Tokens");
                }

                /*
                 * If All navigation passes then we can save the details.
                 * **/


                var authToken = new AuthToken
                {
                    IssueDate = DateTime.UtcNow,
                    ExpiryDate = expiryDate,
                    UserId = user.Id,
                    Name = name,
                    Value = token,  
                };

                if(!await tokenManager.CreateTokenManager(authToken)) 
                {
                    return RedirectToAction(nameof(Create));
                }

                              

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TokenManagerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
         
        // GET: TokenManagerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TokenManagerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                if(!tokenManager.DeleteToken(id))
                {
                    return View();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
