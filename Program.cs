using advent_appointment_booking.Database;
using advent_appointment_booking.Enums;
using advent_appointment_booking.Helpers;
using advent_appointment_booking.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using log4net;
using log4net.Config;
using System.Security.Claims;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

builder.Services.AddControllers(options =>
{
    // Set authentication globally i.e. for all Controllers 
    // Hence, no need to use [Authorize] for every controller or action.
    options.Filters.Add(new AuthorizeFilter());
    options.Filters.Add<CustomExceptionFilter>();
});

// Suppress automatic validation by Asp.net core to allow 
// using ModelState.IsValid and return custom error messages
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()  // Allow all origins
                   .AllowAnyHeader()  // Allow any header
                   .AllowAnyMethod(); // Allow any method
        });
});

// Register the DbContext using the connection string from the configuration
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

// Register the Services 
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<JwtTokenGenerator>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IDriverService, DriverService>();

// Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
});

// Add Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policy.RequireTruckingCompanyRole, policy => policy.RequireClaim(ClaimTypes.Role, UserType.TruckingCompany));
    options.AddPolicy(Policy.RequireTerminalRole, policy => policy.RequireClaim(ClaimTypes.Role, UserType.Terminal));
    options.AddPolicy(Policy.RequireTruckingCompanyOrTerminalRole, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == ClaimTypes.Role && (c.Value == UserType.TruckingCompany || c.Value == UserType.Terminal))));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// Use CORS policy
app.UseCors("AllowAllOrigins");

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

