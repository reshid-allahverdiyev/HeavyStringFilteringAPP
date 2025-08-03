using HeavyStringFilteringAPP.Application;
using HeavyStringFilteringAPP.Core.Interfaces;
using HeavyStringFilteringAPP.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IFilteringConfigService , FilteringConfigService >();
builder.Services.AddSingleton<IChunkStorage, InMemoryChunkStorage>();
builder.Services.AddSingleton<ISimilarityService, SimilarityService>(); 
builder.Services.AddSingleton<FilteringQueueService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<FilteringQueueService>());
 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.UseHttpsRedirection();

app.Run(); 