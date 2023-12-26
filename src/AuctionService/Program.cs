using AuctionService;
using AuctionService.Data;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<AuctionDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(x => 
{
        x.UsingRabbitMq((context, cfg)=>
        {
    cfg.ConfigureEndpoints(context);
        });
    });

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
             options.Authority = builder.Configuration["IdnetityServiceUrl"];
             options.RequireHttpsMetadata = false;
             options.TokenValidationParameters.ValidateAudience = false;
             options.TokenValidationParameters.NameClaimType = "username";
        });

builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

try
{
    DbInitializer.InitDb(app);
}
catch (Exception e)
{
    Console.WriteLine("Exception at DBInitializer" + e);
}

app.Run();
