using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

namespace SmartShoppingAssistant.Api.Controllers;


[Route("api/[controller]")]
[ApiController]

public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CategoryGetDTO>>> GetAll()
    {
        var categories = await categoryService.GetAllAsync();
        return Ok(categories);
    }

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

    [HttpPost]
    public async Task<ActionResult<CategoryGetDTO>> Create([FromBody] CategoryCreateDTO dto)
    {
        var category = await categoryService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryGetDTO>> Update(int id, [FromBody] CategoryUpdateDTO dto)
    {
        try
        {
            var category = await categoryService.UpdateAsync(id, dto);
            return Ok(category);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await categoryService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
