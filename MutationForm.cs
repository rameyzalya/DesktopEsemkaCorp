using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DESKTOP_I_21.View
{
    public partial class MutationForm : Form
    {
        private readonly Entities entities = new Entities();
        private readonly employee employee;

        public MutationForm(int id)
        {
            employee = entities.employee.Find(id);
            InitializeComponent();

            employeeBindingSource.Clear();
            employeeBindingSource.Add(employee);

            LoadData();
        }

        private void LoadData()
        {
            var job = employee.position.Where(e => e.deleted_at == null).First().job;
            var jl = job.job_level_id;
            jobBindingSource.Clear();
            jobBindingSource.DataSource = entities.job.Where(e => e.deleted_at == null && e.id != job.id && e.job_level_id == jl && e.position.Where(f => f.deleted_at == null).Count() < e.head_count).OrderBy(e => e.job_level_id).ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new AppContext(new MainForm(employee.id));
            Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void employeeBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (employeeBindingSource.Current is employee employee)
            {
                var position = employee.position.Where(f => f.deleted_at == null).First();
                textBox1.Text = position.job.department.name;
                textBox2.Text = position.job.name;
                textBox3.Text = position.job.job_level.name;
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is job job)
            {
                if (e.ColumnIndex == departmentDataGridViewTextBoxColumn.Index)
                {
                    e.Value = job.department.name;
                }

                if (e.ColumnIndex == applyButton.Index)
                {
                    var applied = job.mutation.Any(f => f.deleted_at == null && f.employee_id == employee.id);
                    e.Value = applied ? "Applied" : "Apply";
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is job job)
            {
                if (e.ColumnIndex == applyButton.Index)
                {
                    var applied = job.mutation.FirstOrDefault(f => f.deleted_at == null && f.employee_id == employee.id);
                    if (applied != null)
                    {
                        var status = applied.status == "P" ? "Pending" : (applied.status == "I" ? "In Progress" : "Approved");
                        MessageBox.Show($"Already applied before. Application status is {status}", "Job Applied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } else
                    {
                        entities.mutation.Add(new mutation
                        {
                            created_at = DateTime.Now,
                            employee_id = employee.id,
                            job_id = job.id,
                            status = "P",
                        });

                        if (entities.SaveChanges() > 0)
                        {
                            MessageBox.Show("Your application has been sent to HRD, please wait for a minute", "Job Application", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                        } else
                        {
                            MessageBox.Show("Cannot send application, try again later", "Job Application", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
}
