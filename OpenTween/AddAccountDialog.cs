using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenTween
{
    public partial class AddAccountDialog : Form
    {
        internal string ConsumerKey;
        internal string ConsumerSecret;
        internal string AccessToken;
        internal string AccessTokenSecret;

        public AddAccountDialog(bool onlyConsumer = false)
        {
            InitializeComponent();
            if (onlyConsumer)
            {
                this.label3.Visible = false;
                this.label4.Visible = false;
                this.textBox3.Visible = false;
                this.textBox4.Visible = false;
                this.Height -= 31;
                this.label5.Visible = true;
            }
            else
            {
                this.label5.Visible = false;
            }
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            this.ConsumerKey = this.textBox1.Text;
            this.ConsumerSecret = this.textBox2.Text;
            this.AccessToken = this.textBox3.Text;
            this.AccessTokenSecret = this.textBox4.Text;
        }
    }
}
