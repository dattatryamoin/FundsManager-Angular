using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace BL
{
    public class FundsReader
    {
        DataTable _schemaTable = new DataTable();

        public DataTable FundsTable { get { return _schemaTable; } }

        public FundsReader()
        {
            PopulateFundsData();
        }

        public void PopulateFundsData()
        {
            try
            {
                WebRequest request = WebRequest.Create(Config.FilePath);
                //request.Timeout = 30 * 60 * 1000;
                //request.UseDefaultCredentials = true;
                //request.Proxy.Credentials = request.Credentials;
                WebResponse response = (WebResponse)request.GetResponse();
                using (Stream s = response.GetResponseStream())
                {
                    string line;
                    StreamReader file = new StreamReader(s);

                    string header = file.ReadLine();
                    List<string> _fields = header.Split(Config.Delimiter.ToCharArray()).ToList<string>();
                    string[] fieldsArray = _fields.ToArray();

                    if (_fields != null)
                    {
                        foreach (string field in _fields)
                        {
                            _schemaTable.Columns.Add(Regex.Replace(field, @"\s+", ""), typeof(String));
                        }

                        _schemaTable.Columns.Add("SchemeType", typeof(String));
                        _schemaTable.Columns.Add("FundHouse", typeof(String));
                    }

                    List<string> list = new List<string>();
                    string schemeType = string.Empty;
                    string fundHouse = string.Empty;

                    while ((line = file.ReadLine()) != null)
                    {
                        // Below logic is skip empty rows
                        if (line.Length < 2)
                            continue;

                        // Below logic to get actual funds details

                        if (Regex.IsMatch(line, @"^\d"))
                        {
                            list = new List<string>();

                            string[] values = line.Split(Config.Delimiter.ToCharArray());

                            DataRow dr = _schemaTable.NewRow();

                            for (int i = 0; i < fieldsArray.Length; i++)
                            {
                                dr[Regex.Replace(fieldsArray[i], @"\s+", "")] = values[i];
                            }

                            dr["SchemeType"] = schemeType;
                            dr["FundHouse"] = fundHouse;

                            _schemaTable.Rows.Add(dr);
                        }
                        else
                        {
                            list.Add(line);
                            if (list.Count > 1)
                                schemeType = list[0];

                            fundHouse = line;
                        }
                    }
                    file.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public IEnumerable<FundInfo> GetFundsBySchemeCodeList(IEnumerable<int> list)
        //{

        //    if (list.Any())
        //    {
        //        string fundsList = string.Join(",", list.ToArray());
        //        DataView dv = _schemaTable.DefaultView;
        //        dv.RowFilter = "SchemeCode In (" + fundsList + ")";
        //        dv.Sort = "SchemeName";
        //        return DataTableToEntity(dv.ToTable());
        //    }
        //    return null;
        //}

        //public IEnumerable<FundInfo> GetFundsByFundHouse(string fundHouse)
        //{
        //    string expression = string.Format("FundHouse = '{0}'", fundHouse);

        //    if (!string.IsNullOrEmpty(fundHouse))
        //    {
        //        DataRow[] rows = _schemaTable.Select(expression);
        //        return DataTableToEntity(rows.CopyToDataTable());
        //    }
        //    return null;
        //}

        //public IEnumerable<FundInfo> GetFundsBySchemeType(string schemeType)
        //{
        //    string expression = string.Format("SchemeType = '{0}'", schemeType);

        //    if (!string.IsNullOrEmpty(schemeType))
        //    {
        //        DataRow[] rows = _schemaTable.Select(expression);
        //        return DataTableToEntity(rows.CopyToDataTable());
        //    }
        //    return null;
        //}

        public IEnumerable<FundInfo> GetFundsBySchemeTypeAndFundHouse(string schemeType, string fundHouse)
        {
            string expression = string.Format("SchemeType = '{0}' AND FundHouse = '{1}'", schemeType, fundHouse);

            if (!string.IsNullOrEmpty(schemeType))
            {
                DataRow[] rows = _schemaTable.Select(expression);

                return rows.Length > 0 ? DataTableToEntity(rows.CopyToDataTable()) : null;
            }
            return null;
        }

        public IEnumerable<string> GetAllFundHouses()
        {
            List<string> list = new List<string>();

            var fundHouses = _schemaTable.AsEnumerable()
                        .Select(row => new
                        {
                            FundHouse = row.Field<string>("FundHouse")
                        }).Distinct().ToList();

            foreach (var item in fundHouses)
            {
                list.Add(item.FundHouse);
            }
            return list;

        }

        public IEnumerable<string> GetAllSchemeTypes()
        {
            List<string> list = new List<string>();

            var schemeType = _schemaTable.AsEnumerable()
                        .Select(row => new
                        {
                            SchemeType = row.Field<string>("SchemeType")
                        }).Distinct().ToList();

            foreach (var item in schemeType)
            {
                list.Add(item.SchemeType);
            }
            return list;
        }


        //public IEnumerable<FundInfo> GetAllFundsData()
        //{
        //    return DataTableToEntity(FundsTable);
        //}

        public string ReadStringFile()
        {
            var webClient = new WebClient();
            return webClient.DownloadString(Config.FilePath);
        }

        public IEnumerable<FundInfo> DataTableToEntity(DataTable dt)
        {
            List<FundInfo> list = new List<FundInfo>();
            foreach (DataRow dr in dt.Rows)
            {
                FundInfo fund = new FundInfo();
                fund.SchemeCode = Convert.ToInt32(dr[0]);
                fund.ISINDivPayout_ISINGrowth = dr[1] != null ? dr[1].ToString() : string.Empty;
                fund.ISINDivReinvestment = dr[2] != null ? dr[2].ToString() : string.Empty;
                fund.SchemeName = dr[3] != null ? dr[3].ToString() : string.Empty;
                fund.NetAssetValue = dr[4] != null ? dr[4].ToString() : string.Empty;
                fund.RepurchasePrice = dr[5] != null ? dr[5].ToString() : string.Empty;
                fund.SalePrice = dr[6] != null ? dr[6].ToString() : string.Empty;
                fund.Date = Convert.ToDateTime(dr[7]);
                fund.FundHouse = dr[8] != null ? dr[8].ToString() : string.Empty;
                fund.SchemeType = dr[9] != null ? dr[9].ToString() : string.Empty;
                list.Add(fund);
            }
            return list;
        }
    }
}
