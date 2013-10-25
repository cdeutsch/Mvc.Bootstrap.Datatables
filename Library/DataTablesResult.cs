using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace Mvc.Bootstrap.Datatables
{
    public class DataTablesResult : JsonResult
    {
        public static DataTablesResult<TRes> Create<T, TRes>(IQueryable<T> q, DataTablesParam dataTableParam, DataTablesOptions dataTableOptions, Func<T, TRes> transform)
        {
            return new DataTablesResult<T, TRes>(q, dataTableParam, dataTableOptions, transform);
        }
        public static DataTablesResult<T> Create<T>(IQueryable<T> q, DataTablesParam dataTableParam)
        {
            return new DataTablesResult<T, T>(q, dataTableParam, null, t => t);
        }

        public static DataTablesResult<T> CreateResultUsingEnumerable<T>(IEnumerable<T> q, DataTablesParam dataTableParam)
        {
            return new DataTablesResult<T, T>(q.AsQueryable(), dataTableParam, null, t => t);
        }

        public static DataTablesResult Create(object queryable, DataTablesParam dataTableParam)
        {
            queryable = ((IEnumerable) queryable).AsQueryable();
            var s = "Create";

            var openCreateMethod =
                typeof (DataTablesResult).GetMethods().Single(x => x.Name == s && x.GetGenericArguments().Count() == 1);
            var queryableType = queryable.GetType().GetGenericArguments()[0];
            var closedCreateMethod = openCreateMethod.MakeGenericMethod(queryableType);
            return (DataTablesResult) closedCreateMethod.Invoke(null, new[] {queryable, dataTableParam});
        }

    }
    public class DataTablesResult<T> : DataTablesResult
    {
        
    }
    public class DataTablesResult<T, TRes> : DataTablesResult<TRes>
    {
        private readonly Func<T, TRes> _transform;

        public DataTablesResult(IQueryable<T> q, DataTablesParam dataTableParam, DataTablesOptions dataTableOptions, Func<T, TRes> transform)
        {
            _transform = transform;
            var properties = typeof(TRes).GetProperties();

            if (dataTableOptions == null)
            {
                dataTableOptions = new DataTablesOptions();
            }

            var content = GetResults(q, dataTableParam, dataTableOptions, properties.Select(p => Tuple.Create(p.Name, (string)null, p.PropertyType)).ToArray());
            this.Data = content;
            this.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
        }

        static readonly List<PropertyTransformer> PropertyTransformers = new List<PropertyTransformer>()
        {
            Guard<DateTimeOffset>(dateTimeOffset => dateTimeOffset.ToLocalTime().ToString("g")),
            Guard<DateTime>(dateTime => dateTime.ToLocalTime().ToString("g")),
            Guard<IHtmlString>(s => s.ToHtmlString()),
            Guard<object>(o => (o ?? "").ToString())
        };

        public delegate object PropertyTransformer(Type type, object value);
        public delegate object GuardedValueTransformer<TVal>(TVal value);

        static PropertyTransformer Guard<TVal>(GuardedValueTransformer<TVal> transformer)
        {
            return (t, v) =>
            {
                if (!typeof(TVal).IsAssignableFrom(t))
                {
                    return null;
                }
                return transformer((TVal) v);
            };
        }
        public static void RegisterFilter<TVal>(GuardedValueTransformer<TVal> filter)
        {
            PropertyTransformers.Add(Guard<TVal>(filter));
        }
        private DataTablesData GetResults(IQueryable<T> data, DataTablesParam param, DataTablesOptions dataTableOptions, Tuple<string, string, Type>[] searchColumns)
        {

            int totalRecords = data.Count();


            int totalRecordsDisplay;

            //var filters = new DataTablesFilter();

            //var dataArray = data.Select(_transform).AsQueryable();
            //var dataArray = filters.FilterPagingSortingSearch(param, data, out totalRecordsDisplay, searchColumns).Cast<TRes>();
            var dtParameters = param;
            var columns = searchColumns;

            //// CDEUTSCH: searching doesn't work at all in EnitityFramework due to lack of .ToString() support.
            // https://github.com/mcintyre321/mvc.jquery.datatables/issues/18
            //if (!String.IsNullOrEmpty(dtParameters.sSearch))
            //{
            //    var parts = new List<string>();
            //    var parameters = new List<object>();
            //    for (var i = 0; i < dtParameters.iColumns; i++)
            //    {
            //        if (dtParameters.bSearchable[i])
            //        {
            //            parts.Add(DataTablesFilter.GetFilterClause(dtParameters.sSearch, columns[i], parameters));
            //        }
            //    }
            //    data = data.Where(string.Join(" or ", parts), parameters.ToArray());
            //}
            //for (int i = 0; i < dtParameters.sSearchColumns.Count; i++)
            //{
            //    if (dtParameters.bSearchable[i])
            //    {
            //        var searchColumn = dtParameters.sSearchColumns[i];
            //        if (!string.IsNullOrWhiteSpace(searchColumn))
            //        {
            //            var parameters = new List<object>();
            //            var filterClause = DataTablesFilter.GetFilterClause(dtParameters.sSearchColumns[i], columns[i], parameters);
            //            data = data.Where(filterClause, parameters.ToArray());
            //        }
            //    }
            //}

            string sortString = "";
            for (int i = 0; i < dtParameters.iSortingCols; i++)
            {

                int columnNumber = dtParameters.iSortCol[i];
                string columnName = columns[columnNumber].Item1;
                string sortDir = dtParameters.sSortDir[i];
                if (i != 0)
                    sortString += ", ";
                if (dataTableOptions.SearchAliases.ContainsKey(columnName))
                {
                    // check for comma if so add direction to all tokens.
                    var toks = dataTableOptions.SearchAliases[columnName].Split(',');
                    for (var xx = 0; xx < toks.Length; xx++)
                    {
                        if (xx == (toks.Length - 1))
                        {
                            sortString += toks[xx] + " " + sortDir;
                        }
                        else
                        {
                            sortString += toks[xx] + " " + sortDir + ", ";
                        }                        
                    }                    
                }
                else
                {
                    sortString += columnName + " " + sortDir;
                }
            }

            totalRecordsDisplay = data.Count();

            data = data.OrderBy(sortString);
            data = data.Skip(dtParameters.iDisplayStart);
            if (dtParameters.iDisplayLength > -1)
            {
                data = data.Take(dtParameters.iDisplayLength);
            }

            
            var type = typeof(TRes);
            var properties = type.GetProperties();

            var aaData = data.Select(_transform).Cast<object>().ToArray();

            //var toArrayQuery = from i in data.Select(_transform)
            //                   let pairs = properties.Select(p => new {p.PropertyType, Value = (p.GetGetMethod().Invoke(i, null))})
            //                   let values = pairs.Select(p => GetTransformedValue(p.PropertyType, p.Value))
            //                   select values;

            
            var result = new DataTablesData
            {
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecordsDisplay,
                sEcho = param.sEcho,
                aaData = aaData
            };

            return result;
        }

        private object GetTransformedValue(Type propertyType, object value)
        {
            foreach (var transformer in PropertyTransformers)
            {
                var result = transformer(propertyType, value);
                if (result != null) return result;
            }
            return (value as object ?? "").ToString();
        }

    }
}