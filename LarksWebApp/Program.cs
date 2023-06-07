using LarksWebApp.DAL;
using LarksWebApp.Repository.AddressRepository.Address_Repository;
using LarksWebApp.Repository.AddressRepository.BuildingTypeRepository;
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
using LarksWebApp.Repository.LocationRepository;
using LarksWebApp.Repository.MunicipalityRepository;
using LarksWebApp.Repository.ProviceRepository;
using LarksWebApp.Repository.StreetRepository;
using LarksWebApp.Repository.SubcategoryRepository;
using LarksWebApp.Services.FileManagement;
using LarksWebApp.Services.Loggers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using LarksWebApp.Models.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using LarksWebApp.Repository.AddressRepository.HooksRepository;
using LarksWebApp.Services.WebHooks;
using LarksWebApp.Services.UserManager;
using LarksWebApp.Repository.TokenRepository;
using LarksWebApp.Services.PaginationService;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(EntityMapping));



ConfigureServices(builder.Services);
ConfigurationManager Configuration = builder.Configuration;

//trying to setup email 
 


//Prepare the Connection String
var connectionString = builder.Configuration.GetConnectionString("PROD_CONN");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 33));
 

builder.Services.AddDbContext<LarksContext>(options =>
{
    //options.UseSqlServer(connectionString);
    options.UseMySql(connectionString, serverVersion).LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors();
});

//builder.Services.AddDefaultIdentity<ApplicationIdentity>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<LarksContext>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<LarksContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();


builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

//JWT CONFIGURATION
//var JwtIssuer = builder.Configuration["Jwt:Issuer"];
//var JwtAudience = builder.Configuration["Jwt:Audience"];
//var JwtKey = builder.Configuration["Jwt:Key"];



builder.Services.AddAuthentication(options =>
{

    options.DefaultScheme = "JWT_OR_IDENTITY";
    options.DefaultChallengeScheme = "JWT_OR_IDENTITY";
    //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{

    o.TokenValidationParameters = new TokenValidationParameters
    {

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
})
.AddPolicyScheme("JWT_OR_IDENTITY", "JWT_OR_IDENTITY", options => {
    options.ForwardDefaultSelector = context =>
    {
        string authorization = context.Request.Headers[HeaderNames.Authorization];
        if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
            return JwtBearerDefaults.AuthenticationScheme;

        return IdentityConstants.ApplicationScheme;
    };

});

//Authentication Configuration for Cookie Auth 
 

//builder.Services.AddAuthorization(o => o.DefaultPolicy = multiSchemePolicy) ;
builder.Services.AddAuthorization();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}


//app.UseDatabserrorPages();


/*
 * Update IP Address
 * **/

app.UseDeveloperExceptionPage();
app.UseMigrationsEndPoint();


ConfigureRolesServices(builder.Services);

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();



//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

app.MapControllers();
app.Run();



void ConfigureServices(IServiceCollection services)
{


    //throw new NotImplementedException();
    services.AddScoped<ICountryRepository, CountryRepository>();
    services.AddScoped<IProvinceRepository, ProvinceRepository>();
    services.AddScoped<IMunicipalityRepository, MunicipalityRepository>();
    services.AddScoped<IDistrictRepository, DistrictRepository>();
    services.AddScoped<ISuburbRepository, SuburbRepository>();
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
    services.AddScoped<ILoggerTrace, ServiceLogger>();
    services.AddScoped<INeighbourhoodStreet, NeighbourhoodStreeetRepository>();
    services.AddScoped<IStreetCoordinateRepository, StreetCoordinateRepository>();
    services.AddScoped<IAddressRepository, AddressRepository>();
    services.AddScoped<IBuildingTypeRepository, BuildingTypeRepository>();
    services.AddScoped<IFileManagement, FileManagement>();
    services.AddScoped<ILocationRepository, LocationRepository>();
    services.AddScoped<IHooksRepository,HooksRepository>();
    services.AddScoped<IWebhookService, WebHookService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<ITokenManager, TokenManager>();
    services.AddSingleton<IUriService>(o =>
    {
        var accessor = o.GetRequiredService<IHttpContextAccessor>();
        var request = accessor.HttpContext.Request;
        var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
        return new UriService(uri);
    });




    //var httpFactory = new IHttpClientFactory();

    //configuration.Bind(nameof(HttpClientFactory), httpFactory);


    services.AddMvcCore().ConfigureViews(options =>
    {
        options.HtmlHelperOptions.ClientValidationEnabled = true;
    });

     
}


async void ConfigureRolesServices(IServiceCollection services)
{

    
    using (var scope = app.Services.CreateScope()) 
    {
        var _services = scope.ServiceProvider;

        var logger = new ServiceLogger();
        try
        {
            var context = _services.GetRequiredService<LarksContext>();
            var userManager = _services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = _services.GetRequiredService<RoleManager<IdentityRole>>();

            await ContextSeed.SeedRolesAsync(userManager, roleManager);
            await ContextSeed.SeedSuperAdminAsync(userManager, roleManager);

        }
        catch (Exception ex)
        {
            logger.LogError("An error occurred seeding the DB " + ex.Message);
        }


    }

}