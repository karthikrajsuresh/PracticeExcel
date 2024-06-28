using Microsoft.EntityFrameworkCore;
using PracticeExcel.Data;
using OfficeOpenXml;
var builder = WebApplication.CreateBuilder(args);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Race>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PracticeConnectionString")));

builder.Services.AddScoped<ExcelService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
