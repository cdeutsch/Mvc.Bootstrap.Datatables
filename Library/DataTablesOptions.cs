using System.Collections.Generic;

namespace Mvc.Bootstrap.Datatables
{
    public class DataTablesOptions
    {
        /// <summary>
        /// Allows mapping Database field names to translated properties.
        /// </summary>
        public Dictionary<string, string> SearchAliases { get; set; }

        public DataTablesOptions()
        {
            SearchAliases = new Dictionary<string, string>();
        }
    }    
}