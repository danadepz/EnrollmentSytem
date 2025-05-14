using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace EnrollmentSytem
{
    public partial class Start : Form
    {
        public Start()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Start_KeyDown);
            UsernameTextBox.Focus();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Use the DatabaseConnection class directly
                using (DatabaseConnection dbConnection = new DatabaseConnection())
                {
                    dbConnection.Open();

                    string query = "SELECT ROLE FROM UsersFile WHERE USERNAME = ? AND PASSWORD = ?";
                    OleDbParameter[] parameters = new OleDbParameter[]
                    {
                        new OleDbParameter("USERNAME", username),
                        new OleDbParameter("PASSWORD", password)
                    };

                    object roleObj = dbConnection.ExecuteScalar(query, parameters);

                    if (roleObj != null)
                    {
                        string role = roleObj.ToString();

                        switch (role)
                        {
                            case "Admin":
                                SubjectEntryForm adminForm = new SubjectEntryForm();
                                adminForm.Show();
                                this.Hide();
                                break;

                            case "Teacher":
                                StudentGrades teacherForm = new StudentGrades();
                                teacherForm.Show();
                                this.Hide();
                                break;

                            case "Student":
                                StudentEntryForm studentForm = new StudentEntryForm();
                                studentForm.Show();
                                this.Hide();
                                break;

                            default:
                                MessageBox.Show("Invalid role detected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Start_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Button1_Click(sender, e); // Call the login logic
                e.SuppressKeyPress = true; // Prevent the Enter key from triggering other default behaviors
            }
        }
    }
}