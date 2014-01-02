using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc.Bootstrap.Datatables.Sample.Models
{
    public class Car 
    {
        public long Id { get; set; }

        public int Year { get; set; }

        [Required]
        [StringLength(100)]
        public string Make { get; set; }

        [Required]
        [StringLength(100)]
        public string Model { get; set; }

        public string FriendlyName
        {
            get
            {
                return string.Format ("{0} {1} {2}", Year, Make, Model);
            }
        }
    }
}