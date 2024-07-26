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
    public partial class MainForm : Form
    {
        private readonly employee employee;
        private readonly Entities entities = new Entities();

        public MainForm(int id)
        {
            employee = entities.employee.Find(id);
            InitializeComponent();

            label1.Text = label1.Text.Replace("Employee", employee.name);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new AppContext(new LoginForm());
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new AppContext(new ProfileForm(employee.id));
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new AppContext(new MutationForm(employee.id));
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new AppContext(new PromotionForm(employee.id));
            Close();
        }
    }
}
