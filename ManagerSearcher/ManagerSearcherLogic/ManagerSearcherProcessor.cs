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
            for (int row = 1; row <= managersDataSheet.LastRowNum; row++)
            {
                IRow rowData = managersDataSheet.GetRow(row);
                if (string.IsNullOrEmpty(rowData.GetCell(2).StringCellValue))
                {
                    break;
                }
                var data = ManagerSearcherCommon.GetMiddleAndSurname(rowData.GetCell(2).StringCellValue).Split(',');
                string middlename = data[0];
                string surname = data[1];
                string URL = rowData.GetCell(3).StringCellValue;

                if (isNFF(middlename, surname, URL))
                {
                    rowData.GetCell(5).SetCellValue("NFF");
                }
                else
                {
                    rowData.GetCell(5).SetCellValue("FF");
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

        private bool isNFF(string middleName, string surname, string URL)
        {
            if (string.IsNullOrEmpty(middleName) || string.IsNullOrEmpty(surname))
            {
                return false;
            }
            
            IWebBrowser wb = new SHDocVw.WebBrowser();
            Settings.AutoStartDialogWatcher = false;
            using (IE ie = new IE())
            {
                ie.AutoClose = true;
                ie.Visible = false;
                
                ie.GoTo(URL);

                SiteModel sm = new SiteModel(
                    ie.Table(Find.ById("MR37N2-S-GB").And(Find.ByClass("nfvtTab linkTabBl"))),
                    ie.Table(Find.ByClass("tabElemNoBor overfH"))
                    );
                return isSiteContainsName(sm, middleName) || isSiteContainsName(sm, surname);
            }
        }

        private bool isSiteContainsName(SiteModel sm, string name)
        {
            bool result = false;
            if (sm.DescriptionSentence.Contains(name))
            {
                return true;
            }
            foreach (var item in sm.ShareholderValues)
            {
                if (item.Contains(name))
                {
                    return true;
                }
            }
            return result;
        }
    }
}
