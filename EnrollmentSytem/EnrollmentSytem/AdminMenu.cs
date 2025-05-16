using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnrollmentSytem;

namespace EnrollmentSytem
{
    public partial class AdminMenu : Form
    {
        public AdminMenu()
        {
            InitializeComponent();
            LoadUserAccounts();
        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            Start start = new Start();
            start.Show();
            this.Hide();
        }
        private void SubjectsScheduleLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SubjectEntryForm subjectEntryForm = new SubjectEntryForm();
            subjectEntryForm.Show();
            this.Hide();
        }
        private void CreateButton_Click(object sender, EventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordTextBox.Text.Trim();
            string role = StudentRadioButton.Checked ? "Student" : "Teacher";

            // Validate inputs
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!StudentRadioButton.Checked && !TeacherRadioButton.Checked)
            {
                MessageBox.Show("Please select a role (Student or Teacher).", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if username already exists
            if (IsUsernameExists(username))
            {
                MessageBox.Show("Username already exists. Please choose a different username.",
                    "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (DatabaseConnection db = new DatabaseConnection())
            {
                try
                {
                    db.Open();
                    string query = "INSERT INTO UsersFile ([USERNAME], [PASSWORD], [ROLE]) VALUES (?, ?, ?)";
                    OleDbParameter[] parameters = new OleDbParameter[]
                    {
                        new OleDbParameter("@Username", username),
                        new OleDbParameter("@Password", password),
                        new OleDbParameter("@Role", role)
                    };

                    int result = db.ExecuteNonQuery(query, parameters);

                    if (result > 0)
                    {
                        MessageBox.Show("Account created successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ClearInputFields();
                        LoadUserAccounts();
                    }
                    else
                    {
                        MessageBox.Show("Failed to create account.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool IsUsernameExists(string username)
        {
            using (DatabaseConnection db = new DatabaseConnection())
            {
                try
                {
                    db.Open();
                    string query = "SELECT COUNT(*) FROM UsersFile WHERE USERNAME = ?";
                    OleDbParameter parameter = new OleDbParameter("@Username", username);

                    int count = Convert.ToInt32(db.ExecuteScalar(query, parameter));
                    return count > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error checking username: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        private void LoadUserAccounts()
        {
            using (DatabaseConnection db = new DatabaseConnection())
            {
                try
                {
                    db.Open();
                    string query = "SELECT USERNAME, PASSWORD, ROLE FROM UsersFile";
                    DataTable dataTable = db.ExecuteQuery(query);

                    AccManDataGridView.DataSource = null;
                    AccManDataGridView.DataSource = dataTable;

                    if (AccManDataGridView.Columns.Count > 0)
                    {
                        AccManDataGridView.Columns[0].HeaderText = "USERNAME";
                        AccManDataGridView.Columns[1].HeaderText = "PASSWORD";
                        AccManDataGridView.Columns[2].HeaderText = "ROLE";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading user accounts: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearInputFields()
        {
            UsernameTextBox.Clear();
            PasswordTextBox.Clear();
            StudentRadioButton.Checked = false;
            TeacherRadioButton.Checked = false;
        }

        private void DeleteButton_Click_1(object sender, EventArgs e)
        {
            if (AccManDataGridView.SelectedRows.Count == 0 && AccManDataGridView.SelectedCells.Count == 0)
            {
                MessageBox.Show("Please select a user to delete.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int rowIndex = AccManDataGridView.SelectedCells[0].RowIndex;
            string username = AccManDataGridView.Rows[rowIndex].Cells[0].Value.ToString();

            DialogResult result = MessageBox.Show($"Are you sure you want to delete the user '{username}'?",
                "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (DatabaseConnection db = new DatabaseConnection())
                {
                    try
                    {
                        db.Open();
                        string query = "DELETE FROM UsersFile WHERE USERNAME = ?";
                        OleDbParameter parameter = new OleDbParameter("@Username", username);

                        int deleteResult = db.ExecuteNonQuery(query, parameter);

                        if (deleteResult > 0)
                        {
                            MessageBox.Show("User deleted successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadUserAccounts();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete the user.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while deleting: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SubjAssignment subjAssignmentForm = new SubjAssignment();
            subjAssignmentForm.Show();
            this.Hide();
        }
    }
}
