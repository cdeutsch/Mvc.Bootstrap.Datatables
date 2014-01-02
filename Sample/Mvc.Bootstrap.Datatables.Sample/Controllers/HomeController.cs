using Mvc.Bootstrap.Datatables.Sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc.Bootstrap.Datatables.Sample.Controllers
{
    public class HomeController : Controller
    {
        readonly SampleDb db;

        public HomeController ()
        {
            db = new SampleDb();
        }

        public ActionResult Index(DataTablesParam dataTableParam = null)
        {
            if (Request.IsAjaxRequest())
            {
                var cars = db.Cars.AsQueryable();

                // swap out sort fields with ones DynamicLinq will handle.
                var dataTableOptions = new DataTablesOptions();
                dataTableOptions.SearchAliases.Add("Name", "Year");

                return DataTablesResult.Create(cars, dataTableParam, dataTableOptions, oo => new
                {
                    Name = oo.FriendlyName,
                    Year = oo.Year.ToString().Substring(2),
                    Id = oo.Id
                });
            }
            else
            {
                return View();
            }
        }
    }
}
