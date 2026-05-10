using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

namespace SmartShoppingAssistant.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PromotionController(IPromotionService promotionService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<PromotionGetDTO>>> GetAll()
    {
        var promotions = await promotionService.GetAllAsync();
        return Ok(promotions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PromotionGetDTO>> GetById(int id)
    {
        try
        {
            var promotion = await promotionService.GetByIdAsync(id);
            return Ok(promotion);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<PromotionGetDTO>> Create([FromBody] PromotionCreateDTO dto)
    {
        var promotion = await promotionService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = promotion.Id }, promotion);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PromotionGetDTO>> Update(int id, [FromBody] PromotionUpdateDTO dto)
    {
        try
        {
            var promotion = await promotionService.UpdateAsync(id, dto);
            return Ok(promotion);
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
            await promotionService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
