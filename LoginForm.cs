using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DESKTOP_I_21.View
{
    public partial class LoginForm : Form
    {

        private readonly Entities Entities = new Entities();
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var email = textBox1.Text;
            var password = textBox2.Text;

            if (!email.Contains("@") || !email.Contains(".") || email == "@." || email == ".@")
            {
                label4.Text = "Email is not valid format";
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                label4.Text = "Password cannot be empty";
                return;
            }


            var user = Entities.employee.Where(f => f.email == email && f.password == password && f.deleted_at == null).FirstOrDefault();

            if (user != null)
            {
                label4.Text = "";
                new AppContext(new MainForm(user.id));
                Close();
            } else
            {
                label4.Text = "Credential not found";
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
