using NPOI.SS.UserModel;
using System;
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
            var res = data.Trim().Split(' ');
            if (res.Length > 0)
            {
                string surName = res[res.Length - 1];
                string middleName = res[res.Length - 2];
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
    }
}
