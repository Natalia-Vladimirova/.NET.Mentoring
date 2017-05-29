using System.Collections.Generic;
using NorthwindORM;

namespace NorthwindDAL.Models
{
    public class EmployeeShippers
    {
        public Employee Employee { get; set; }

        public IList<Shipper> Shippers { get; set; }
    }
}
