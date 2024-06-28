using OfficeOpenXml;
using PracticeExcel.Models;


public class ExcelService
{
    public byte[] GenerateExcelFile(IEnumerable<Bike> bikes, IEnumerable<Part> parts)
    {
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Data");

            // Add headers
            worksheet.Cells[1, 1].Value = "Manufacturer ID";
            worksheet.Cells[1, 2].Value = "Bike Name";
            worksheet.Cells[1, 3].Value = "Part Number";

            // Add data
            int row = 2;
            foreach (var bike in bikes)
            {
                worksheet.Cells[row, 1].Value = bike.MANUFACTURER_ID;
                worksheet.Cells[row, 2].Value = bike.BIKE_NAME;
                row++;
            }

            foreach (var part in parts)
            {
                worksheet.Cells[row, 1].Value = part.MANUFACTURER_ID;
                worksheet.Cells[row, 3].Value = part.PART_NO;
                row++;
            }

            return package.GetAsByteArray();
        }
    }
}
