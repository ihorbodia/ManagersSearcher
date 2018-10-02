using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagerSearcher.Logic;
using System.Diagnostics;
using WatiN.Core;
using System.Windows.Forms;
using SHDocVw;
using System.Threading;

namespace ManagerSearcher.Logic
{
    public class ManagerSearcherProcessor 
    {
        ISheet managersDataSheet;
        String excelFileName;
        String excelFilePath;
        IWorkbook excelWorkBook;

        public ManagerSearcherProcessor(string filePath)
        { 
            if (filePath.Contains("xlsx#"))
            {
                Console.WriteLine("File Error");
            }
            else
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    excelWorkBook = new XSSFWorkbook(file);
                    excelWorkBook.MissingCellPolicy = MissingCellPolicy.CREATE_NULL_AS_BLANK;
                    managersDataSheet = excelWorkBook.GetSheetAt(0);
                    excelFileName = new FileInfo(filePath).Name;
                    excelFilePath = new FileInfo(filePath).FullName;
                    file.Close();
                }
            }
        }

        public void ProcessFile()
        {
            for (int row = 1; row <= 4; row++) 
            {
                IRow rowData = managersDataSheet.GetRow(row);
                if (!string.IsNullOrEmpty(rowData.GetCell(2).StringCellValue))
                {
                    var data = ManagerSearcherCommon.GetMiddleAndSurname(rowData.GetCell(2).StringCellValue).Split(',');
                    string middlename = data[0];
                    string surname = data[1];
                    string URL = rowData.GetCell(3).StringCellValue;
                    ProcessManager(middlename, surname, URL);
                }
            }
        }
        public void SaveFile()
        {
            using (var saveFile = new FileStream(excelFilePath, FileMode.Create, FileAccess.Write))
            {
                excelWorkBook.Write(saveFile);
                saveFile.Close();
                excelWorkBook.Close();
            }
        }

        private void ProcessManager(string middleName, string surname, string URL)
        {
            if (string.IsNullOrEmpty(middleName) || string.IsNullOrEmpty(surname))
            {
                return;
            }
            IE ie = null;
            IWebBrowser wb = new SHDocVw.WebBrowser();
            Settings.AutoStartDialogWatcher = false;
            ie = new IE(wb);
            try
            {
                ie = new IE();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            ie.AutoClose = true;
            ie.Visible = true;

            ie.GoTo(URL);

            var table = ie.Table(Find.ById("MR37N2-S-GB").And(Find.ByClass("nfvtTab linkTabBl")));
            foreach (var row in table.TableRows)
            {
                Debug.WriteLine(row);
            }
        }
    }
}
