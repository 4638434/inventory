using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PassBook
{
    public partial class FormRecord : Form
    {
        public String addr = "";
        public String login = "";
        public String password = "";

        public FormRecord()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            addr = textBoxSite.Text;
            login = textBoxLogin.Text;
            password = textBoxPassword.Text;
        }

        private void FormRecord_Load(object sender, EventArgs e)
        {
            textBoxSite.Text = addr;
            textBoxLogin.Text = login;
            textBoxPassword.Text = password;
        }
    }
}
