using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficLightProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void TrafficLight_OnColorChanged(uctlTrafficLight.TrafficLightColor obj)
        {
            MessageBox.Show($"Color changed to \n");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TrafficLight.Start();
            uctlTrafficLight1.Start();
            uctlTrafficLight2.Start();
            uctlTrafficLight3.Start();
        }
    }
}
