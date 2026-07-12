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
using LarksWebApp.Services.InternalService;
using System.Timers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(EntityMapping));



ConfigureServices(builder.Services);
ConfigurationManager Configuration = builder.Configuration;

//trying to setup email 
 
//Prepare the Connection String
var connectionString = builder.Configuration.GetConnectionString("DEV_ENV_DOCKER");


builder.Services.AddDbContext<LarksContext>(options =>
{
    //options.UseSqlServer(connectionString);
    options.UseNpgsql(connectionString);
    
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



#region Timer Code
//System.Timers.Timer timer = new System.Timers.Timer(60000);
//timer.Elapsed += ExecuteQueuedItems;
//timer.Start();
#endregion



//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//var timer = new Timer(20000);






app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});


//ExecuteQueuedItems();
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


 async void ExecuteQueuedItems(object? sender, ElapsedEventArgs e) {

    try
    {

        using (var scope = app.Services.CreateScope()) { 


        var _services = scope.ServiceProvider;
            //instantite Object

            try
            {
               var configService = _services.GetRequiredService<IConfiguration>();
               var countryService = _services.GetRequiredService<ICountryRepository>();
               var provinceService = _services.GetRequiredService<IProvinceRepository>();
               var cityService = _services.GetRequiredService<ICityRepository>();
               var municipalityService = _services.GetRequiredService<IMunicipalityRepository>();
               var suburbService = _services.GetRequiredService<ISuburbRepository>();
               var districtService = _services.GetRequiredService<IDistrictRepository>();

                var suburbServiceForQueue = new SuburbServiceForQueue(
                    configuration:configService,countryRepo:countryService,provinceService,cityService,suburbService,districtService,municipalityService);


                var queueService = new QueueService();


                //Collect the data from the Queue. 
                //var payload = queueService.CollectFromStationQueue();
                var payloadList = queueService.CollectFromStationQueue("q.createPoliceStations");


                if (payloadList.Count > 0)
                {
                    //EXtrcatLocation
                    await suburbServiceForQueue.ExtractLocationPayload(payloadList,queueService);
                }

            }
            catch (Exception)
            {

                throw;
            }
            // var suburbService = new SuburbService();


            //Setup the Queuing service


            //Consume the Message from the Queue


            //Provide the payload to the Suburb Service


            //Get the Items that have been updated 


            //Send the updated items to a new QUEUE TO be Consumed by the Consumer

           // return true;

        }

    }
    catch (Exception ex)
    {

        throw;
    }
}