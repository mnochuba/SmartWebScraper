using SmartWebScraper.API;
using SmartWebScraper.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
//builder.Services.RegisterApplicationServices();
builder.Services.RegisterPersistenceServices(builder.Configuration);

// Add Swagger services.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SmartWebScraper API", Version = "v1" });
});

var app = builder.Build();

app.ApplyDbMigrations();


//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();