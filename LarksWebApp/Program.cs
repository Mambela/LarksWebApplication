using LarksWebApp.DAL;
using LarksWebApp.Data;
using LarksWebApp.Repository.AddressRepository;
using LarksWebApp.Repository.AddressRepository.StreetRepository;
using LarksWebApp.Repository.AssoHoodStreetRepository;
using LarksWebApp.Repository.CategoryRepository;
using LarksWebApp.Repository.CityRepository;
using LarksWebApp.Repository.Coordinates.CityCoordinateRepository;
using LarksWebApp.Repository.Coordinates.HoodCoordinateRepository;
using LarksWebApp.Repository.Coordinates.MunicipalityCoordinateRepository;
using LarksWebApp.Repository.Coordinates.ProviceCoordinateRepository;
using LarksWebApp.Repository.Coordinates.StreetCoordinateRepository;
using LarksWebApp.Repository.CountryRepository;
using LarksWebApp.Repository.DistrictRepository;
using LarksWebApp.Repository.HoodRepository;
using LarksWebApp.Repository.MunicipalityRepository;
using LarksWebApp.Repository.ProviceRepository;
using LarksWebApp.Repository.StreetRepository;
using LarksWebApp.Repository.SubcategoryRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.EntityFrameworkCore;
 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
 


ConfigureServices(builder.Services);
ConfigurationManager Configuration = builder.Configuration;


builder.Services.AddAutoMapper(typeof(EntityMapping));

//Prepare the Connection String
var connectionString = builder.Configuration.GetConnectionString("SIT_ENV");

builder.Services.AddDbContext<LarksContext>(options => {
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();
app.Run();



void ConfigureServices(IServiceCollection services)
{


    //throw new NotImplementedException();
    services.AddScoped<ICountryRepository, CountryRepository>();
    services.AddScoped<IProvinceRepository, ProvinceRepository>();
    services.AddScoped<IMunicipalityRepository, MunicipalityRepository>();
    services.AddScoped<IDistrictRepository, DistrictRepository>();
    services.AddScoped<INeighbourhoodRepository, NeighbourhoodRepository>();
    services.AddScoped<IStreetRepository, StreetRepository>();
    services.AddScoped<IAddressRepository, AddressRepository>();
    services.AddScoped<ICityRepository, CityRepository>();

    services.AddScoped<IProvinceCoordinatesRepository, ProvinceCoordinatesRepository>();
    services.AddScoped<ICityCoordinatesRepository, CityCoordinatesRepository>();
    services.AddScoped<IMunicipalityCoordinateRepository, MunicipalityCoordinateRepository>();
    services.AddScoped<IHoodCoordinateRepository, HoodCoordinateRepository>();
    services.AddScoped<IStreetCoordinatesRepository, StreetCoordinatesRepository>();
    services.AddScoped<ICategoryRepository, CategoryRepository>();
    services.AddScoped<ISubcategoryRepository, SubcategoryRepository>();
    services.AddScoped<ISubcategoryRepository, SubcategoryRepository>();
    services.AddScoped<ILoggerTrace,ServiceLogger>();
    services.AddScoped<INeighbourhoodStreet, NeighbourhoodStreeetRepository>();
    services.AddScoped<IStreetCoordinateRepository, StreetCoordinateRepository>();

}