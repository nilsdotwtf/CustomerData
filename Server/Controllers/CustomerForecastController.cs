using CustomerData.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace CustomerData.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerForecastController : ControllerBase
    {

        public CustomerForecastController()
        {

        }

        public DataTable getAllCustomers()
        {
            StringBuilder sql = new StringBuilder();
            DataTable dataTable = new DataTable();
            sql.Append("SELECT * FROM Customer");

            using (SqlConnection conn = new SqlConnection(CustomerData.Shared.Properties.Resources.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sql.ToString(), conn);
                try
                {
                    conn.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dataTable);
                    conn.Close();
                    da.Dispose();
                    return dataTable;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return new DataTable();
            }
        }

        public CustomerForecast getCustomerById(int id)
        {
            StringBuilder sql = new StringBuilder();
            DataTable dataTable = new DataTable();
            sql.Append("SELECT * FROM Customer WHERE ID="+id.ToString());

            using (SqlConnection conn = new SqlConnection(CustomerData.Shared.Properties.Resources.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sql.ToString(), conn);
                try
                {
                    conn.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dataTable);
                    conn.Close();
                    da.Dispose();
                    CustomerForecast customer = new CustomerForecast
                    {
                        customer_id = (Int32)dataTable.Rows[0].ItemArray[0],
                        name = dataTable.Rows[0].ItemArray[1].ToString(),
                        street = dataTable.Rows[0].ItemArray[2].ToString(),
                        zip = dataTable.Rows[0].ItemArray[3].ToString(),
                        city = dataTable.Rows[0].ItemArray[4].ToString()
                    };
                    return customer;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return new CustomerForecast();
            }
        }

        public void updateCustomer(CustomerForecast customer)
        {
            using (SqlConnection conn = new SqlConnection(CustomerData.Shared.Properties.Resources.ConnectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(String.Format("UPDATE customer SET name = '{1}', street = '{2}', ZIP = '{3}', City = '{4}' WHERE ID = {0}",
                        customer.customer_id.ToString(),
                        customer.street,
                        customer.zip,
                        customer.city), conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        while (reader.Read()) { }
                    }
                    finally
                    {
                        conn.Close();
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void createCustomer(CustomerForecast customer)
        {
            using (SqlConnection conn = new SqlConnection(CustomerData.Shared.Properties.Resources.ConnectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(String.Format("INSERT INTO customer (name,street,zip,city) VALUES('{0}','{1}','{2}','{3}'",
                        customer.street,
                        customer.zip,
                        customer.city), conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    try
                    {
                        while (reader.Read()) { }
                    }
                    finally
                    {
                        conn.Close();
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        [HttpGet]
        public IEnumerable<CustomerForecast> Get()
        {
            List<CustomerForecast> custs = (from DataRow row in getAllCustomers().Rows
                                            select new CustomerForecast()
                                            {
                                                customer_id = (Int32)row.ItemArray[0],
                                                name = row.ItemArray[1].ToString(),
                                                street = row.ItemArray[2].ToString(),
                                                zip = row.ItemArray[3].ToString(),
                                                city = row.ItemArray[4].ToString()
                                            }).ToList();
            return custs;
        }
    }
}
