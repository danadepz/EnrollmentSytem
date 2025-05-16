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
using EnrollmentSytem;


namespace EnrollmentSytem
{
    public partial class SubjAssignment : Form
    {
        private string currentUsername;
        private DataTable availableSubjects;
        private DataTable handledSubjects;
        private DataTable pendingAdditions; // To track subjects to be added

        public SubjAssignment()
        {
            InitializeComponent();
            Load += SubjAssignment_Load;
            UsernameTextBox.KeyDown += UsernameTextBox_KeyDown;
        }

        private void SubjAssignment_Load(object sender, EventArgs e)
        {
            ConfigureDataGridViews();
            InitializeDataTables();
            SaveButton.Enabled = false;
            DeleteButton.Enabled = false;
            AddButton.Enabled = false;
        }

        private void InitializeDataTables()
        {
            // Initialize main tables
            availableSubjects = new DataTable();
            availableSubjects.Columns.Add("Subject Code", typeof(string));
            availableSubjects.Columns.Add("Description", typeof(string));

            handledSubjects = new DataTable();
            handledSubjects.Columns.Add("Subject Code", typeof(string));
            handledSubjects.Columns.Add("Description", typeof(string));

            // Initialize pending additions table
            pendingAdditions = new DataTable();
            pendingAdditions.Columns.Add("Subject Code", typeof(string));
            pendingAdditions.Columns.Add("Description", typeof(string));

            // Set data sources
            SubjAvailableDataGridView.DataSource = availableSubjects;
            SubjHandledDataGridView.DataSource = handledSubjects;
        }

        private void ConfigureDataGridViews()
        {
            // Configure SubjAvailableDataGridView
            SubjAvailableDataGridView.AutoGenerateColumns = false;
            SubjAvailableDataGridView.Columns.Clear();

            // Add checkbox column
            DataGridViewCheckBoxColumn selectColumn = new DataGridViewCheckBoxColumn
            {
                Name = "SELECT",
                HeaderText = "SELECT",
                Width = 60
            };
            SubjAvailableDataGridView.Columns.Add(selectColumn);

            // Add Subject Code column
            DataGridViewTextBoxColumn codeColumn = new DataGridViewTextBoxColumn
            {
                Name = "SubjCodeColumn",
                HeaderText = "SUBJECT CODE",
                DataPropertyName = "Subject Code",
                Width = 150
            };
            SubjAvailableDataGridView.Columns.Add(codeColumn);

            // Add Description column
            DataGridViewTextBoxColumn descColumn = new DataGridViewTextBoxColumn
            {
                Name = "DescriptionColumn",
                HeaderText = "DESCRIPTION",
                DataPropertyName = "Description",
                Width = 250
            };
            SubjAvailableDataGridView.Columns.Add(descColumn);

            // Configure SubjHandledDataGridView
            SubjHandledDataGridView.AutoGenerateColumns = false;
            SubjHandledDataGridView.Columns.Clear();

            // Add Subject Code column
            DataGridViewTextBoxColumn codeColumn2 = new DataGridViewTextBoxColumn
            {
                Name = "SubjCodeColumn",
                HeaderText = "SUBJECT CODE",
                DataPropertyName = "Subject Code",
                Width = 150
            };
            SubjHandledDataGridView.Columns.Add(codeColumn2);

            // Add Description column
            DataGridViewTextBoxColumn descColumn2 = new DataGridViewTextBoxColumn
            {
                Name = "DescriptionColumn",
                HeaderText = "DESCRIPTION",
                DataPropertyName = "Description",
                Width = 250
            };
            SubjHandledDataGridView.Columns.Add(descColumn2);

            // Set selection modes
            SubjAvailableDataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            SubjHandledDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            SubjHandledDataGridView.MultiSelect = true;
        }

