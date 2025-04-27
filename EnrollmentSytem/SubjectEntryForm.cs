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
            RequisiteDataGridView.RowHeadersVisible = false;

            if (OfferingComboBox.Items.Count > 0)
            {
                OfferingComboBox.SelectedIndex = 0;
            }

            if (CategoryComboBox.Items.Count > 0)
            {
                CategoryComboBox.SelectedIndex = 0;
            }

            if (CourseCodeComboBox.Items.Count > 0)
            {
                CourseCodeComboBox.SelectedIndex = 0;
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
            // Connection string (ensure it's correct and points to your database)
            string connStr = @"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=C:\Users\kimpc\Desktop\dePaz_EnrollmentSystem\Enrollment_System.accdb;";

            // SQL Insert Query
            string query = "INSERT INTO SubjectFile (SFSUBJCODE, SFSUBJDESC, SFSUBJUNITS, SFSUBJREGOFRNG, SFSUBJCATEGORY, SFSUBJSTATUS, SFSUBJCOURSECODE, SFSUBJCURRCODE) " +
                           "VALUES (@SFSUBJCODE, @SFSUBJDESC, @SFSUBJUNITS, @SFSUBJREGOFRNG, @SFSUBJCATEGORY, @SFSUBJSTATUS, @SFSUBJCOURSECODE, @SFSUBJCURRCODE)";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(connStr))
                {
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        // Add parameters with values from input controls
                        cmd.Parameters.AddWithValue("@SFSUBJCODE", SubjectCodeTextBox.Text);
                        cmd.Parameters.AddWithValue("@SFSUBJDESC", DescriptionTextBox.Text);
                        cmd.Parameters.AddWithValue("@SFSUBJUNITS", UnitsTextBox.Text);
                        cmd.Parameters.AddWithValue("@SFSUBJREGOFRNG", OfferingComboBox.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@SFSUBJCATEGORY", CategoryComboBox.SelectedItem.ToString());
                        //cmd.Parameters.AddWithValue("@SFSUBJSTATUS", StatusComboBox.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@SFSUBJCOURSECODE", CourseCodeComboBox.Text);
                        cmd.Parameters.AddWithValue("@SFSUBJCURRCODE", CurriculumYearTextBox.Text);

                        // Open connection and execute query
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Notify user of success
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Subject added successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to add the subject.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

    }
}
