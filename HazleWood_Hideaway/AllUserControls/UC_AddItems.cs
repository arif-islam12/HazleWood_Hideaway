using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using DataAccess;

namespace HazleWood_Hideaway.AllUserControls
{
    public partial class UC_AddItems : UserControl
    {
        Database_2 db = new Database_2(); // Using Database_2 class
        String query;

        public UC_AddItems()
        {
            InitializeComponent();
        }

        public void clearAll()
        {
            txtCatagory.SelectedIndex = -1;
            txtItemName.Clear();
            txtPrice.Clear();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtItemName.Text) || string.IsNullOrEmpty(txtCatagory.Text)
               || string.IsNullOrEmpty(txtPrice.Text))
            {
                MessageBox.Show("Please fill the textbox .", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else { 
            // Parameterized query to insert a new item
            query = "INSERT INTO items (name, catagory, price) VALUES (@name, @catagory, @price)";
            SqlParameter[] parameters = {
                new SqlParameter("@name", txtItemName.Text),
                new SqlParameter("@catagory", txtCatagory.Text),
                new SqlParameter("@price", txtPrice.Text)
            };

            db.setDta(query, parameters); // Using setDta method from Database_2

            clearAll(); // Clear input fields after adding the item
               MessageBox.Show("Item added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UC_AddItems_Leave(object sender, EventArgs e)
        {
            clearAll(); // Clear fields when leaving the control
        }
        private void UC_AddItems_Leave_1(object sender, EventArgs e)
        {
            clearAll();
        }

    }
}
