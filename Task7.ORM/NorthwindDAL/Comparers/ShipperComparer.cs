using System.Collections.Generic;
using NorthwindORM;

namespace NorthwindDAL.Comparers
{
    public class ShipperComparer : IEqualityComparer<Shipper>
    {
        public bool Equals(Shipper x, Shipper y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Shipper obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
