using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccess;
//using UserControl;

namespace HazleWood_Hideaway
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        private void btnGuest_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Dashboard ds = new Dashboard("Guest");
            ds.Show();
            this.Hide();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtUsername.Text;  // Assuming txtUsername is the email field
            string password = txtPassword.Text;

            // SQL query to check if the provided email and password exist in the database
            string query = $"SELECT Email FROM NUsers WHERE Email = '{email}' AND Password = '{password}'";

            // Use the existing function class to execute the query
            DataAccess.Database_2 dataAccess = new DataAccess.Database_2();
            DataSet ds = dataAccess.getdata(query);

            // Check if the query returned any rows
            if (ds.Tables[0].Rows.Count > 0)
            {
                // Email domain check for access level
                if (email.EndsWith("@admin.com"))
                {
                    // Admin access
                    Dashboard dashboard = new Dashboard("Admin");
                    dashboard.Show();
                    this.Hide();
                }
                else if (email.EndsWith("@cashier.com"))
                {
                    // Guest access
                    Dashboard dashboard = new Dashboard("Guest");
                    dashboard.Show();
                    this.Hide();
                }
                else
                {
                    // Default case for unknown email domains
                    MessageBox.Show("Access denied for this email domain.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Invalid credentials, show error message
                MessageBox.Show("Invalid Email or Password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnSignup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SignupForm sf = new SignupForm();
            sf.Show();
            this.Hide();
        }
    }
}
