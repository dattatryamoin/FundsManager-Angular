using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace FundsManager.Controllers
{
    public class FundsController : ApiController
    {

        BL.FundsReader fundsReader = new BL.FundsReader();

        [HttpGet]
        public IEnumerable<string> GetAllFundHouses()
        {
            return fundsReader.GetAllFundHouses();
        }

        [HttpGet]
        public IEnumerable<string> GetAllSchemeTypes()
        {
            return fundsReader.GetAllSchemeTypes();
        }

        [HttpGet]
        public IEnumerable<FundInfo> GetFundsBySchemeTypeAndFundHouse(string schemeType, string fundHouse)
        {
            return fundsReader.GetFundsBySchemeTypeAndFundHouse(schemeType, fundHouse);
        }

        [HttpGet]
        public string ReadStringFile()
        {
            return fundsReader.ReadStringFile();
        }

        [HttpPost]
        public void SaveCustomList(IEnumerable<FundInfo> fundsList)
        {
            CustomerFundsController.CustomerFunds = fundsList;
        }
    }
}
