using HtmlAgilityPack;
using ManagerSearcher.Common;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerSearcher.Logic.AgilityPack
{
    public class ManagerSearcherProcessorAP
    {
        String excelFileName;
        String excelFilePath;
        ExcelPackage p;

        List<Task> tasks;

        public ManagerSearcherProcessorAP(string filePath)
        {
            if (filePath.Contains("xlsx#"))
            {
                Console.WriteLine("File Error");
            }
            else
            {
                tasks = new List<Task>();
                excelFileName = new FileInfo(filePath).Name;
                excelFilePath = new FileInfo(filePath).FullName;
            }
        }

        public void ProcessFileByEpp()
        {
            FileInfo fi = new FileInfo(excelFilePath);
            if (fi.Exists)
            {
                p = new ExcelPackage(fi);
                ExcelWorksheet workSheet = p.Workbook.Worksheets["Feuil1"];
                var start = workSheet.Dimension.Start.Row + 1;
                var end = workSheet.Dimension.End.Row;
                for (int row = start; row <= end; row++)
                {
                    string names = workSheet.Cells[row, 3].Text;
                    string URL = workSheet.Cells[row, 4].Text;
                    if (string.IsNullOrEmpty(names))
                    {
                        break;
                    }
                    var data = ManagerSearcherCommon.GetMiddleAndSurname(names).Split(',');
                    string middlename = data[0];
                    string surname = data[1];
                    object arg = row;
                    tasks.Add(Task.Factory.StartNew(new Action<object>((argValue) =>
                    {
                        int num = Convert.ToInt32(argValue);
                        Debug.WriteLine(num);
                        if (isNFF(middlename, surname, URL))
                        {
                            workSheet.Cells[num, 6].Value = "NFF";
                        }
                        else
                        {
                            workSheet.Cells[num, 6].Value = "FF";
                        }
                    }), arg));

                }
            }
        }

        public void SaveFile()
        {
            Task.WaitAll(tasks.ToArray());
            p.Save();
            p.Dispose();
        }

        private bool isNFF(string middleName, string surname, string URL)
        {
            if (string.IsNullOrEmpty(middleName) || string.IsNullOrEmpty(surname))
            {
                return false;
            }
            string html = string.Empty;
            if (!URL.Contains("https://www."))
            {
                html = "https://www." + URL;
            }
            else
            {
                html = URL;
            }
            
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDoc = null;
            try
            {
                htmlDoc = web.Load(html);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            if (htmlDoc == null)
            {
                return false;
            }
            var desc = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='std_txt' and @align='justify']");
            string description = desc.InnerHtml.Substring(desc.InnerHtml.LastIndexOf('>') + 1);
            Debug.WriteLine(description);

            var managers = htmlDoc.DocumentNode.SelectNodes("//table[@class='nfvtTab linkTabBl']").FirstOrDefault(x => x.Attributes.Count < 5);
            IEnumerable<string> data = null;
            if (managers != null)
            {
                data = managers.ChildNodes.Where(x => x.Name == "tr" && x.PreviousSibling.Name == "tr")
                .Select(x => x.ChildNodes.Where(y => y.Name == "td").FirstOrDefault().InnerText.Trim());
            }
            Debug.WriteLine(managers);
            SiteModelAG sm = new SiteModelAG(
                    data,
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
            if (sm.ShareholderValues != null)
            {
                foreach (var item in sm.ShareholderValues)
                {
                    if (item.Contains(name))
                    {
                        return true;
                    }
                }
            }
            return result;
        }
    }
}
