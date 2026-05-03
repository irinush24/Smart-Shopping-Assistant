using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

namespace SmartShoppingAssistantLigaAc.Api.Controllers;


[Route("api/[controller]")]
[ApiController]

public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryGetDTO>> GetById(int id)
    {
        try
        {
            var category = await categoryService.GetByIdAsync(id);
            return Ok(category);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<CategoryGetDTO>> Delete(int id)
    {
        try
        {
            await categoryService.DeleteAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryGetDTO>>> GetAll()
    {
        try
        {
            var categories = await categoryService.GetAllAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<CategoryGetDTO>> Add(CategoryGetDTO categoryDTO)
    {
        try
        {
            var addedCategory = await categoryService.AddAsync(categoryDTO);
            return CreatedAtAction(nameof(GetById), new { id = addedCategory.Id }, addedCategory);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryGetDTO>> Update(int id, CategoryGetDTO category)
    {
        try
        {
            var updatedCategory = await categoryService.UpdateAsync(id,category);
            return Ok(updatedCategory);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
