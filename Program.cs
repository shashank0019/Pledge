using Pledge.BUSINESSLOGIC;
using Pledge.DATAACCESS;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Add services to DI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 Read connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 🔹 Register DataAccess & BusinessLogic in DI container
builder.Services.AddScoped<EmployeePledgeDataAccess>(sp => new EmployeePledgeDataAccess(connectionString));
builder.Services.AddScoped<EmployeePledgeBusinessLogic>();

var app = builder.Build();

// 🔹 Enable Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