        private void LoadUserSubjects()
        {
            try
            {
                using (var db = new DatabaseConnection())
                {
                    db.Open();

                    // Verify user exists
                    string userCheckQuery = "SELECT COUNT(*) FROM UsersFile WHERE USERNAME = ?";
                    int userExists = Convert.ToInt32(db.ExecuteScalar(userCheckQuery,
                        new OleDbParameter("@username", currentUsername)));

                    if (userExists == 0)
                    {
                        MessageBox.Show("User not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Load handled subjects
                    string handledQuery = @"SELECT s.SFSUBJCODE as [Subject Code], s.SFSUBJDESC as [Description]
                                         FROM SubjectFile s
                                         INNER JOIN UserSubjects us ON s.SFSUBJCODE = us.USSUBJCODE
                                         WHERE us.USUSERNAME = ?";
                    handledSubjects = db.ExecuteQuery(handledQuery,
                        new OleDbParameter("@username", currentUsername));
                    SubjHandledDataGridView.DataSource = handledSubjects;

                    // Load available subjects
                    string availableQuery = @"SELECT s.SFSUBJCODE as [Subject Code], s.SFSUBJDESC as [Description]
                                           FROM SubjectFile s
                                           WHERE s.SFSUBJCODE NOT IN (
                                               SELECT USSUBJCODE FROM UserSubjects WHERE USUSERNAME = ?
                                           )";
                    availableSubjects = db.ExecuteQuery(availableQuery,
                        new OleDbParameter("@username", currentUsername));
                    SubjAvailableDataGridView.DataSource = availableSubjects;

                    // Enable buttons
                    SaveButton.Enabled = true;
                    AddButton.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading subjects: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            // Clear previous pending additions
            pendingAdditions.Clear();

            // Collect selected subjects
            foreach (DataGridViewRow row in SubjAvailableDataGridView.Rows)
            {
                var selectCell = row.Cells["SELECT"] as DataGridViewCheckBoxCell;
                if (selectCell != null && Convert.ToBoolean(selectCell.Value))
                {
                    string subjectCode = row.Cells["SubjCodeColumn"].Value.ToString();
                    string description = row.Cells["DescriptionColumn"].Value.ToString();

                    // Add to pending additions
                    pendingAdditions.Rows.Add(subjectCode, description);
                }
            }

            if (pendingAdditions.Rows.Count == 0)
            {
                MessageBox.Show("No subjects selected", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Add to handled subjects grid
            foreach (DataRow row in pendingAdditions.Rows)
            {
                // Check if already exists in handled subjects
                bool exists = handledSubjects.AsEnumerable()
                    .Any(r => r.Field<string>("Subject Code") == row.Field<string>("Subject Code"));

                if (!exists)
                {
                    handledSubjects.ImportRow(row);
                }
            }

            // Update the grid
            SubjHandledDataGridView.DataSource = handledSubjects;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (handledSubjects.Rows.Count == 0)
            {
                MessageBox.Show("No subjects to save", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (var db = new DatabaseConnection())
                {
                    db.Open();
                    db.BeginTransaction();

                    try
                    {
                        int successCount = 0;
                        foreach (DataRow row in handledSubjects.Rows)
                        {
                            string subjectCode = row["Subject Code"].ToString();

                            // Try to insert (will fail silently if duplicate exists)
                            try
                            {
                                string insertQuery = @"
                            INSERT INTO UserSubjects (USUSERNAME, USSUBJCODE) 
                            VALUES (?, ?)";

                                int rowsAffected = db.ExecuteNonQuery(insertQuery,
                                    new OleDbParameter("@username", currentUsername),
                                    new OleDbParameter("@subjectCode", subjectCode));

                                if (rowsAffected > 0) successCount++;
                            }
                            catch (OleDbException ex) when (ex.Message.Contains("duplicate"))
                            {
                                // Skip duplicate errors silently
                                continue;
                            }
                        }

                        db.CommitTransaction();

                        MessageBox.Show($"Successfully saved {successCount} subject assignments",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Refresh the data
                        LoadUserSubjects();
                    }
                    catch (Exception ex)
                    {
                        db.RollbackTransaction();
                        MessageBox.Show($"Error saving subjects: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (SubjHandledDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select subjects to remove", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Remove selected rows
            foreach (DataGridViewRow row in SubjHandledDataGridView.SelectedRows)
            {
                string subjectCode = row.Cells["SubjCodeColumn"].Value.ToString();
                DataRow[] rowsToDelete = handledSubjects.Select($"[Subject Code] = '{subjectCode}'");
                foreach (DataRow dr in rowsToDelete)
                {
                    handledSubjects.Rows.Remove(dr);
                }
            }

            // Update the grid
            SubjHandledDataGridView.DataSource = handledSubjects;
        }

        private void EnterButton_Click(object sender, EventArgs e)
        {
            currentUsername = UsernameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(currentUsername))
            {
                MessageBox.Show("Please enter a username", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            LoadUserSubjects();
        }

        private void UsernameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                EnterButton_Click(sender, e);
            }
        }

        private void SubjAvailableDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == SubjAvailableDataGridView.Columns["SELECT"].Index && e.RowIndex >= 0)
            {
                SubjAvailableDataGridView.EndEdit();
            }
        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            AdminMenu adminMenu = new AdminMenu();
            adminMenu.Show();
            this.Hide();
        }
    }
}