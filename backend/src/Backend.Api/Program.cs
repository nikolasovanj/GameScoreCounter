using Backend.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services
                .AddApplication()
                .AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
