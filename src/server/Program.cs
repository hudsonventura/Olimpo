using Microsoft.OpenApi.Models;
using System.Reflection;

using Olimpo;
using Olimpo.Domain;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend Olimpo server", Version = "1.0" });

    // Inclua o caminho para o arquivo XML gerado
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();








var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
IConfiguration appsettings = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env}.json", optional: true)
    .Build();

List<Stack> stacks = appsettings.GetSection("stacks").Get<List<Stack>>();

var db = new Context(appsettings);
var stacks2 = db.stacks.Include(x => x.services).ThenInclude(x => x.sensors);


//star threads to check each sensor
SensorsChecker.StartLoopChecker(stacks2);

while(true){
    ConsoleExhibitor.Show(stacks);
    Thread.Sleep(3000);
}











app.Run();