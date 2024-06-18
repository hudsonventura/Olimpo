using Microsoft.OpenApi.Models;
using System.Reflection;

using Olimpo;
using Olimpo.Domain;

using Newtonsoft.Json;



var initialCpuStats = Teste.GetCpuStats();
        Thread.Sleep(1000); // Wait a second to get a valid reading
        var finalCpuStats = Teste.GetCpuStats();

        for (int i = 0; i < initialCpuStats.Length; i++)
        {
            var initialStats = initialCpuStats[i];
            var finalStats = finalCpuStats[i];

            var totalDiff = finalStats.Total - initialStats.Total;
            var idleDiff = finalStats.Idle - initialStats.Idle;

            var usage = (1.0 - ((double)idleDiff / totalDiff)) * 100;

            Console.
            
            WriteLine($"Core {i} usage: {usage:F2}%");
        }


string thermalPath = "/sys/class/thermal/";
        var directories = Directory.GetDirectories(thermalPath, "cooling_device*");

        foreach (var dir in directories)
        {
            try
            {
                string type = File.ReadAllText(Path.Combine(dir, "type")).Trim();
                string temp = File.ReadAllText(Path.Combine(dir, "temp")).Trim();

                if (double.TryParse(temp, out double temperature))
                {
                    // Normalmente, a temperatura é dada em milicelsius, então dividimos por 1000 para obter celsius
                    temperature /= 1000;
                    Console.WriteLine($"Sensor: {type}, Temperature: {temperature}°C");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading temperature from {dir}: {ex.Message}");
            }
        }




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

//this is to cancel error: System.Text.Json.JsonException: A possible object cycle was detected. This can either be due to a cycle or if the object depth is larger than the maximum allowed depth of 32.
builder.Services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Formatting = Formatting.Indented; // Opcional: para saída JSON indentada
            });


//CORS
builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
    });

builder.Services.AddScoped<Context>();

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

app.UseCors("AllowAll"); // Aplica a política de CORS específica






var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
IConfiguration appsettings = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env}.json", optional: true)
    .Build();



//star threads to check each sensor
Task checker_tread = Task.Run(() => SensorsChecker.StartLoopChecker(appsettings));

//Start the tread to show console
Task console_tread = Task.Run(() => ConsoleExhibitor.StartLoopShow(appsettings));

//Start the backend API server
app.Run();