using System;
using System.Web.Mvc;
using System.Linq;
using System.Collections;

namespace Mvc.Bootstrap.Datatables
{
    /// <summary>
    /// Model binder for datatables.js parameters a la http://geeksprogramando.blogspot.com/2011/02/jquery-datatables-plug-in-with-asp-mvc.html
    /// </summary>
    public class NullableDataTablesModelBinder : IModelBinder
    {
        private DataTablesModelBinder dataTablesModelBinder = null;

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            DataTablesParam obj = new DataTablesParam();
            var request = controllerContext.HttpContext.Request.Params;

            if (request.AllKeys.Contains("iDisplayStart")
                && request.AllKeys.Contains("iDisplayLength")
                && request.AllKeys.Contains("iColumns")
                && request.AllKeys.Contains("iSortingCols")
                && request.AllKeys.Contains("sEcho"))
            {
                if (dataTablesModelBinder == null)
                {
                    dataTablesModelBinder = new DataTablesModelBinder();
                }
                return dataTablesModelBinder.BindModel(controllerContext, bindingContext);
            }
            else
            {
                return null;
            }
        }
    }
}