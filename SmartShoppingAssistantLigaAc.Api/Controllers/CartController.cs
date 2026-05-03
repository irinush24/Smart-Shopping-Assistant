using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BusinessLogic.DTOs.Requests;

namespace SmartShoppingAssistantLigaAc.Api.Controllers;

[Route ("api/cart")]
[ApiController]
public class CartController(ICartItemService cartItemService) : ControllerBase
{
    [HttpPost("items")]
    public async Task<ActionResult<CartItemGetDTO>> AddAsync(CartItemAddRequest request)
    {
        try
        {
            var createdCartItem = await cartItemService.AddAsync(request);
            return Ok(createdCartItem);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("items/{itemId}")]
    public async Task<ActionResult<CartItemGetDTO>> DeleteById(int itemId)
    {
        try
        {
            await cartItemService.DeleteByIdAsync(itemId);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteAll()
    {
        try
        {
            await cartItemService.DeleteAllAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<CartGetDTO>>> GetAll()
    {
        try
        {
            var cartItems = await cartItemService.GetAsync();
            return Ok(cartItems);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPut("items/{itemId}")]
    public async Task<ActionResult<CartItemGetDTO>> Update(int itemId, CartItemUpdateRequest request)
    {
        try
        {
            var updatedItem = await cartItemService.UpdateAsync(itemId, new CartItemGetDTO
            {
                Id = itemId,
                Quantity = request.Quantity
            });
            return Ok(updatedItem);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}