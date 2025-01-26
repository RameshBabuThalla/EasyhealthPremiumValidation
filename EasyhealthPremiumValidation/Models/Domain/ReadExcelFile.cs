using OfficeOpenXml;
using System.Data;
namespace HERGPremiumValidationSchedular.Models.Domain
{
    public class ReadExcelFile
    {
        public DataTable ReadExcelData(string filePath)
        {
            var dataTable = new DataTable();
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var startRow = 1;
                var endRow = worksheet.Dimension.End.Row;
                var startCol = 1;
                var endCol = worksheet.Dimension.End.Column;

                //Add columns
                for (int col = startCol; col <= endCol; col++)
                {
                    dataTable.Columns.Add(worksheet.Cells[startRow, col].Text);
                }

                // Add rows
                for (int row = startRow + 1; row <= endRow; row++)
                {
                    var dataRow = dataTable.NewRow();
                    for (int col = startCol; col <= endCol; col++)
                    {
                        dataRow[col - 1] = worksheet.Cells[row, col].Text;
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }
    }
}
