using Microsoft.AspNetCore.Mvc;
using PracticeExcel.Data;


namespace PracticeExcel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly Race _dbContext;
        private readonly ExcelService _excelService;

        public ExcelController(Race dbContext, ExcelService excelService)
        {
            _dbContext = dbContext;
            _excelService = excelService;
        }

        [HttpGet("export")]
        public IActionResult ExportToExcel()
        {
            try
            {
                var bikes = _dbContext.Bikes.ToList();
                var parts = _dbContext.Parts.ToList();

                Task.Run(() => _excelService.GenerateExcelFile(bikes, parts));

                var fileContents = _excelService.GenerateExcelFile(bikes, parts);

                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "exported_data.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
