using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class FundInfo
    {
        public int SchemeCode { get; set; }

        public string SchemeName { get; set; }

        public string ISINDivPayout_ISINGrowth { get; set; }

        public string ISINDivReinvestment { get; set; }

        public string NetAssetValue { get; set; }

        public string RepurchasePrice { get; set; }

        public string SalePrice { get; set; }

        public DateTime Date { get; set; }

        public string FundHouse { get; set; }

        public string SchemeType { get; set; }

    }
}

