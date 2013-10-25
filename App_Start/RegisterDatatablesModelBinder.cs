using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

[assembly: PreApplicationStartMethod(typeof(Mvc.Bootstrap.Datatables.Example.App_Start.RegisterDatatablesModelBinder), "Start")]

namespace Mvc.Bootstrap.Datatables.Example.App_Start
{
    public static class RegisterDatatablesModelBinder {
        public static void Start() {
            ModelBinders.Binders.Add(typeof(DataTablesParam), new NullableDataTablesModelBinder());
        }
    }
}
