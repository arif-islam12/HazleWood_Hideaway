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
        Database_2 db = new Database_2();
        string query;
        protected int n;
        protected int total = 0;
        protected int invoiceNumber = 0; // Invoice number variable

        public UC_PlaceOrder()
        {
            InitializeComponent();
            invoiceNumber = db.GetMaxInvoiceNumber(); // Retrieve the latest invoice number from the database
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
            if (!string.IsNullOrEmpty(txtPrice.Text))
            {
                int quantity = (int)txtQuantityUpDown.Value;
                decimal price = decimal.Parse(txtPrice.Text);
                txtTotal.Text = (quantity * price).ToString();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.Rows.Count == 1)
            {
                MessageBox.Show("No items in the cart to send to the chef.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (string.IsNullOrEmpty(txtTableNumber.Text))
            {
                MessageBox.Show("Please enter a valid table number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                invoiceNumber++; // Increment the invoice number

                DGVPrinter printer = new DGVPrinter();
                printer.Title = "Customer Bill";
                printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date);
                printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                printer.PageNumbers = true;
                printer.PageNumberInHeader = false;
                printer.PorportionalColumns = true;
                printer.HeaderCellAlignment = StringAlignment.Near;
                printer.Footer = $"Invoice Number: {invoiceNumber}\nTotal Payable Amount: {labelTotalAmount.Text}\nTable Number: {txtTableNumber.Text}";
                printer.FooterSpacing = 15;

                // Set the page size to an invoice size page
                printer.PrintPreviewDataGridView(guna2DataGridView1);

                // Store the invoice and order details in the database
                StoreInvoiceAndOrder();
                send_chef();

                total = 0;
                guna2DataGridView1.Rows.Clear();
                labelTotalAmount.Text = "TK " + total;
            }
        }

        private void StoreInvoiceAndOrder()
        {
            string query = "INSERT INTO SalesHistory (InvoiceNumber, Date, TotalAmount, TableNumber, ItemDetails) VALUES (@InvoiceNumber, @Date, @TotalAmount, @TableNumber, @ItemDetails)";
            try
            {
                string itemDetails = "";
                for (int i = 0; i < guna2DataGridView1.Rows.Count; i++)
                {
                    if (guna2DataGridView1.Rows[i].IsNewRow) continue;

                    itemDetails += $"{guna2DataGridView1.Rows[i].Cells[0].Value} (Qty: {guna2DataGridView1.Rows[i].Cells[2].Value}), ";
                }
                // Remove the last comma and space
                if (itemDetails.Length > 2) itemDetails = itemDetails.Substring(0, itemDetails.Length - 2);

                SqlParameter[] parameters = {
                    new SqlParameter("@InvoiceNumber", invoiceNumber),
                    new SqlParameter("@Date", DateTime.Now),
                    new SqlParameter("@TotalAmount", labelTotalAmount.Text.Replace("TK ", "").Trim()),
                    new SqlParameter("@TableNumber", txtTableNumber.Text),
                    new SqlParameter("@ItemDetails", itemDetails)
                };

                db.setDta(query, parameters);
                MessageBox.Show("Order details saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving the invoice: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                total += int.Parse(txtTotal.Text);
                labelTotalAmount.Text = "TK " + total;
            }
            else
            {
                MessageBox.Show("Minimum Quantity needs to be 1", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (guna2DataGridView1.Rows[e.RowIndex].Cells[3].Value != null)
                {
                    int amount = int.Parse(guna2DataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                    DialogResult result = MessageBox.Show("Do you want to remove this item?", "Confirm Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        total -= amount;
                        labelTotalAmount.Text = "TK " + total;
                        guna2DataGridView1.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
        }
        private void send_chef()
        {
            // Check if the DataGridView has any rows


            // Check if txtTableNumber is initialized


            string query = "INSERT INTO chef_order (item_name, quantity, table_number) VALUES (@item_name, @quantity, @table_number)";

            try
            {
                for (int i = 0; i < guna2DataGridView1.Rows.Count; i++)
                {
                    // Ensure the row is not the new row placeholder
                    if (guna2DataGridView1.Rows[i].IsNewRow) continue;

                    SqlParameter[] parameters = {
                        new SqlParameter("@item_name", guna2DataGridView1.Rows[i].Cells[0].Value?.ToString() ?? string.Empty),
                        new SqlParameter("@quantity", guna2DataGridView1.Rows[i].Cells[2].Value?.ToString() ?? string.Empty),
                        new SqlParameter("@table_number", txtTableNumber.Text) // Ensure this is your table number textbox
                    };

                    // Call to the Database_2 method
                    db.setDta(query, parameters);
                }

                MessageBox.Show("Order sent to the chef successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while sending the order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
