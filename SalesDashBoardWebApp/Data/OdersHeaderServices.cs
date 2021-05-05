using Microsoft.Data.SqlClient;
using SalesDashBoardWebApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SalesDashBoardWebApp.Data
{
    public class OdersHeaderServices
    {
        private string ConnectionString()
        {
            return "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB_A6EA21_michael;Integrated Security=True";
        }
        private string ConnectionStringAldleo()
        {
            return "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=aldleo;Integrated Security=True";
        }


        public async Task<List<OderHeader>> GetToadysSale(string enddate,string startdate)
        {

            SqlConnection connection = new SqlConnection(ConnectionString());

            string query = "SELECT orderid,amountdue,orderdatetime FROM orderheaders WHERE orderheaders.orderdatetime BETWEEN '" + enddate + "' AND '" + startdate + "' ;";


            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);


            SqlDataReader reader = command.ExecuteReader();
            List<OderHeader> OderHeaders = new List<OderHeader>();
            OderHeader s;
            // Call Read before accessing data.
            while (reader.Read())
            {

                s = new OderHeader();
                s.Orderid = Convert.ToInt32(reader["orderid"]);
                s.AmountDue = Convert.ToDouble(reader["amountdue"]);
                s.Orderdatetime = (DateTime)reader["orderdatetime"];
                OderHeaders.Add(s);


            }

            // Call Close when done reading.
            reader.Close();


            return await Task.FromResult(OderHeaders.ToList());
        }


        public async Task<List<TopSalesViewModel>> GetTopSale(string tdate,string lastsevnday)
        {
           
            SqlConnection connection = new SqlConnection(ConnectionString());

            string query = "SELECT TOP(10)  menuitems.menuitemtext,Sum(OrderTransactions.Quantity) AS SumOfQuantity FROM ordertransactions INNER JOIN orderheaders ON ordertransactions.orderid = orderheaders.orderid INNER JOIN menuitems ON menuitems.menuitemid = ordertransactions.menuitemid WHERE orderheaders.orderdatetime BETWEEN '"+ lastsevnday + "' AND '"+tdate+ "'GROUP BY menuitems.menuitemtext ORDER BY SumOfQuantity desc";


            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);


            SqlDataReader reader = command.ExecuteReader();
            List<TopSalesViewModel> topSales = new List<TopSalesViewModel>();

            TopSalesViewModel Topsl;
            // Call Read before accessing data.
            while (reader.Read())
            {


                Topsl = new TopSalesViewModel
                {
                    MenuTextItem = Convert.ToString(reader["menuitemtext"]),
                    SumOfQuantity = Convert.ToInt32(reader["SumOfQuantity"])
                };
                topSales.Add(Topsl);
                

            }

            // Call Close when done reading.
            reader.Close();
            connection.Close();
            return await Task.FromResult(topSales.ToList());

        }


        public async Task<List<DepartmentSell>> DepartmentSale()
        {

            SqlConnection connection = new SqlConnection(ConnectionString());

            string query = "SELECT * FROM DepartmentSales";


            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);


            SqlDataReader reader = command.ExecuteReader();
            List<DepartmentSell> departmentSales = new List<DepartmentSell>();
            DepartmentSell d;
            // Call Read before accessing data.
            while (reader.Read())
            {

                d = new DepartmentSell();
               d.Department = reader["Department"].ToString();
                d.DepartmentSale =Convert.ToDouble(reader["DepartmentSale"]);
              
                departmentSales.Add(d);


            }

            // Call Close when done reading.
            reader.Close();


            return await Task.FromResult(departmentSales.ToList());
        }

        public async Task<List<HourlySale>> GetHourlySale(string tdate)
        {

            SqlConnection connection = new SqlConnection(ConnectionStringAldleo());

            string query = "SELECT DATEPART(HOUR, OrderDateTime)AS Hours, SUM(AmountDue) AS Total FROM OrderHeaders WHERE CONVERT(DATE, OrderDateTime)= '"+tdate+"' GROUP BY DATEPART(HOUR, OrderDateTime)";


            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);


            SqlDataReader reader = command.ExecuteReader();
            List<HourlySale> HourlySales = new List<HourlySale>();
            HourlySale s;
            // Call Read before accessing data.
            while (reader.Read())
            {

                s = new HourlySale();
                s.Hours = Convert.ToString(reader["Hours"])+":00:00";
                s.Total = Convert.ToDouble(reader["Total"]);
                
                HourlySales.Add(s);


            }

            // Call Close when done reading.
            reader.Close();


            return await Task.FromResult(HourlySales.ToList());
        }
        public async Task<List<Salesweekly>> GetSaleWeekly( string enddate, string Startdate)
        {

            SqlConnection connection = new SqlConnection(ConnectionStringAldleo());

            string query = " SELECT CAST(OrderDateTime AS DATE) AS Date ,SUM(AmountDue) AS Amount FROM OrderHeaders WHERE orderheaders.orderdatetime BETWEEN '"+enddate+"' AND '"+Startdate+"' GROUP BY CAST(OrderDateTime AS DATE)";

            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);


            SqlDataReader reader = command.ExecuteReader();
            List<Salesweekly> Sales = new List<Salesweekly>();

            Salesweekly Topsl;
            // Call Read before accessing data.
            while (reader.Read())
            {


                Topsl = new Salesweekly
                {
                    Date = Convert.ToString(reader["Date"]),
                    Amount = Convert.ToDouble(reader["Amount"])
                };
                Sales.Add(Topsl);


            }

            // Call Close when done reading.
            reader.Close();
            connection.Close();
            return await Task.FromResult(Sales.ToList());

        }
    }
}
