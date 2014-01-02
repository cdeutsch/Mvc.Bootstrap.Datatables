using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.Bootstrap.Datatables.Sample.Models
{
    public class SampleDb : DbContext
    {
        public SampleDb()
            : base("DefaultConnection")
        {
        }

        public SampleDb (string connectionStringOrName)
            : base(connectionStringOrName)
        {
        }

        public DbSet<Car> Cars { get; set; }

    }
}
