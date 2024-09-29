using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using DataAccess;
using Guna.UI2.WinForms;

namespace HazleWood_Hideaway.AllUserControls
{
    public partial class UC_UpdateItems : UserControl
    {
        Database_2 db = new Database_2(); // Updated to use the Database_2 class
        String query;
        public UC_UpdateItems()
        {
            InitializeComponent();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Ensure that id is set correctly before executing the update query
            if (int.TryParse(guna2DataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int id))
            {
                query = "UPDATE items SET name=@name, catagory=@catagory, price=@price WHERE iid=@id";
                SqlParameter[] parameters = {
                    new SqlParameter("@name", txtName.Text),
                    new SqlParameter("@catagory", txtCategory.Text),
                    new SqlParameter("@price", decimal.Parse(txtPrice.Text)), // Ensure price is a decimal
                    new SqlParameter("@id", id)
                };

                db.setDta(query, parameters);
                loadData();

                txtName.Clear();
                txtPrice.Clear();
                txtCategory.Clear();
            }
            else
            {
                MessageBox.Show("Please select an item to update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UC_UpdateItems_Load(object sender, EventArgs e)
        {
            loadData();
        }

        public void loadData()
        {
            query = "SELECT * FROM items";
            DataTable dt = db.getData(query); // Use DataTable instead of DataSet
            guna2DataGridView1.DataSource = dt; // Set the DataSource to the DataTable
        }

        private void txtSearchItem_TextChanged(object sender, EventArgs e)
        {
            query = "SELECT * FROM items WHERE name LIKE @name";
            SqlParameter[] parameters = {
                new SqlParameter("@name", txtSearchItem.Text + "%")
            };
            DataTable dt = db.getData(query, parameters); // Use DataTable instead of DataSet
            guna2DataGridView1.DataSource = dt; // Set the DataSource to the DataTable
        }

        int id; // This variable should be set when a user selects an item in the DataGridView
        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is selected
            {
                if (int.TryParse(guna2DataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(), out id))
                {
                    String category = guna2DataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                    String name = guna2DataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    decimal price = decimal.Parse(guna2DataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());

                    txtCategory.Text = category;
                    txtName.Text = name;
                    txtPrice.Text = price.ToString(); // Ensure to convert price to string
                }
                else
                {
                    MessageBox.Show("Invalid ID format.");
                }
            }
        }
    }
}
