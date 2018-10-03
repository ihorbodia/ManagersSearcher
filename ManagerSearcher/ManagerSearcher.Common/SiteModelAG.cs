using System.Collections;
using System.Collections.Generic;
using WatiN.Core;

namespace ManagerSearcher.Common
{
    public class SiteModelAG
    {
        public List<string> ShareholderValues { get; }
        public string DescriptionSentence { get; }
        public SiteModelAG(IEnumerable<string> list, string description)
        {
            if (list != null)
            {
                ShareholderValues = new List<string>(list);
            }
            DescriptionSentence = description;
        }
    }
}
