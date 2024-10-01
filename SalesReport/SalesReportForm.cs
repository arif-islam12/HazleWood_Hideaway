using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using DataAccess;
using DGVPrinterHelper;

namespace SalesReport
{
    public partial class SalesReportForm : Form
    {
        Database_2 db = new Database_2();

        public SalesReportForm()
        {
            InitializeComponent();
        }

        private void SalesReport_Load(object sender, EventArgs e)
        {
            // Any initialization if needed
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            DateTime startDate = dtpStartDate.Value;
            DateTime endDate = dtpEndDate.Value;

            if (endDate < startDate)
            {
                MessageBox.Show("End date cannot be earlier than start date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = "SELECT InvoiceNumber, Date, TotalAmount, TableNumber, ItemDetails FROM SalesHistory WHERE Date BETWEEN @StartDate AND @EndDate";
            SqlParameter[] parameters = {
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate)
            };

            DataTable dt = db.getData(query, parameters);
            guna2DataGridView1.DataSource = dt;

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("No records found for the selected date range.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintSalesReport();
        }

        private void PrintSalesReport()
        {
            if (guna2DataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No data to print.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Calculate the total sales amount
            decimal totalSales = 0;
            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                if (row.Cells["TotalAmount"].Value != null)
                {
                    totalSales += Convert.ToDecimal(row.Cells["TotalAmount"].Value);
                }
            }

            // Create DGVPrinter instance
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Sales Report";
            printer.SubTitle = $"From: {dtpStartDate.Value.ToShortDateString()} To: {dtpEndDate.Value.ToShortDateString()}";
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = $"Total Sales: TK {totalSales}"; // Total Sales in the footer
            printer.FooterSpacing = 15;

            // Print the DataGridView
            printer.PrintDataGridView(guna2DataGridView1);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
