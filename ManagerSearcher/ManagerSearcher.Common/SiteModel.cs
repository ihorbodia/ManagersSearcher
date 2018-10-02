using System.Collections.Generic;
using WatiN.Core;

namespace ManagerSearcher.Common
{
    public class SiteModel
    {
        public List<string> ShareholderValues { get; }
        public string DescriptionSentence { get; }
        public SiteModel(Table shareHolderTable, Table descriptionTable)
        {
            if (shareHolderTable != null)
            {
                ShareholderValues = new List<string>();
                foreach (var row in shareHolderTable.TableRows)
                {
                    ShareholderValues.Add(row.OuterText);
                }
                DescriptionSentence = descriptionTable.OuterText.Substring(descriptionTable.OuterText.LastIndexOf('\n') + 1);
            }
        }
    }
}
