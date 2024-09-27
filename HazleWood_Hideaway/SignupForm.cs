using System;
using DataAccess;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HazleWood_Hideaway
{
    public partial class SignupForm : Form
    {
       
        public SignupForm()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

       

        private void btnLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }

        private void btnSingup_Click(object sender, EventArgs e)
        {
            string firstName = txtFirstname.Text;
            string lastName = txtLastname.Text;
            string email = txtEmail.Text + cmbRole.SelectedItem.ToString();  // Concatenate email with role
            string role = cmbRole.SelectedItem.ToString();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;
            DateTime dob = dtpDOB.Value;

            // Validate password confirmation
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Prepare SQL query to insert data into the Users table
            string query = $"INSERT INTO NUsers (FirstName, LastName, Email, Role, Password, DOB) " +
                           $"VALUES ('{firstName}', '{lastName}', '{email}', '{role}', '{password}', '{dob.ToString("yyyy-MM-dd")}')";

            // Call the setDta method from the function class to execute the query
            Database_2 dataAccess = new Database_2();
            dataAccess.setDta(query);
        }

        private void txtFirstname_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }
    }
}
