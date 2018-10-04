using NPOI.SS.UserModel;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ManagerSearcher.Common
{
    public static class ManagerSearcherCommon
    {
        public static string SelectFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            string selectedFileName = string.Empty;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFileName = openFileDialog.FileName;
            }
            else
            {
                selectedFileName = string.Empty;
            }
            return selectedFileName;
        }

        public static string GetMiddleAndSurname(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }
            if (data.Contains(','))
            {
                data = data.Substring(0, data.IndexOf(","));
            }
            var res = data.Trim().Split(' ');
            if (res.Length > 1)
            {
                string surName = res[res.Length - 1];
                string middleName = res[res.Length - 2];
                return middleName + "," + surName;
            }
            else if (res.Length  == 1)
            {
                string surName = res[res.Length - 1];
                string middleName = string.Empty;
                return middleName + "," + surName;
            }
            return string.Empty;
        }

        public static string GetCellData(ICell cell)
        {
            if (cell != null)
            {
                if (cell.CellType == CellType.Numeric)
                {
                    return Convert.ToString((cell.NumericCellValue)).Trim();
                }
                else if (cell.CellType == CellType.String)
                {
                    return cell.StringCellValue.Trim();
                }
            }
            return string.Empty;
        }

        public static DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                return tbl;
            }
        }
    }
}
