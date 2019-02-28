using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PluginJUmpCloudPgina
{
    public partial class Form1 : Form
    {
        public static pGina.Shared.Settings.pGinaDynamicSettings settings = new pGina.Shared.Settings.pGinaDynamicSettings();
        public string Uuid = "D0DC46C9-40A6-415E-9012-C9C9056B1A1F";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                settings.SetSetting("Repository", textBox1.Text);
                this.Close();
            }
        }
    }
}
