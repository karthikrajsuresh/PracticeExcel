using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PracticeExcel.Data;
using OfficeOpenXml;
using PracticeExcel.ObjectLifeCycleExample;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Database to Excel API", Version = "v1" });
});

builder.Services.AddDbContext<Race>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PracticeConnectionString")));

builder.Services.AddScoped<ExcelService>();
builder.Services.AddScoped<DatabaseToExcelExporter>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    options.InstanceName = "SampleInstance";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Database to Excel API v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapGet("/export-to-excel", async (DatabaseToExcelExporter exporter, IConfiguration config, IDistributedCache cache) =>
{
    string cacheKey = "ExcelExport";
    byte[] fileBytes;

    fileBytes = await cache.GetAsync(cacheKey);

    if (fileBytes == null)
    {
        string connectionString = config.GetConnectionString("PracticeConnectionString");
        string outputPath = Path.Combine(Path.GetTempPath(), $"export_{Guid.NewGuid()}.xlsx");

        await Task.Run(() => exporter.ExportData(connectionString, outputPath));

        fileBytes = await File.ReadAllBytesAsync(outputPath);
        File.Delete(outputPath);

        var cacheEntryOptions = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(15));
        await cache.SetAsync(cacheKey, fileBytes, cacheEntryOptions);
    }

    return Results.File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "export.xlsx");
});

app.MapControllers();

app.Run();
