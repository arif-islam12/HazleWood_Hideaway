using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DataAccess
{
    public class Database_2
    {
        // Method to get the connection string for the database
        protected SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "data source=DESKTOP-0EICFO7;database=Hwood;Trusted_Connection=True"; // Adjust your connection string as needed
            return conn;
        }

        // Method to return DataTable for a given SQL query with optional parameters
        public DataTable getData(string query, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters); // Adding SQL parameters if available
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);  // Fills the DataTable with the query result
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt; // Returning the DataTable
        }

        // Method to execute INSERT, UPDATE, DELETE SQL commands with optional parameters
        public void setDta(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters); // Adding SQL parameters if available
                    }

                    conn.Open();
                    cmd.ExecuteNonQuery(); // Executes the non-query (INSERT, UPDATE, DELETE)
                    conn.Close();
                }

                MessageBox.Show("Data processed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method to execute a query using SqlCommand object directly
        public void setDta(SqlCommand cmd)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery(); // Executes the non-query with prepared command
                    conn.Close();
                }

                MessageBox.Show("Data processed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method to retrieve the maximum invoice number from the SalesHistory table
        public int GetMaxInvoiceNumber()
        {
            int maxInvoiceNumber = 0;
            string query = "SELECT MAX(InvoiceNumber) FROM SalesHistory";
            try
            {
                DataTable dt = getData(query);
                if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                {
                    maxInvoiceNumber = Convert.ToInt32(dt.Rows[0][0]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving maximum invoice number: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return maxInvoiceNumber; // Return the highest invoice number
        }
    }
}
