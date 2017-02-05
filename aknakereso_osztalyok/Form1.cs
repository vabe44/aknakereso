using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aknakereso_osztalyok
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Jatek j;

        private void Form1_Load(object sender, EventArgs e)
        {
            j = new Jatek(10, 10, 10);
            j.Lerajzol(panel1);
        }
    }
}
