using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccess;

namespace HazleWood_Hideaway
{
    public partial class chef : Form
    {
        Database_2 db = new Database_2();
        public chef()
        {
            InitializeComponent();
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    // Get the selected order's ID from the DataGridView (column index 0 is assumed for the ID)
                    int orderId = Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells[0].Value);

                    // Show a confirmation dialog asking if the order is done
                    DialogResult result = MessageBox.Show("Is this order done?", "Order Completion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // Remove the selected row from the chef_order table and DataGridView
                        string deleteQuery = "DELETE FROM chef_order WHERE id = @id";
                        SqlParameter[] parameters = { new SqlParameter("@id", orderId) };
                        db.setDta(deleteQuery, parameters); // Delete the order from the database

                        // Remove the row from the DataGridView
                        guna2DataGridView1.Rows.RemoveAt(e.RowIndex);
                        MessageBox.Show("Order completed and removed from the list.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string query = "SELECT item_name, quantity, table_number, order_date FROM chef_order";
            DataTable dt = db.getData(query);

            // Clear existing rows in DataGridView before loading new data
            guna2DataGridView1.Rows.Clear();

            // Populate DataGridView with fetched data
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int rowIndex = guna2DataGridView1.Rows.Add(); // Add new row
                guna2DataGridView1.Rows[rowIndex].Cells[0].Value = (i + 1).ToString(); // Serial number
                guna2DataGridView1.Rows[rowIndex].Cells[1].Value = dt.Rows[i]["item_name"].ToString(); // Item Name
                guna2DataGridView1.Rows[rowIndex].Cells[2].Value = dt.Rows[i]["quantity"].ToString(); // Quantity
                guna2DataGridView1.Rows[rowIndex].Cells[3].Value = dt.Rows[i]["table_number"].ToString(); // Table Number
                guna2DataGridView1.Rows[rowIndex].Cells[4].Value = Convert.ToDateTime(dt.Rows[i]["order_date"]).ToString("yyyy-MM-dd HH:mm:ss"); // Order Time
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            LoginForm lf = new LoginForm();
            lf.Show();
            this.Hide();
        }
    }
}
