using EnrollmentSytem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

        private void ClearButton_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SubjectEntryForm subjectEntryForm = new SubjectEntryForm();
            subjectEntryForm.Show();
            this.Hide();

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void AmPmComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SubjectScheduleEntryForm_Load(object sender, EventArgs e)
        {

        }
    }
}
