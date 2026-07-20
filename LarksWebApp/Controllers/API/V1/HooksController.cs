using AutoMapper;
using LarksWebApp.DTOs;
using LarksWebApp.DTOs.DTO_Addresses;
using LarksWebApp.Models;
using LarksWebApp.Repository.AddressRepository.HooksRepository;
using LarksWebApp.Services.Loggers;
using LarksWebApp.Services.WebHooks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers.API.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class HooksController : ControllerBase
    {

        /// <summary>
        /// This controller will have to be handled with JWT tokens that the user ID will be retrieved from JWT
        /// </summary>
        private readonly IHooksRepository hookRepository;
        private readonly ILoggerTrace serviceLogger;
        private readonly IMapper mapper;
        private readonly IWebhookService webService;
        public HooksController(IHooksRepository _hooksRepository, ILoggerTrace _serviceLogger,IMapper _mapper,IWebhookService _webhookService)
        {
            hookRepository =_hooksRepository;
            serviceLogger =_serviceLogger;
            mapper = _mapper;
            webService = _webhookService;
        }

        
        /// <summary>
        /// This end point is responsible for creating a webhook for the application. 
        /// </summary>
        /// <param name="userWebhook"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(HooksModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(HooksDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateWebHook(HooksDTO userWebhook)
        {
            try
            {

                if(userWebhook == null)   
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(userWebhook);
                }



                var webHook = mapper.Map<HooksModel>(userWebhook);


                if (!await hookRepository.CreateHookAsync(webHook))
                {

                    ModelState.AddModelError("", "Failed to Add new web hook");
                    return BadRequest(ModelState);
                }

                userWebhook.Id = webHook.Id;

                return StatusCode(StatusCodes.Status201Created,webHook);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                var message = new ErrorMessage
                {
                    Message = "Failed to create a web hook",
                    Source = "Webhook create"

                };
                return BadRequest(message);
                
            }
        }


        /// <summary>
        /// End point to be used to test Web Hook. 
        /// </summary>
        /// <param name="hooksDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("TestWebHook")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> PostWebHook(HookTestDTO hooksDTO)
        {
            try
            {

                var webObject = new
                {
                   reference = "SOME REFERENCE",
                   provinceId = 1,
                   suburbId = 23
                };

                webService.SendWebHook(hooksDTO.WebHook, webObject);

                return Ok("Processed Hook");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
          

        /// <summary>
        ///  Returns a specific web hook from the user. 
        /// </summary>
        /// <param name="hookDesignation"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult actionResult(string hookDesignation)
        {
            try
            {


                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        /// <summary>
        /// Update a client's WebHook
        /// </summary>
        /// <param name="webHook"></param>
        /// <param name="hookId"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateWebHook(HooksDTO webHook,Guid  hookId)
        {
            try
            {
                if (webHook == null || webHook.Id != hookId)
                {
                    return BadRequest();
                }
                              
                


                return Ok();
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);

                return BadRequest();
                
            }
        }
    }

}
