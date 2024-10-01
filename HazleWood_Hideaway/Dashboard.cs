using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SalesReport;

namespace HazleWood_Hideaway
{
    public partial class Dashboard : Form
    {
        public Dashboard(String user)
        {
            InitializeComponent();
            if (user == "Guest")
            {
                btnAddItems.Hide();
                btnRemove.Hide();
                btnUpdate.Hide();
                guna2Button1.Hide();
                btnSalesReport.Hide();
            }
            else if (user == "Admin")
            {
                btnAddItems.Show();
                btnRemove.Show();
                btnUpdate.Show();
                guna2Button1.Show();
                btnSalesReport.Show();
                btnPlaceOrder.Hide();
            }
        }
        

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            uC_AddItems1.Visible = false;
            uC_PlaceOrder1.Visible = false;
            uC_UpdateItems1.Visible = false;
            uC_RemoveItems1.Visible = false;
            uC_UserControl1.Visible = false;
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            uC_Welcome1.SendToBack();
            guna2Transition1.ShowSync(uC_PlaceOrder1);
            uC_PlaceOrder1.Visible = true;
            uC_PlaceOrder1.BringToFront();
        }

        private void btnAddItems_Click(object sender, EventArgs e)
        {
            uC_AddItems1.Visible = true;
            uC_AddItems1.BringToFront();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            uC_UpdateItems1.Visible = true;
            uC_UpdateItems1.BringToFront();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            uC_RemoveItems1.Visible = true;
            uC_RemoveItems1.BringToFront();
        }

        private void uC_RemoveItems1_Load(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            uC_UserControl1.Visible = true;
            uC_UserControl1.BringToFront();
        }

        private void uC_UpdateItems1_Load(object sender, EventArgs e)
        {

        }

        private void uC_AddItems1_Load(object sender, EventArgs e)
        {

        }

        private void uC_PlaceOrder1_Load(object sender, EventArgs e)
        {

        }

        private void uC_Welcome1_Load(object sender, EventArgs e)
        {

        }

        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            SalesReportForm reportForm = new SalesReportForm();
            reportForm.Show();
        }
    }
}
