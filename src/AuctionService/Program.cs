using AuctionService;
using AuctionService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<AuctionDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(x => {
    x.UsingRabbitMq
})

builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
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
