using NPOI.SS.UserModel;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        private static string RemoveThrash(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }
            string result = string.Empty;
            string newChar = string.Empty;
            data = 
                data.Replace("MBA", newChar)
                    .Replace("PhD", newChar)
                    .Replace("CPA", newChar)
                    .Replace("Jr", newChar)
                    .Replace("Sr", newChar)
                    .Replace("MD", newChar)
                    .Replace("CFA", newChar)
                    .Replace(",", newChar)
                    .Replace(".", newChar);
            data = Regex.Replace(data, @"\s[I]{1,}", string.Empty);
            data = Regex.Replace(data, @"(\s)([IV]{1,})(\b|\s)", string.Empty);
            var res = data.Trim().Split(' ').ToList().Where(x => x.Length > 2);
            var resData = string.Join(" ", res);
            return resData;
        }

        public static string GetMiddleAndSurname(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }
            data = RemoveThrash(data);
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
