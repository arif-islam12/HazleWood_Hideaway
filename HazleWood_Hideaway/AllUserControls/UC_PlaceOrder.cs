using DataAccess;
using DGVPrinterHelper;
using Guna.UI2.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HazleWood_Hideaway.AllUserControls
{
    public partial class UC_PlaceOrder : UserControl
    {
        function fn = new function();
        string query;
        public UC_PlaceOrder()
        {
            InitializeComponent();
        }

        private void comboCatagory_SelectedIndexChanged(object sender, EventArgs e)
        {
            String catagory = comboCatagory.Text;
            query = "select name from items where catagory='" + catagory + "'";
            DataSet ds = fn.getdata(query);
            showItemList(query);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            String catagory = comboCatagory.Text;
            query = "select name from items where catagory='" + catagory + "' and name like'" + txtSearch.Text + "%'";
            DataSet ds = fn.getdata(query);
            showItemList(query);
        }
        private void showItemList(String query)
        {
            listBox1.Items.Clear();
            DataSet ds = fn.getdata(query);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                listBox1.Items.Add(ds.Tables[0].Rows[i][0].ToString());
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtQuantityUpDown.ResetText();
            txtTotal.Clear();

            String text = listBox1.GetItemText(listBox1.SelectedItem);
            txtItemName.Text = text;

            query = "select price from items where name='" + text + "'";
            DataSet ds = fn.getdata(query);
            txtPrice.Text = ds.Tables[0].Rows[0][0].ToString();
            try
            {
                txtPrice.Text = ds.Tables[0].Rows[0][0].ToString();
            }
            catch { }
        }

        private void txtQuantityUpDown_ValueChanged(object sender, EventArgs e)
        {
            Int64 quan = Int64.Parse(txtQuantityUpDown.Value.ToString());
            Int64 price = Int64.Parse(txtPrice.Text);
            txtTotal.Text = (quan * price).ToString();
        }
        protected int n, total = 0;
        int amount;

        private void btnRemove_Click(object sender, EventArgs e)
        {
            // Check if anow is selected
            if (guna2DataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No items to remove.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // Exit if there are no rows
            }

            // Check if any row is selected
            if (guna2DataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item to remove.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Exit if no row is selected

                return;

            }

            try
            {
                // If the total amount is greater than 0, proceed with removal
                if (total > 0)
                {
                    total -= amount;
                    labelTotalAmount.Text = "TK " + total;
                }
                else
                {
                    MessageBox.Show("Can't remove any more items, total is already zero.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Remove the selected row from the DataGridView
                int selectedIndex = guna2DataGridView1.SelectedRows[0].Index;

                // Ensure a valid row is selected and it's not the last row unless explicitly selected
                if (selectedIndex >= 0)
                {
                    guna2DataGridView1.Rows.RemoveAt(selectedIndex);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while removing the item: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Customer Bill";
            printer.SubTitle = String.Format("Date: {0}", DateTime.Now.Date);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "Total Payable Amount:" + labelTotalAmount.Text;
            printer.FooterSpacing = 15;
            printer.PrintDataGridView(guna2DataGridView1);

            total = 0;
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
                total = total + int.Parse(txtTotal.Text);
                labelTotalAmount.Text = "TK " + total;
            }
            else
            {
                MessageBox.Show("Minimum Quantity need to be 1", "information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        amount = int.Parse(guna2DataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
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
