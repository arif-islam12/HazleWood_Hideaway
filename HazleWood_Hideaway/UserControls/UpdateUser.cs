using DataAccess;
using Guna.UI2.WinForms;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace HazleWood_Hideaway.UserControls
{
    public partial class UpdateUser : Form
    {
        private int userId;
        Database_2 dataAccess = new Database_2();
        public UpdateUser(int userId, string firstName, string lastName, string email, string role, DateTime dob)
        {
            InitializeComponent();
            this.userId = userId;

            // Pre-fill the fields with user data
            txtFirstname.Text = firstName;
            txtLastname.Text = lastName;
            txtEmail.Text = email;
            cmbRole.SelectedItem = role;
            dtpDOB.Value = dob;
        }

        private void btnUpadate_Click(object sender, EventArgs e)
        {

        }

        private void UpdateUser_Load(object sender, EventArgs e)
        {
           // loadData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            string firstName = txtFirstname.Text;
            string lastName = txtLastname.Text;
            string email = txtEmail.Text;
            string role = cmbRole.SelectedItem.ToString();
            DateTime dob = dtpDOB.Value;
            string password = txtPassword.Text;  // New password field
            string confirmPassword = txtConfirmPassword.Text;  // Password confirmation field

            // Validate password confirmation
            if (!string.IsNullOrEmpty(password) && password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Prepare the SQL query to update user details
            string query = "UPDATE Users SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Role = @Role, DOB = @DOB";

            // Only include password in the query if the user has entered a new one
            if (!string.IsNullOrEmpty(password))
            {
                query += ", Password = @Password";
            }

            query += " WHERE UserID = @UserID";

            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Role", role);
            cmd.Parameters.AddWithValue("@DOB", dob.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@UserID", userId);

            // Add password parameter only if it's not empty
            if (!string.IsNullOrEmpty(password))
            {
                cmd.Parameters.AddWithValue("@Password", password);
            }

            // Execute the query using the data access class
            // Execute the query using the data access class
            dataAccess.setDta(cmd);

            MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            UserUpdated?.Invoke(); // Notify subscribers that a user was updated
            this.Close(); // Close the form after successful update
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public event Action UserUpdated; // Event to notify when user data is updated

    }
}
