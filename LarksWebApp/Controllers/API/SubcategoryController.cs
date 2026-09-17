using AutoMapper;
using LarksWebApp.DTOs.DTO_Category;
using LarksWebApp.DTOs.DTO_Subcategory;
using LarksWebApp.Models;
using LarksWebApp.Repository.SubcategoryRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
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

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(Create_subcategoryDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(typeof(List<SubCategoryDTO>), StatusCodes.Status200OK)]
        public IActionResult GetSubcategories(string lang)
        {

            var listofSubcategoriesObj = _subcategoryRepository.GetAllSubCategories(lang);

            var listOfSubcategoriesDTO = _mapper.Map<List<SubCategoryDTO>>(listofSubcategoriesObj);

            return Ok(listOfSubcategoriesDTO);
        }
        
        [HttpGet("SubcategoriesInCategory")]
        [ProducesResponseType(typeof(List<SubCategoryDTO>), StatusCodes.Status200OK)]
        public IActionResult GetSubcategoriesInCategory(Guid categoryId)
        {

            var listofSubcategoriesObj = _subcategoryRepository.GetSubcategoriesInCategory(categoryId);

            var listOfSubcategoriesDTO = _mapper.Map<List<SubCategoryDTO>>(listofSubcategoriesObj);

            return Ok(listOfSubcategoriesDTO);
        }

        [HttpGet("Subcategory")]
        [ProducesResponseType(typeof(SubCategoryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult GetSubcategory(Guid Id)
        {
            try
            {
                var subCategory = _subcategoryRepository.GetSubcateogryById(Id);

                var subcategoryDTO = _mapper.Map<SubCategoryDTO>(subCategory);

                return Ok(subcategoryDTO);
            }
            catch (Exception ex)
            {
                serviceLogger.LogError("Error retrieving subcategory " + ex.Message);
                return BadRequest("Failed to return a category");
            }
        }


        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        // [Authorize(Roles = "Admin")]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateSubcategory(Create_subcategoryDTO create_SubcateogryDTO,Guid subcategoryId)
        {
            try
            {

                if(create_SubcateogryDTO == null ||  subcategoryId != create_SubcateogryDTO.Id)
                {
                    return BadRequest();
                }


                var subcategoryObj = _mapper.Map<SubcategoryModel>(create_SubcateogryDTO);


                if (!_subcategoryRepository.UpdateSubCategory(subcategoryObj))
                {
                    return BadRequest();
                }

                return NoContent();

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
