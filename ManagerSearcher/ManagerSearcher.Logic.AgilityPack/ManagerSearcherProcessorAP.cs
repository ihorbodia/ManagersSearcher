using HtmlAgilityPack;
using ManagerSearcher.Common;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ManagerSearcher.Logic.AgilityPack
{
    public class ManagerSearcherProcessorAP
    {
        ISheet managersDataSheet;
        String excelFileName;
        String excelFilePath;
        IWorkbook excelWorkBook;

        public ManagerSearcherProcessorAP(string filePath)
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

        public async Task ProcessFile()
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
                await Task.Run(() =>
                {
                    if (isNFF(middlename, surname, URL))
                    {
                        rowData.GetCell(5).SetCellValue("NFF");
                    }
                    else
                    {
                        rowData.GetCell(5).SetCellValue("FF");
                    }
                });
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
            var html = "https://www." + URL;
            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(html);
            var desc = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='std_txt' and @align='justify']");
            string description = desc.InnerHtml.Substring(desc.InnerHtml.LastIndexOf('>') + 1);
            Debug.WriteLine(description);

            var shareHolders = htmlDoc.DocumentNode.SelectNodes("//table[@class='nfvtTab linkTabBl']")
                .FirstOrDefault(x => x.Attributes.Count > 5)
                .ChildNodes.Where(x => x.Name == "tr" && x.PreviousSibling.Name == "tr")
                .Select(x => x.ChildNodes.Where(y => y.Name == "td").FirstOrDefault().InnerText.Trim());
            Debug.WriteLine(shareHolders);
            SiteModelAG sm = new SiteModelAG(
                    shareHolders,
                    description
                    );
            return isSiteContainsName(sm, middleName) || isSiteContainsName(sm, surname);
        }

        private bool isSiteContainsName(SiteModelAG sm, string name)
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
