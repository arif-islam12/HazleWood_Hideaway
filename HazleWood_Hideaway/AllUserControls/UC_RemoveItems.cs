using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Windows.Forms;
using DataAccess;
using System.Data.SqlClient;

namespace HazleWood_Hideaway.AllUserControls
{
    public partial class UC_RemoveItems : UserControl
    {
        Database_2 db = new Database_2(); // Using Database_2 class
        String query;

        public UC_RemoveItems()
        {
            InitializeComponent();
        }

        private void UC_RemoveItems_Load(object sender, EventArgs e)
        {
            loadData();
        }

        public void loadData()
        {
            query = "SELECT * FROM items"; // Load all items
            DataTable dt = db.getData(query); // Using getData method from Database_2
            guna2DataGridView1.DataSource = dt; // Set the DataSource to the DataTable
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            query = "SELECT * FROM items WHERE name LIKE @name";
            SqlParameter[] parameters = { new SqlParameter("@name", txtSearch.Text + "%") };
            DataTable dt = db.getData(query, parameters); // Now works with the updated method
            guna2DataGridView1.DataSource = dt;
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Ensure the value is not null or empty, and try to parse it safely
                string cellValue = guna2DataGridView1.Rows[e.RowIndex].Cells[0].Value?.ToString();

                if (int.TryParse(cellValue, out int id))
                {
                    if (MessageBox.Show("Delete Item?", "Important Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        query = "DELETE FROM items WHERE iid = @iid";
                        SqlParameter[] parameters = { new SqlParameter("@iid", id) };
                        db.setDta(query, parameters); // Now works with the updated method
                        loadData();
                        MessageBox.Show("Item Deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid item ID. Please ensure the selected row contains a valid ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
