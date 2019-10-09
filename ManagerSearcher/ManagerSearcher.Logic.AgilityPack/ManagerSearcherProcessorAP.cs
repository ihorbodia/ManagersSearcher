using HtmlAgilityPack;
using ManagerSearcher.Common;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerSearcher.Logic.AgilityPack
{
    public class ManagerSearcherProcessorAP
    {
        String excelFileName;
        String excelFilePath;
        ExcelPackage p;
        bool searchByMiddles;

        List<Task> tasks;

        public ManagerSearcherProcessorAP(string filePath, bool searchByMiddleNames)
        {
            
            if (filePath.Contains("xlsx#"))
            {
                Console.WriteLine("File Error");
            }
            else
            {
                searchByMiddles = searchByMiddleNames;
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
                ExcelWorksheet workSheet = p.Workbook.Worksheets[1];
                DataTable dt = new DataTable();
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
						try
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
						}
						catch (Exception ex)
						{
							Console.WriteLine(ex.StackTrace);
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
            if (string.IsNullOrEmpty(surname))
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
            
            Encoding iso = Encoding.GetEncoding("iso-8859-1");
            HtmlWeb web = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = iso,
            };
            HtmlDocument htmlDoc = null;
            try
            {
                htmlDoc = web.Load(html);
                var encoding = htmlDoc.Encoding;
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
			int index = 0;
			string description = string.Empty;
			if (desc != null && desc.InnerHtml != null)
			{
				index = desc.InnerHtml.LastIndexOf('>') + 1;
				description = desc.InnerHtml.Substring(index);
			}

            Debug.WriteLine(description);

			HtmlNode managers = null;
			try
			{
				managers = htmlDoc.DocumentNode.SelectNodes("//table[@class='nfvtTab linkTabBl']")?
					.FirstOrDefault(x => x.Attributes.Count < 6);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

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

            if (searchByMiddles)
            {
                return isSiteContainsName(sm, middleName) || isSiteContainsName(sm, surname);
            }
            return isSiteContainsName(sm, surname);
        }

        private bool isSiteContainsName(SiteModelAG sm, string name)
        {
            bool result = false;
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            if (sm.DescriptionSentence.ToUpper().Contains(name.ToUpper()))
            {
                return true;
            }
            if (sm.ShareholderValues != null)
            {
                foreach (var item in sm.ShareholderValues)
                {
                    if (item.ToUpper().Contains(name.ToUpper()))
                    {
                        return true;
                    }
                }
            }
            return result;
        }
    }
}
