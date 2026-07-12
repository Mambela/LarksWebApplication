using AutoMapper;
using LarksWebApp.DTOs.DTO_Category;
using LarksWebApp.Models;
using LarksWebApp.Repository.CategoryRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    /*
     * Category means the category of the location if its a Residents, Shopping or Commercial. 
     * 
     * **/
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ServiceLogger serviceLogger;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;

            serviceLogger = new ServiceLogger();
        }

        [HttpGet]
        public IActionResult GetCategories(string lang)
        {
            try
            {
                var listofCategoriesObj = _categoryRepository.GetAllCategories(lang);

                var ListofCategoriesDTO = new List<CategoryDTO>();

                foreach (var category in listofCategoriesObj)
                {
                    ListofCategoriesDTO.Add(_mapper.Map<CategoryDTO>(category));
                }

                return Ok(ListofCategoriesDTO);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

 

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategories(CreateCategoryDTO createCategoryDTO)
        {

            try
            {
                if (createCategoryDTO == null)
                {
                    return BadRequest(ModelState);
                }


                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                if (_categoryRepository.CategoryExists(createCategoryDTO.Designation))
                {
                    ModelState.AddModelError("", $"The record {createCategoryDTO.Designation} already exisits as a country");

                    return BadRequest(ModelState);
                }


                var categoryObj = _mapper.Map<CategoryModel>(createCategoryDTO);

                if (await _categoryRepository.CreateCategory(categoryObj) == null)
                {
                    ModelState.AddModelError("", $"Something went wrong while creating the record {categoryObj.Designation}");

                    return StatusCode(500, ModelState);
                }

                createCategoryDTO.Id = categoryObj.Id;

                return StatusCode(StatusCodes.Status201Created,createCategoryDTO);

            }
            catch (Exception ex)
            {

                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        
        [HttpPatch]
        public IActionResult UpdateCategory([FromBody] CreateCategoryDTO categoryDTO, Guid categoryId)
        {

            try
            {


                if (categoryDTO == null || categoryDTO.Id != categoryId)
                {
                    return BadRequest(ModelState);
                }

                var categoryObj = _mapper.Map<CategoryModel>(categoryDTO);

                if (!_categoryRepository.UpdateCategory(categoryObj))
                {
                    ModelState.AddModelError("", $"Something went wrong while updating the record {categoryObj.Designation}");

                    return StatusCode(StatusCodes.Status500InternalServerError, categoryDTO);
                }


                return Ok(categoryDTO);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpDelete]
        public IActionResult DeleteCountry(Guid categoryId)
        {

            try
            {

                if (!_categoryRepository.CategoryExists(categoryId))
                {
                    return NotFound();
                }

                var categoryObj = _categoryRepository.GetCategoryById(categoryId);


                if (categoryObj == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }



                if (!_categoryRepository.DeleteCategory(categoryObj))
                {
                    ModelState.AddModelError("", $"Somehting went Wrong while trying to delete {categoryObj.Designation}");

                    return StatusCode(500, ModelState);
                }


                return NoContent();
            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
    }
}
//Authorize