using HealthClaim.API.Extensions;
using HealthClaim.API.Services;
using HealthClaim.BAL.Repository.Concrete;
using HealthClaim.BAL.Repository.Interface;
using HealthClaim.DAL;
using HealthClaim.Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var config = builder.Configuration;

//builder.Services.AddAppliationServices(config);


builder.Services.AddDbContext<HealthClaimDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));

/*builder.Services.AddDbContext<HealthClaimDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5, // Number of retries
                maxRetryDelay: TimeSpan.FromSeconds(1), // Delay between retries
                errorNumbersToAdd: null // SQL Server error codes to consider transient
            );
        });
});*/



builder.Services.AddIdentityServices(config);

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeefamilyRepository, EmployeefamilyRepository>();
builder.Services.AddScoped<IEmpAccountDetailRepository, EmpAccountDetailRepository>();
builder.Services.AddScoped<IEmpAdvanceRepository, EmpAdvanceRepository>();
builder.Services.AddScoped<ICommanRepository, CommanRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IOrgClaimLimitRepository, OrgClaimLimitRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Named Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin",
        builder =>
        {
            builder.WithOrigins("https://localhost:7063", "https://uat.dfccil.cetpainfotech.com")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});
var app = builder.Build();


app.UseMiddleware<ExeceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("AllowOrigin");

app.MapControllers();

app.Run();
