using System;
using System.Data.SqlClient;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using PracticeExcel.Data;

namespace PracticeExcel.ObjectLifeCycleExample
{
    public class DatabaseToExcelExporter : IDisposable
    {
        private readonly Race _dbContext;
        private SqlConnection connection;
        private XLWorkbook workbook;
        private bool disposed = false;

        public DatabaseToExcelExporter(Race dbContext)
        {
            _dbContext = dbContext;
        }

        public void ExportData(string connectionString, string outputPath)
        {
            try
            {
                string tableName = GetTableName();
                string query = $"SELECT * FROM {tableName}";

                OpenConnection(connectionString);
                CreateWorkbook();
                FetchDataAndPopulateExcel(query);
                SaveWorkbook(outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        private string GetTableName()
        {
            var entityType = _dbContext.Model.FindEntityType(typeof(Race));
            return entityType.GetTableName();
        }

        private void OpenConnection(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        private void CreateWorkbook()
        {
            workbook = new XLWorkbook();
        }

        private void FetchDataAndPopulateExcel(string query)
        {
            var worksheet = workbook.Worksheets.Add("Data");
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    worksheet.Cell(1, i + 1).Value = reader.GetName(i);
                }
                int row = 2;
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        worksheet.Cell(row, i + 1).Value = reader[i].ToString();
                    }
                    row++;
                }
            }
        }

        private void SaveWorkbook(string outputPath)
        {
            workbook.SaveAs(outputPath);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    connection?.Dispose();
                    workbook?.Dispose();
                }
                disposed = true;
            }
        }

        ~DatabaseToExcelExporter()
        {
            Dispose(false);
        }
    }
}