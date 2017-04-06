using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FundsManager.Controllers
{
    public class CustomerFundsController : Controller
    {

        public static IEnumerable<FundInfo> CustomerFunds { get; set; }

        // GET: CustomerFunds
        public ActionResult Index()
        {
            return View(CustomerFunds);
        }
    }
}