namespace Northwind.DAL.Entities
{
    public class CustOrderHist
    {
        public string ProductName { get; set; }

        public int Total { get; set; }

        public override string ToString()
        {
            return $"{ProductName} {Total}";
        }
    }
}
