using FlightBooking.AgentServices;
using FlightBooking.AgentServices.CityDetectors;
using FlightBooking.AgentServices.FoursquareServices;
using FlightBooking.AgentServices.GroqServices;
using FlightBooking.AgentServices.IntentDetectors;
using FlightBooking.AgentServices.PromptBuilders;
using FlightBooking.AgentSettings;
using FlightBooking.Services;
using FlightBooking.Services.BookingServices;
using FlightBooking.Services.CheckInServices;
using FlightBooking.Services.FlightServices;
using FlightBooking.Services.MachineLearningServices;
using FlightBooking.Services.NoShowServices;
using FlightBooking.Services.OverBookingNoShowServices;
using FlightBooking.Settings;
using FlightBooking.Tools.WeatherTool;
using Microsoft.Extensions.Options;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ICheckInService, CheckInService>();
builder.Services.AddSingleton<FlightRegressionService>();
builder.Services.AddSingleton<FlightMlService>();
builder.Services.AddScoped<NoShowService>();
builder.Services.AddScoped<MongoFlightDataService>();
builder.Services.AddScoped<OverbookingRecommendationService>();
builder.Services.AddScoped<NoShowPredictionService>();
builder.Services.AddScoped<ITravelAgentService, TravelAgentService>();
builder.Services.AddScoped<IGroqService, GroqService>();
builder.Services.AddScoped<ITravelPromptBuilder, TravelPromptBuilder>();
builder.Services.AddScoped<IIntentDetector, TravelIntentDetector>();
builder.Services.AddScoped<IWeatherTool, WeatherTool>();
builder.Services.Configure<GroqSettings>(builder.Configuration.GetSection("Groq"));
builder.Services.AddHttpClient<IGroqService, GroqService>();
builder.Services.AddHttpClient<ICityExtractor, GroqCityExtractor>();
builder.Services.Configure<RapidApiSettings>(builder.Configuration.GetSection("RapidApi"));
builder.Services.AddHttpClient<IWeatherTool, WeatherTool>();
builder.Services.Configure<FoursquareSettings>(builder.Configuration.GetSection("Foursquare"));
builder.Services.AddHttpClient<IFoursquareService, FoursquareService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettingsKey"));
builder.Services.AddScoped<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();