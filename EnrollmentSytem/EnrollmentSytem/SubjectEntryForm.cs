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

namespace EnrollmentSytem
{
    public partial class SubjectEntryForm : Form
    {
        public SubjectEntryForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            RequisiteDataGridView.RowHeadersVisible = false;
            InitializeComboBoxes();
        }

        private void InitializeComboBoxes()
        {
            // Initialize combo boxes with default selections
            if (OfferingComboBox.Items.Count > 0) OfferingComboBox.SelectedIndex = 0;
            if (CategoryComboBox.Items.Count > 0) CategoryComboBox.SelectedIndex = 0;
            if (CourseCodeComboBox.Items.Count > 0) CourseCodeComboBox.SelectedIndex = 0;
        }

        #region Validation Methods
        private bool ValidateSubjectFields()
        {
            if (string.IsNullOrWhiteSpace(SubjectCodeTextBox.Text) ||
                string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ||
                string.IsNullOrWhiteSpace(UnitsTextBox.Text) ||
                string.IsNullOrWhiteSpace(OfferingComboBox.Text) ||
                string.IsNullOrWhiteSpace(CategoryComboBox.Text) ||
                string.IsNullOrWhiteSpace(CourseCodeComboBox.Text) ||
                string.IsNullOrWhiteSpace(CurriculumYearTextBox.Text))
            {
                ShowMessage("Please fill in all required fields.", "Validation Error", MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(UnitsTextBox.Text, out _))
            {
                ShowMessage("Please enter a valid number for units.", "Validation Error", MessageBoxIcon.Warning);
                UnitsTextBox.Focus();
                return false;
            }

            return true;
        }

        private bool IsSubjectCodeDuplicate(string subjectCode)
        {
            const string query = "SELECT COUNT(*) FROM SubjectFile WHERE SFSUBJCODE = @SFSUBJCODE";

            try
            {
                using (var db = new DatabaseConnection())
                {
                    db.Open();
                    int count = Convert.ToInt32(db.ExecuteScalar(query,
                        new OleDbParameter("@SFSUBJCODE", subjectCode)));
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Error checking for duplicate subject code: {ex.Message}", "Database Error");
                return false;
            }
        }
        #endregion

        #region Database Operations
        private void FetchSubjectData(string subjectCode)
        {
            const string query = @"SELECT SFSUBJCODE, SFSUBJDESC, SFSUBJUNITS, SFSUBJCATEGORY 
                                FROM SubjectFile 
                                WHERE SFSUBJCODE = @SFSUBJCODE";

            try
            {
                using (var db = new DatabaseConnection())
                {
                    db.Open();
                    DataTable result = db.ExecuteQuery(query,
                        new OleDbParameter("@SFSUBJCODE", subjectCode));

                    if (result.Rows.Count == 0)
                    {
                        ShowMessage("No subject found with the entered code.", "Not Found", MessageBoxIcon.Information);
                        return;
                    }

                    foreach (DataRow row in result.Rows)
                    {
                        string coPreValue = PreRequisiteRadioButton.Checked ? "Prerequisite" : "Corequisite";
                        RequisiteDataGridView.Rows.Add(
                            row["SFSUBJCODE"].ToString(),
                            row["SFSUBJDESC"].ToString(),
                            row["SFSUBJUNITS"].ToString(),
                            coPreValue
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Error fetching subject data: {ex.Message}", "Database Error");
            }
        }

        private void SaveSubjectToDatabase()
        {
            if (!ValidateSubjectFields()) return;

            const string checkQuery = "SELECT COUNT(*) FROM SubjectFile WHERE SFSUBJCODE = @SFSUBJCODE";
            const string insertQuery = @"INSERT INTO SubjectFile 
                                      (SFSUBJCODE, SFSUBJDESC, SFSUBJUNITS, SFSUBJREGOFRNG, 
                                       SFSUBJCATEGORY, SFSUBJSTATUS, SFSUBJCOURSECODE, SFSUBJCURRCODE) 
                                      VALUES 
                                      (@SFSUBJCODE, @SFSUBJDESC, @SFSUBJUNITS, @SFSUBJREGOFRNG, 
                                       @SFSUBJCATEGORY, @SFSUBJSTATUS, @SFSUBJCOURSECODE, @SFSUBJCURRCODE)";

            try
            {
                using (var db = new DatabaseConnection())
                {
                    db.Open();

                    // Check for duplicate
                    int count = Convert.ToInt32(db.ExecuteScalar(checkQuery,
                        new OleDbParameter("@SFSUBJCODE", SubjectCodeTextBox.Text)));

                    if (count > 0)
                    {
                        ShowMessage("This subject code already exists!", "Duplicate Error", MessageBoxIcon.Error);
                        return;
                    }

                    // Insert new record
                    db.ExecuteNonQuery(insertQuery,
                        new OleDbParameter("@SFSUBJCODE", SubjectCodeTextBox.Text),
                        new OleDbParameter("@SFSUBJDESC", DescriptionTextBox.Text),
                        new OleDbParameter("@SFSUBJUNITS", UnitsTextBox.Text),
                        new OleDbParameter("@SFSUBJREGOFRNG", OfferingComboBox.SelectedItem.ToString()),
                        new OleDbParameter("@SFSUBJCATEGORY", CategoryComboBox.SelectedItem.ToString()),
                        new OleDbParameter("@SFSUBJSTATUS", "AC"),
                        new OleDbParameter("@SFSUBJCOURSECODE", CourseCodeComboBox.Text),
                        new OleDbParameter("@SFSUBJCURRCODE", CurriculumYearTextBox.Text)
                    );

                    ShowMessage("Subject saved successfully!", "Success", MessageBoxIcon.Information);
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Error saving subject: {ex.Message}", "Error", MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Event Handlers
        private void SubjectCodeReqTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(SubjectCodeReqTextBox.Text))
                {
                    ShowMessage("Please enter a subject code.", "Validation Error", MessageBoxIcon.Warning);
                    return;
                }

                FetchSubjectData(SubjectCodeReqTextBox.Text.Trim());
                e.SuppressKeyPress = true;
            }
        }

        private void SubjectCodeTextBox_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SubjectCodeTextBox.Text))
            {
                if (IsSubjectCodeDuplicate(SubjectCodeTextBox.Text))
                {
                    SubjectCodeTextBox.BackColor = Color.LightPink;
                    ShowMessage("This subject code already exists!", "Duplicate Found", MessageBoxIcon.Warning);
                    SubjectCodeTextBox.Focus();
                }
                else
                {
                    SubjectCodeTextBox.BackColor = SystemColors.Window;
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveSubjectToDatabase();
        }

        private void ClearButton2_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            // Clear textboxes
            SubjectCodeTextBox.Clear();
            DescriptionTextBox.Clear();
            UnitsTextBox.Clear();
            CurriculumYearTextBox.Clear();
            SubjectCodeReqTextBox.Clear();

            // Reset comboboxes
            InitializeComboBoxes();

            // Clear radio buttons and datagrid
            PreRequisiteRadioButton.Checked = false;
            CoRequisiteRadioButton.Checked = false;
            RequisiteDataGridView.Rows.Clear();

            // Reset focus
            SubjectCodeTextBox.Focus();
        }

        private void SubjectScheduleEntryButton_Click(object sender, EventArgs e)
        {
            SubjectScheduleEntryForm subjectScheduleEntryForm = new SubjectScheduleEntryForm(); 
            subjectScheduleEntryForm.Show();
            this.Hide();
        }

        private void LogInPageButton_Click_1(object sender, EventArgs e)
        {
            Start start = new Start();
            start.Show();
            this.Hide();
        }

        #endregion

        #region Helper Methods
        private void ShowMessage(string message, string title, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }
        #endregion

        #region Unused Event Handlers
        private void label4_Click(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e) { }
        private void SubjectEntryForm_Load(object sender, EventArgs e) { }
        #endregion
    }
}