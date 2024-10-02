using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using DataAccess;

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
            /*Dashboard ds = new Dashboard("Guest",);
            ds.Show();
            this.Hide();*/
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtUsername.Text;  // Assuming txtUsername is the email field
            string password = txtPassword.Text;

            // SQL query to get the user's full name along with email and password validation
            string query = "SELECT FirstName, LastName, Email FROM Users WHERE Email = @Email AND Password = @Password";

            // Use SqlParameter to prevent SQL injection
            Database_2 dataAccess = new Database_2();
            SqlParameter[] parameters = {
        new SqlParameter("@Email", email),
        new SqlParameter("@Password", password)
    };

            DataTable dt = dataAccess.getData(query, parameters);

            // Check if the query returned any rows
            if (dt.Rows.Count > 0)
            {
                // Retrieve the user's full name
                string firstName = dt.Rows[0]["FirstName"].ToString();
                string lastName = dt.Rows[0]["LastName"].ToString();
                string fullName = firstName + " " + lastName;

                // Email domain check for access level
                if (email.EndsWith("@admin.com"))
                {
                    // Admin access: Pass "Admin" and full name to the Dashboard
                    Dashboard dashboard = new Dashboard("Admin", fullName);
                    dashboard.Show();
                    this.Hide();
                }
                else if (email.EndsWith("@cashier.com"))
                {
                    // Cashier access: Pass "Guest" and full name to the Dashboard
                    Dashboard dashboard = new Dashboard("Guest", fullName);
                    dashboard.Show();
                    this.Hide();
                }
                else if (email.EndsWith("@chef.com"))
                {
                    // Chef access (no need to pass full name for this case if not needed)
                    chef cf = new chef(fullName);
                    cf.Show();
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
