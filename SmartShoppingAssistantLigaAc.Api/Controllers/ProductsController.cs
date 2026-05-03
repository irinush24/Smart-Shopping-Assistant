using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.Requests;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

namespace SmartShoppingAssistantLigaAc.Api.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductGetDTO>> GetById(int id)
    {
        try
        {
            var product = await productService.GetByIdAsync(id);
            return Ok(product);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ProductGetDTO>> Delete(int id)
    {
        try
        {
            await productService.DeleteAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductGetDTO>>> GetAll([FromQuery] int? categoryId)
    {
        try
        {
            var products = await productService.GetAllAsync(categoryId);
            return Ok(products);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<ProductGetDTO>> Add(ProductGetDTO productDTO)
    {
        try
        {
            var addedProduct = await productService.AddAsync(productDTO, new List<int>());
            return CreatedAtAction(nameof(GetById), new { id = addedProduct.Id }, addedProduct);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductGetDTO>> Update(int id, ProductRequest request)
    {
        try
        {
            var updatedProduct = await productService.UpdateAsync(id, request.Product, request.NewCategoryIDs);
            return Ok(updatedProduct);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}