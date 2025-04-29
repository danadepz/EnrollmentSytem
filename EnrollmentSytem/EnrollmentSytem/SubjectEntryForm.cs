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
        private void InitializeComboBox(ComboBox comboBox)
        {
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }

        public SubjectEntryForm()
        {
            InitializeComponent();
            RequisiteDataGridView.RowHeadersVisible = false;

            InitializeComboBox(OfferingComboBox);
            InitializeComboBox(CategoryComboBox);
            InitializeComboBox(CourseCodeComboBox);
        }

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
                MessageBox.Show("Some fields are missing.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void SubjectCodeReqTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(SubjectCodeReqTextBox.Text))
                {
                    MessageBox.Show("Please enter a subject code.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                FetchSubjectData(SubjectCodeReqTextBox.Text.Trim());

                e.SuppressKeyPress = true;
            }
        }


        private void FetchSubjectData(string subjectCode)
        {
            string connStr = @"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=C:\Users\kimpc\Desktop\dePaz_EnrollmentSystem\Enrollment_System.accdb;";
            string query = "SELECT SFSUBJCODE, SFSUBJDESC, SFSUBJUNITS, SFSUBJCATEGORY FROM SubjectFile WHERE SFSUBJCODE = @SFSUBJCODE";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SFSUBJCODE", subjectCode);

                        conn.Open();
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            bool dataFound = false;

                            while (reader.Read())
                            {
                                dataFound = true;

                                string coPreValue = PreRequisiteRadioButton.Checked ? "Prerequisite" : "Corequisite";

                                RequisiteDataGridView.Rows.Add(
                                    reader["SFSUBJCODE"].ToString(), 
                                    reader["SFSUBJDESC"].ToString(), 
                                    reader["SFSUBJUNITS"].ToString(),
                                    coPreValue 
                                );
                            }

                            if (!dataFound)
                            {
                                MessageBox.Show("No subject found with the entered code.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching subject data: {ex.Message}");
            }
        }


        private bool IsSubjectCodeDuplicate(string subjectCode)
        {
            string connStr = @"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=C:\Users\kimpc\Desktop\dePaz_EnrollmentSystem\Enrollment_System.accdb;";
            string query = "SELECT COUNT(*) FROM SubjectFile WHERE SFSUBJCODE = @SFSUBJCODE";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SFSUBJCODE", subjectCode);
                        conn.Open();
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking for duplicate subject code: {ex.Message}");
                return false;
            }
        }

        private void SubjectCodeTextBox_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SubjectCodeTextBox.Text) &&
                IsSubjectCodeDuplicate(SubjectCodeTextBox.Text))
            {
                SubjectCodeTextBox.BackColor = Color.LightPink;
                MessageBox.Show("This subject code already exists!", "Duplicate Found",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SubjectCodeTextBox.Focus();
            }
            else
            {
                SubjectCodeTextBox.BackColor = SystemColors.Window;
            }
        }
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!ValidateSubjectFields()) return;

            if (IsSubjectCodeDuplicate(SubjectCodeTextBox.Text))
            {
                MessageBox.Show("Duplicate Entries Found! This subject code already exists.",
                               "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connStr = @"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=C:\Users\kimpc\Desktop\dePaz_EnrollmentSystem\Enrollment_System.accdb;";

            string subjectQuery = "INSERT INTO SubjectFile (SFSUBJCODE, SFSUBJDESC, SFSUBJUNITS, SFSUBJREFOFRNG, SFSUBJCATEGORY, SFSUBJSTATUS, SFSUBJCOURSECODE, SFSUBJCURRCODE) " +
                                  "VALUES (@SFSUBJCODE, @SFSUBJDESC, @SFSUBJUNITS, @SFSUBJREFOFRNG, @SFSUBJCATEGORY, @SFSUBJSTATUS, @SFSUBJCOURSECODE, @SFSUBJCURRCODE)";

            string requisiteQuery = "INSERT INTO SubjectPreqFile (SUBJCODE, SUBJPRECODE, SUBJCATEGORY) " +
                                    "VALUES (@SUBJCODE, @SUBJPRECODE, @SUBJCATEGORY)";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    conn.Open();

                    //SubjectFile
                    using (OleDbCommand subjectCmd = new OleDbCommand(subjectQuery, conn))
                    {
                        subjectCmd.Parameters.AddWithValue("@SFSUBJCODE", SubjectCodeTextBox.Text);
                        subjectCmd.Parameters.AddWithValue("@SFSUBJDESC", DescriptionTextBox.Text);
                        subjectCmd.Parameters.AddWithValue("@SFSUBJUNITS", UnitsTextBox.Text);
                        subjectCmd.Parameters.AddWithValue("@SFSUBJREFOFRNG", OfferingComboBox.SelectedItem.ToString());
                        subjectCmd.Parameters.AddWithValue("@SFSUBJCATEGORY", CategoryComboBox.SelectedItem.ToString());
                        subjectCmd.Parameters.AddWithValue("@SFSUBJSTATUS", "AC");
                        subjectCmd.Parameters.AddWithValue("@SFSUBJCOURSECODE", CourseCodeComboBox.Text);
                        subjectCmd.Parameters.AddWithValue("@SFSUBJCURRCODE", CurriculumYearTextBox.Text);

                        subjectCmd.ExecuteNonQuery();
                    }

                    //DataGridView
                    List<string> prerequisites = new List<string>();
                    string subjCategory = PreRequisiteRadioButton.Checked ? "PR" : "CR";

                    foreach (DataGridViewRow row in RequisiteDataGridView.Rows)
                    {
                        if (row.Cells[0].Value != null)
                        {
                            prerequisites.Add(row.Cells[0].Value.ToString()); // Add the subject code
                        }
                    }

                    //Separate with a comma
                    string prerequisiteCodes = string.Join(",", prerequisites);

                    // Save to SubjectPreqFile if there are prerequisites
                    if (!string.IsNullOrWhiteSpace(prerequisiteCodes))
                    {
                        using (OleDbCommand requisiteCmd = new OleDbCommand(requisiteQuery, conn))
                        {
                            requisiteCmd.Parameters.AddWithValue("@SUBJCODE", SubjectCodeTextBox.Text);
                            requisiteCmd.Parameters.AddWithValue("@SUBJPRECODE", prerequisiteCodes);
                            requisiteCmd.Parameters.AddWithValue("@SUBJCATEGORY", subjCategory);

                            requisiteCmd.ExecuteNonQuery();
                        }
                    }

                    SubjectScheduleEntryButton.Enabled = true;

                    MessageBox.Show("Entries Recorded!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private void SubjectScheduleEntryButton_Click(object sender, EventArgs e)
        {
            SubjectScheduleEntryForm subjectScheduleEntryForm = new SubjectScheduleEntryForm();
            subjectScheduleEntryForm.Show();
            this.Hide();
        }

        private void ClearButton2_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is Panel panel)
                {
                    foreach (Control panelControl in panel.Controls)
                    {
                        if (panelControl is TextBox textBox)
                        {
                            textBox.Text = string.Empty;
                        }
                        else if (panelControl is ComboBox comboBox)
                        {
                            if (comboBox.Items.Count > 0)
                            {
                                comboBox.SelectedIndex = 0;
                            }
                        }
                        else if (panelControl is RadioButton radioButton)
                        {
                            radioButton.Checked = false;
                        }
                    }
                }
            }
            RequisiteDataGridView.Rows.Clear();
            SubjectCodeTextBox.Focus();
        }

        private void SubjectEntryForm_Load(object sender, EventArgs e)
        {

        }
    }
}
