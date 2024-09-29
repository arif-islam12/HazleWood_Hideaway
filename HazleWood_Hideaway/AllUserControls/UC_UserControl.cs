using DataAccess;
using HazleWood_Hideaway.UserControls;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HazleWood_Hideaway.AllUserControls
{
    public partial class UC_UserControl : UserControl
    {
        Database_2 dataAccess = new Database_2();

        private int selectedUserId = -1;  // Store the selected user's ID
        private string firstName;
        private string lastName;
        private string email;
        private string role;
        private DateTime dob;

        public UC_UserControl()
        {
            InitializeComponent();
            LoadUsers();
        }

        public void LoadUsers()
        {
            string query = "SELECT UserID, FirstName, LastName, Email, Role, DOB FROM Users";
            DataTable usersTable = dataAccess.getData(query);
            guna2DataGridView1.DataSource = usersTable;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedUserId != -1)
            {
                // Open the UpdateForm with the selected user's data
                UpdateUser updateForm = new UpdateUser(selectedUserId, firstName, lastName, email, role, dob);
                updateForm.ShowDialog();

                // Reload the data grid after the update
                LoadUsers();
            }
            else
            {
                MessageBox.Show("Please select a user to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)  // Ensure the clicked cell is valid
            {
                DataGridViewRow row = guna2DataGridView1.Rows[e.RowIndex];

                // Check for DBNull and safely convert to int
                if (row.Cells["UserID"].Value != DBNull.Value)
                {
                    selectedUserId = Convert.ToInt32(row.Cells["UserID"].Value);
                }
                else
                {
                    selectedUserId = -1; // Reset to -1 if the value is DBNull
                    MessageBox.Show("Selected user ID is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                firstName = row.Cells["FirstName"].Value?.ToString() ?? string.Empty;
                lastName = row.Cells["LastName"].Value?.ToString() ?? string.Empty;
                email = row.Cells["Email"].Value?.ToString() ?? string.Empty;
                role = row.Cells["Role"].Value?.ToString() ?? string.Empty;

                // Check if DOB is DBNull and handle accordingly
                if (row.Cells["DOB"].Value != DBNull.Value)
                {
                    dob = Convert.ToDateTime(row.Cells["DOB"].Value);
                }
                else
                {
                    dob = DateTime.MinValue; // or set a default value
                    MessageBox.Show("Selected user's date of birth is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SignupForm signupForm = new SignupForm();
            signupForm.ShowDialog();
            LoadUsers(); // Refresh the user list after adding a new user
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (selectedUserId == -1)
            {
                MessageBox.Show("Please select a user to remove.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirm the action
            var confirmResult = MessageBox.Show("Are you sure you want to remove this user?", "Confirm Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                // Prepare SQL query to delete the user
                string query = "DELETE FROM Users WHERE UserID = @UserID";

                // Create a SqlCommand object
                SqlCommand cmd = new SqlCommand(query);
                cmd.Parameters.AddWithValue("@UserID", selectedUserId);

                // Call the method from your Database class to execute the query
                dataAccess.setDta(cmd);

                MessageBox.Show("User removed successfully!");

                // Refresh the DataGridView to show updated data
                LoadUsers();

                // Reset selectedUserId
                selectedUserId = -1;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string query = "SELECT UserID, FirstName, LastName, Email, Role, DOB FROM Users WHERE Email LIKE @Email";
            SqlParameter[] parameters = { new SqlParameter("@Email", txtSearch.Text + "%") };
            DataTable dt = dataAccess.getData(query, parameters); // Works with the updated method
            guna2DataGridView1.DataSource = dt;
        }
    }
}
