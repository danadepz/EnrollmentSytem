using EnrollmentSytem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnrollmentSytem
{
    public partial class SubjectScheduleEntryForm : Form
    {
        public SubjectScheduleEntryForm()
        {
            InitializeComponent();
        }

        #region Helper Methods

        private string GetDaysString()
        {
            var days = new StringBuilder(6);
            if (MondayCheckBox.Checked) days.Append("M");
            if (TuesdayCheckBox.Checked) days.Append("T");
            if (WednesdayCheckBox.Checked) days.Append("W");
            if (ThursdayCheckBox.Checked) days.Append("H");
            if (FridayCheckBox.Checked) days.Append("F");
            if (SaturdayCheckBox.Checked) days.Append("S");
            return days.ToString();
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(SubjectEdpCodeTextBox.Text) ||
                string.IsNullOrWhiteSpace(SubjectCodeTextBox.Text) ||
                string.IsNullOrWhiteSpace(RoomTextBox.Text) ||
                string.IsNullOrWhiteSpace(MaxSizeTextBox.Text) ||
                string.IsNullOrWhiteSpace(SectionTextBox.Text) ||
                string.IsNullOrWhiteSpace(SchoolYearTextBox.Text) ||
                StartTimeHourComboBox.SelectedIndex == -1 ||
                StartTimeMinComboBox.SelectedIndex == -1 ||
                EndTimeHourComboBox.SelectedIndex == -1 ||
                EndTimeMinComboBox.SelectedIndex == -1 ||
                AmPmComboBox.SelectedIndex == -1)
            {
                ShowValidationError("Please fill in all required fields.");
                return false;
            }

            if (!int.TryParse(MaxSizeTextBox.Text.Trim(), out _))
            {
                ShowValidationError("Max Size must be a valid number.");
                return false;
            }

            // Validate school year format (should be "YYYY-YYYY" or just "YYYY")
            if (!Regex.IsMatch(SchoolYearTextBox.Text.Trim(), @"^\d{4}(-\d{4})?$"))
            {
                ShowValidationError("School Year must be in format YYYY or YYYY-YYYY");
                return false;
            }

            if (!SubjectCodeExists(SubjectCodeTextBox.Text.Trim()))
            {
                ShowValidationError("Invalid subject code. It does not exist in SubjectFile.");
                return false;
            }

            return true;
        }

        private bool SubjectCodeExists(string subjectCode)
        {
            using (var db = new DatabaseConnection())
            {
                db.Open();
                var result = db.ExecuteScalar(
                    "SELECT COUNT(*) FROM SubjectFile WHERE SFSUBJCODE = ?",
                    new OleDbParameter("@subjectCode", subjectCode));
                return Convert.ToInt32(result) > 0;
            }
        }

        private void ShowValidationError(string message)
        {
            MessageBox.Show(message, "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ClearForm()
        {
            // Clear text boxes
            SubjectEdpCodeTextBox.Clear();
            SubjectCodeTextBox.Clear();
            RoomTextBox.Clear();
            MaxSizeTextBox.Clear();
            SectionTextBox.Clear();
            SchoolYearTextBox.Clear();

            // Reset combo boxes
            StartTimeHourComboBox.SelectedIndex = -1;
            StartTimeMinComboBox.SelectedIndex = -1;
            EndTimeHourComboBox.SelectedIndex = -1;
            EndTimeMinComboBox.SelectedIndex = -1;
            AmPmComboBox.SelectedIndex = -1;

            // Uncheck all day checkboxes
            MondayCheckBox.Checked = false;
            TuesdayCheckBox.Checked = false;
            WednesdayCheckBox.Checked = false;
            ThursdayCheckBox.Checked = false;
            FridayCheckBox.Checked = false;
            SaturdayCheckBox.Checked = false;
        }

        #endregion

        #region Event Handlers

        private void ClearButton_Click(object sender, EventArgs e) => ClearForm();

        private void button1_Click(object sender, EventArgs e)
        {
            new SubjectEntryForm().Show();
            this.Hide();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            try
            {
                SaveSchedule();
                MessageBox.Show("Schedule saved successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving schedule: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveSchedule()
        {
            using (var db = new DatabaseConnection())
            {
                db.Open();

                string amPm = AmPmComboBox.SelectedItem.ToString();
                string startTime = $"{StartTimeHourComboBox.SelectedItem}:{StartTimeMinComboBox.SelectedItem}";
                string endTime = $"{EndTimeHourComboBox.SelectedItem}:{EndTimeMinComboBox.SelectedItem}";

                const string insertQuery = @"INSERT INTO SubjectSchedFile 
            (SSFEDPCODE, SSFSUBJCODE, SSFSTARTTIME, SSFENDTIME, SSFDAYS, 
             SSFROOM, SSFMAXSIZE, SSFSTATUS, SSFXM, SSFSECTION, SSFSCHOOLYEAR) 
            VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                var parameters = new OleDbParameter[]
                {
            new OleDbParameter("@edpCode", OleDbType.VarChar) { Value = SubjectEdpCodeTextBox.Text.Trim() },
            new OleDbParameter("@subjCode", OleDbType.VarChar) { Value = SubjectCodeTextBox.Text.Trim() },
            new OleDbParameter("@startTime", OleDbType.VarChar) { Value = startTime },
            new OleDbParameter("@endTime", OleDbType.VarChar) { Value = endTime },
            new OleDbParameter("@days", OleDbType.VarChar) { Value = GetDaysString() },
            new OleDbParameter("@room", OleDbType.VarChar) { Value = RoomTextBox.Text.Trim() },
            new OleDbParameter("@maxSize", OleDbType.Integer) { Value = Convert.ToInt32(MaxSizeTextBox.Text.Trim()) },
            new OleDbParameter("@status", OleDbType.VarChar) { Value = "AC" },
            new OleDbParameter("@xm", OleDbType.VarChar) { Value = amPm },
            new OleDbParameter("@section", OleDbType.VarChar) { Value = SectionTextBox.Text.Trim() },
            new OleDbParameter("@schoolYear", OleDbType.Integer) { Value = Convert.ToInt32(SchoolYearTextBox.Text.Trim().Split('-')[0]) }
                };

                db.ExecuteNonQuery(insertQuery, parameters);
            }
        }

        private void SubjectScheduleEntryForm_Load(object sender, EventArgs e)
        {
            if (AmPmComboBox.Items.Count == 0)
            {
                AmPmComboBox.Items.AddRange(new object[] { "AM", "PM" });
            }
        }

        // Empty event handlers kept as requested
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void AmPmComboBox_SelectedIndexChanged(object sender, EventArgs e) { }

        #endregion
    }
}