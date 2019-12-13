using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DouShouQi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //player duluan
            new Game(2).Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //ai duluan
            new Game(1).Show();
        }
    }
}
