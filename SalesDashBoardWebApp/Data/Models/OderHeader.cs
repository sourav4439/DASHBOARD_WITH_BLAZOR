using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesDashBoardWebApp.Data.Models
{
    public class OderHeader
    {
        public int Orderid { get; set; }
        public DateTime Orderdatetime { get; set; }
        public double GrossSales { get; set; }
        public int Transactions { get; set; }
        public double AvgTransactions { get; set; }
        public double AmountDue { get; set; }
    }
}
