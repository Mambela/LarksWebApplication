using AutoMapper;
using LarksWebApp.DTOs.DTO_Category;
using LarksWebApp.DTOs.DTO_Subcategory;
using LarksWebApp.Models;
using LarksWebApp.Repository.SubcategoryRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LarksWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoryController : ControllerBase
    {
        private readonly ISubcategoryRepository _subcategoryRepository;
        private readonly IMapper _mapper;
        private readonly ServiceLogger serviceLogger;

        public SubcategoryController(ISubcategoryRepository subcategoryRepository, IMapper mapper)
        {

            _subcategoryRepository = subcategoryRepository;
            _mapper = mapper;

            serviceLogger = new ServiceLogger();
        }

        [HttpPost]

        public async Task<IActionResult> CreateCategory(Create_subcategoryDTO create_SubcateogryDTO) {

            try
            {

                if (create_SubcateogryDTO == null)
                {
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                if (_subcategoryRepository.SubCategoryExists(create_SubcateogryDTO.Designation))
                {
                    ModelState.AddModelError("", $"The record {create_SubcateogryDTO.Designation} already exists as a country");

                    return BadRequest(ModelState);
                }





                var subcategoryObj = _mapper.Map<SubcategoryModel>(create_SubcateogryDTO);

                if (await _subcategoryRepository.CreateSubCategory(subcategoryObj) == null)
                {
                    ModelState.AddModelError("", $"Something went wrong while creating the record {subcategoryObj.Designation}");

                    return StatusCode(500, ModelState);
                }


                return StatusCode(StatusCodes.Status201Created, create_SubcateogryDTO);

            }
            catch (Exception ex)
            {
                serviceLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("Allsubcategories")]
        public IActionResult GetSubcategories()
        {

            var listofSubcategoriesObj = _subcategoryRepository.GetAllSubCategories();

            var listOfSubcategoriesDTO = _mapper.Map<List<SubCategoryDTO>>(listofSubcategoriesObj);

            return Ok(listOfSubcategoriesDTO);
        }
        
        [HttpGet("SubcategoriesInCategory")]
        public IActionResult GetSubcategoriesInCategory(int categoryId)
        {

            var listofSubcategoriesObj = _subcategoryRepository.GetSubcategoriesInCategory(categoryId);

            var listOfSubcategoriesDTO = _mapper.Map<List<SubCategoryDTO>>(listofSubcategoriesObj);

            return Ok(listOfSubcategoriesDTO);
        }


        //[HttpGet("subCategoryId:int", Name = "getAllsubcategories")]
        //public IActionResult GetSubcategory(int subCategoryId)
        //{

        //    return Ok();
        //}


        [HttpDelete]
        public IActionResult DeleteSubCategories()
        {
            try
            {
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPatch]
        public IActionResult UpdateSubcategory()
        {

            try
            {

                return Ok();

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
