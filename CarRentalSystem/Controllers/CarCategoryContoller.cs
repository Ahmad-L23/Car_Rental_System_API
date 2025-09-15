using CarRentalAPIBusinessLayer;
using CarRentalDataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CarRentalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarCategoryController : ControllerBase
    {
        private bool IsValidCategory(CarCategoryDTO category)
        {
            if (category == null) return false;
            if (string.IsNullOrEmpty(category.CategoryName)) return false;
            return true;
        }

        [HttpPost("AddCategory", Name = "addNewCategory")]
        [ProducesResponseType(typeof(CarCategoryDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddCategory(CarCategoryDTO newCategoryDTO)
        {
            if (!IsValidCategory(newCategoryDTO))
                return BadRequest("Invalid Category Data");

            ClsCarCategory category = new ClsCarCategory(newCategoryDTO);

            if (category.Save())
            {
                newCategoryDTO.Id = category.ID;
                return CreatedAtRoute("addNewCategory", new { id = newCategoryDTO.Id }, newCategoryDTO);
            }
            else
            {
                return BadRequest("Failed to add category");
            }
        }

        [HttpPut("UpdateCategory")]
        [ProducesResponseType(typeof(CarCategoryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateCategory(CarCategoryDTO updatedCategoryDTO)
        {
            if (updatedCategoryDTO?.Id == null || !IsValidCategory(updatedCategoryDTO))
                return BadRequest("Invalid Category Data");

            ClsCarCategory category = new ClsCarCategory(updatedCategoryDTO, ClsCarCategory.enMood.Update);

            if (category.Save())
            {
                return Ok(updatedCategoryDTO);
            }
            else
            {
                return BadRequest("Failed to update category");
            }
        }

        [HttpDelete("DeleteCategory/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteCategory(int id)
        {
            if (ClsCarCategory.Delete(id))
            {
                return Ok($"Category with ID {id} deleted successfully.");
            }
            return NotFound($"Category with ID {id} not found.");
        }

        [HttpGet("GetCategory/{id}")]
        [ProducesResponseType(typeof(CarCategoryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoryById(int id)
        {
            ClsCarCategory? category = ClsCarCategory.GetById(id);
            if (category == null)
                return NotFound($"Category with ID {id} not found.");

            return Ok(category.CDTO);
        }

        [HttpGet("GetAllCategories")]
        [ProducesResponseType(typeof(List<CarCategoryDTO>), StatusCodes.Status200OK)]
        public IActionResult GetAllCategories()
        {
            List<ClsCarCategory> categories = ClsCarCategory.GetAllCategories();
            List<CarCategoryDTO> dtos = new List<CarCategoryDTO>();
            foreach (var cat in categories)
                dtos.Add(cat.CDTO);

            return Ok(dtos);
        }
    }
}
