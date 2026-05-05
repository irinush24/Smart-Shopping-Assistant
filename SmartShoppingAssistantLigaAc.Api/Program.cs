using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistantLigaAc.DataAccess;
using SmartShoppingAssistantLigaAc.DataAccess.Repositories;
using SmartShoppingAssistantLigaAc.DataAccess.Entities;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.BusinessLogic.Services;
using SmartShoppingAssistantLigaAc.DataAccess.Repositories.Interfaces;
using SmartShoppingAssistant.BusinessLogic.Models;
using Microsoft.Extensions.AI;
using OpenAI;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("SmartShoppingAssistantContext");

builder.Services.AddDbContext<SmartShoppingAssistantDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IRepository<Category>, BaseRepository<Category>>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
builder.Services.AddScoped<IPromotionService, PromotionService>();

builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<ICartItemService, CartItemService>();

var openAIAPIKey = builder.Configuration["OpenAI: API Key"] ??throw new InvalidOperationException("OpenAPIKey is not configured");

var openAIModel = builder.Configuration["OpenAI: ModelId"] ?? "gpt-4o";

builder.Services.AddSingleton<IChatClient>(new OpenAIClient(openAIAPIKey).GetChatClient(openAIModel).AsIChatClient().AsBuilder().UseFunctionInvocation().Build());

builder.Services.AddScoped<IPromotionCheckerAgent, PromotionCheckerAgent>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
