using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficLightProjectIdea
{
    public partial class UserControl__Traffic_Light : UserControl
    {
        public enum Color
        {
            Green,
            Yellow,
            Red
        }

        static public Timer CounterTime;

        public Color CurrentColor { get;  set; }

        public int LightGreenTime { get; set; }

         public int LightYellowTime { get; set; }

        public int LightRedTime { get; set; } 
       
        public event EventHandler RedLightClicked;
        public  UserControl__Traffic_Light()
        {
            InitializeComponent();
        }

        private int GetRightColor(Color color)
        {
            switch (color)
            {
                case Color.Green:
                    return LightGreenTime;
                case Color.Yellow:
                    return LightYellowTime;
                case Color.Red:
                    return LightRedTime;
                default:
                    throw new ArgumentException("Invalid color");
            }
        }

        public  void StartTrafficLight()
        {
            //while (true)
            //{
                 RunLight(CurrentColor, GetRightColor(CurrentColor));
                
            //}
        }

        private async Task RunLight(Color color, int duration)
        {
           
           switch (color)
            {
                case Color.Green:
                    pictureBox1.Image = Properties.Resources.Green;
                    break;
                case Color.Yellow:
                    pictureBox1.Image = Properties.Resources.Yellow;
                    break;
                case Color.Red:
                    pictureBox1.Image = Properties.Resources.Red;
                    break;
            }
            await Task.Delay(duration * 1000);
        }

        private void RedLight_Click(object sender, EventArgs e)
        {
            if (RedLightClicked != null)
                 RedLightClicked?.Invoke(this, EventArgs.Empty);
        }

        private void UserControl__Traffic_Light_Load(object sender, EventArgs e)
        {
            //StartTrafficLight();
        }
    }
}
