using DataAccess;
using DGVPrinterHelper;
using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace HazleWood_Hideaway.AllUserControls
{
    public partial class UC_PlaceOrder : UserControl
    {
        Database_2 db = new Database_2(); // Use Database_2 class
        string query;
        protected int n; // Declare variable n for DataGridView row index
        protected int total = 0; // Declare total at class level
        protected int amount; // Declare amount at class level

        public UC_PlaceOrder()
        {
            InitializeComponent();
        }

        private void comboCatagory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string catagory = comboCatagory.Text;
            query = "SELECT name FROM items WHERE catagory = @catagory";
            SqlParameter[] parameters = { new SqlParameter("@catagory", catagory) };
            showItemList(query, parameters);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string catagory = comboCatagory.Text;
            query = "SELECT name FROM items WHERE catagory = @catagory AND name LIKE @name";
            SqlParameter[] parameters = {
                new SqlParameter("@catagory", catagory),
                new SqlParameter("@name", txtSearch.Text + "%")
            };
            showItemList(query, parameters);
        }

        private void showItemList(string query, SqlParameter[] parameters)
        {
            listBox1.Items.Clear();
            DataTable dt = db.getData(query, parameters);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                listBox1.Items.Add(dt.Rows[i][0].ToString());
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtQuantityUpDown.Value = 0;
            txtTotal.Clear();

            string text = listBox1.GetItemText(listBox1.SelectedItem);
            txtItemName.Text = text;

            query = "SELECT price FROM items WHERE name = @name";
            SqlParameter[] parameters = { new SqlParameter("@name", text) };
            DataTable dt = db.getData(query, parameters);
            if (dt.Rows.Count > 0)
            {
                txtPrice.Text = dt.Rows[0][0].ToString();
            }
        }

        private void txtQuantityUpDown_ValueChanged(object sender, EventArgs e)
        {
            Int64 quan = Int64.Parse(txtQuantityUpDown.Value.ToString());
            Int64 price = Int64.Parse(txtPrice.Text);
            txtTotal.Text = (quan * price).ToString();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Customer Bill";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "Total Payable Amount: " + labelTotalAmount.Text;
            printer.FooterSpacing = 15;
            printer.PrintDataGridView(guna2DataGridView1);

            // Reset totals after printing
            total = 0; // Reset total after printing
            guna2DataGridView1.Rows.Clear();
            labelTotalAmount.Text = "TK " + total;
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (txtTotal.Text != "0" && txtTotal.Text != "")
            {
                n = guna2DataGridView1.Rows.Add();
                guna2DataGridView1.Rows[n].Cells[0].Value = txtItemName.Text;
                guna2DataGridView1.Rows[n].Cells[1].Value = txtPrice.Text;
                guna2DataGridView1.Rows[n].Cells[2].Value = txtQuantityUpDown.Value;
                guna2DataGridView1.Rows[n].Cells[3].Value = txtTotal.Text;

                total += int.Parse(txtTotal.Text); // Update total
                labelTotalAmount.Text = "TK " + total;
            }
            else
            {
                MessageBox.Show("Minimum Quantity needs to be 1", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Check if the clicked row index is valid (non-header and non-out-of-bound row)
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    // Check if the selected row and cell contain a valid value
                    if (guna2DataGridView1.Rows[e.RowIndex].Cells[3].Value != null)
                    {
                        // Parse the amount from the selected cell
                        int amount = int.Parse(guna2DataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());

                        // Ask the user if they want to remove the selected item
                        DialogResult result = MessageBox.Show("Do you want to remove this item?", "Confirm Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            // If "Yes", remove the selected item and update the total amount
                            total -= amount;
                            labelTotalAmount.Text = "TK " + total;

                            // Remove the selected row from the DataGridView
                            guna2DataGridView1.Rows.RemoveAt(e.RowIndex);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Selected cell does not contain a valid amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., empty or invalid cells)
                MessageBox.Show("An error occurred while selecting the item amount: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
