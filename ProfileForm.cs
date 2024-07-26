using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DESKTOP_I_21.View
{
    public partial class ProfileForm : Form
    {
        private readonly Entities entities = new Entities();
        private readonly int id;
        private readonly employee employee;
        public ProfileForm(int id, int? supid = null)
        {
            this.id = id;
            if (supid != null)
            {
                employee = entities.employee.Find(supid);
            } else
            {
                employee = entities.employee.Find(id);
            }
            InitializeComponent();

            employeeBindingSource.Clear();
            employeeBindingSource.Add(employee);

            employeeBindingSource1.Clear();
            var jobid = employee.position.Where(g => g.deleted_at == null).First().job.id;
            var supervisor = employee.position.Where(g => g.deleted_at == null).First().job.supervisor_job_id;
            var subordinate =  entities.employee.Where(e => e.deleted_at == null && e.id != (supid ?? id) && e.position.Any(f => f.job.supervisor_job_id == jobid && f.deleted_at == null)).ToList();
            employeeBindingSource1.DataSource = subordinate;

            bindingSource1.Clear();
            var bff = entities.employee.Where(e => e.deleted_at == null && e.id != (supid ?? id) && e.position.Any(f => f.job.supervisor_job_id == supervisor && f.deleted_at == null)).ToList();
            bindingSource1.DataSource = bff;

            positionBindingSource.Clear();
            positionBindingSource.DataSource = employee.position.OrderByDescending(e => e.job.job_level_id).ToList();
        }

        private void employeeBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (employeeBindingSource.Current is employee employee)
            {
                positionTextbox.Text = employee.position.Where(f => f.deleted_at == null).First().job.name;
                jobLevelTextbo.Text = employee.position.Where(f => f.deleted_at == null).First().job.job_level.name;
                departmentTextbox.Text = employee.position.Where(f => f.deleted_at == null).First().job.department.name;
                linkLabel1.Text = employee.position.Where(f => f.deleted_at == null).First().job.department.name;

                var supid = employee.position.Where(f => f.deleted_at == null).First().job.supervisor_job_id;
                if (supid != null)
                {
                    linkLabel1.Text = entities.employee.Where(f => f.deleted_at == null && f.position.Any(g => g.deleted_at == null && g.job_id == supid)).First().name;
                } else
                {
                    linkLabel1.Text = "No Supervisor";
                    linkLabel1.Enabled = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new AppContext(new MainForm(id));
            Close();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is employee employee)
            {
                if (e.ColumnIndex == positionDataGridViewTextBoxColumn.Index)
                {
                    e.Value = employee.position.Where(f => f.deleted_at == null).First().job.name;
                }
            }
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView2.Rows[e.RowIndex].DataBoundItem is employee employee)
            {
                if (e.ColumnIndex == positionDataGridViewTextBoxColumn1.Index)
                {
                    e.Value = employee.position.Where(f => f.deleted_at == null).First().job.name;
                }
            }
        }

        private void dataGridView3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView3.Rows[e.RowIndex].DataBoundItem is position position)
            {
                if (e.ColumnIndex == jobDataGridViewTextBoxColumn.Index)
                {
                    e.Value = position.job.name;
                }

                if (e.ColumnIndex == departmentColumn.Index)
                {
                    e.Value = position.job.department.name;
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (employeeBindingSource.Current is employee employee)
            {
                var supid = employee.position.Where(f => f.deleted_at == null).First().job.supervisor_job_id;
                if (supid != null)
                {
                    var emp = entities.employee.Where(f => f.deleted_at == null && f.position.Any(g => g.deleted_at == null && g.job_id == supid)).First().id;
                    new AppContext(new ProfileForm(id, emp));
                    Close();
                }
            }
        }

        private void employeeBindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        }
    }
}
