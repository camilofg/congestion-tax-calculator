using Congestion_Models.Configs;
using CongestionTaxApi.Helpers;
using congestion_tax_calculator_netcore;
using CongestionTaxApi.Services;
using Microsoft.OpenApi.Models;
using CongestionTaxApi.Filters;

namespace CongestionTaxCalculatorApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var rangeTaxes = ResourcesHelper.ReadConfig<RangesTax>(builder.Configuration["TaxRangesFile"]);
            var tollFreeVehicles = ResourcesHelper.ReadConfig<List<string>>(builder.Configuration["FreeTollVehiclesFile"]);
            var holydays = ResourcesHelper.ReadConfig<List<DateTime>>(builder.Configuration["HolydaysConfigFile"]).Select(x => DateOnly.FromDateTime(x));

            var congestionTaxCalc = new CongestionTaxCalculator(holydays, rangeTaxes, tollFreeVehicles,
                Convert.ToDouble(builder.Configuration["GracePeriod"]),
                Int32.Parse(builder.Configuration["DayMaxFee"]));
            builder.Services.AddSingleton((ICongestionTaxCalculator)congestionTaxCalc);
            builder.Services.AddTransient<ITaxCalculatorService, TaxCalculatorService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CongestionTaxApi",
                    Description = "A ASP.NET Core web api that generates the Congestion tax fee for a vehicle in periods of time.",
                    Contact = new OpenApiContact
                    {
                        Name = "Congestion Tax - Api",
                        Email = "camus35@hotmail.com",
                    }
                });

                c.OperationFilter<AddHeaderOperationFilter>("x-api-key",
                    "Required api key to access execute requests",
                    "CongestionTaxController");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}