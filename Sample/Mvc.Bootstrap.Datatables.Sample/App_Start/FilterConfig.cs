using System.Web;
using System.Web.Mvc;

namespace Mvc.Bootstrap.Datatables.Sample
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters (GlobalFilterCollection filters)
        {
            filters.Add (new HandleErrorAttribute ());
        }
    }
}