using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BusinessLogic.Models;

namespace SmartShoppingAssistant.Api.Controllers;

[Route("api/cart")]
[ApiController]
public class CartController(ICartService cartService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<CartGetDTO>> GetCart()
    {
        var cart = await cartService.GetCartAsync();
        return Ok(cart);
    }

    [HttpPost("items")]
    public async Task<ActionResult<CartItemGetDTO>> AddItem([FromBody] AddCartItemDTO dto)
    {
        try
        {
            var item = await cartService.AddItemAsync(dto);
            return Ok(item);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("items/{itemId}")]
    public async Task<ActionResult<CartItemGetDTO>> UpdateItem(int itemId, [FromBody] UpdateCartItemDTO dto)
    {
        try
        {
            var item = await cartService.UpdateItemAsync(itemId, dto);
            return Ok(item);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("items/{itemId}")]
    public async Task<IActionResult> RemoveItem(int itemId)
    {
        try
        {
            var cart = await cartService.RemoveItemAsync(itemId);
            return Ok(cart);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        await cartService.ClearCartAsync();
        return NoContent();
    }

    [HttpPost("analyze")]
    public async Task<IActionResult> AnalyzeCart()
    {
        var analysisResponse = await cartService.AnalyzeCartAsync();
        return Ok(analysisResponse);
    }
}