using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.Requests;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

namespace SmartShoppingAssistantLigaAc.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PromotionController(IPromotionService promotionService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<PromotionGetDTO>> Add(PromotionRequest request)
    {
        try
        {
            var createdPromotion = await promotionService.AddAsync(request.Promotion, request.CategoryIDs, request.ProductIDs);
            return CreatedAtAction(nameof(GetById), new { id = createdPromotion.Id }, createdPromotion);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PromotionGetDTO>> Update(int id, PromotionRequest request)
    {
        try
        {
            var updatedPromotion = await promotionService.UpdateAsync(id, request.Promotion, request.CategoryIDs, request.ProductIDs);
            return Ok(updatedPromotion);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
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

     [HttpGet]
    public async Task<ActionResult<List<PromotionGetDTO>>> GetAll()
    {
        try
        {
            var promotions = await promotionService.GetAllAsync();
            return Ok(promotions);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<PromotionGetDTO>> Delete(int id)
    {
        try
        {
            await promotionService.DeleteAsync(id);
            return Ok();

        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

}
