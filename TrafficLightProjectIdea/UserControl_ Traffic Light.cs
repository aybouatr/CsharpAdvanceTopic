using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
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

        static private Timer CounterTime;
       
        public int counter;
        public Color CurrentColor { get;  set; }

        public int LightGreenTime { get; set; }

         public int LightYellowTime { get; set; }

        public int LightRedTime { get; set; } 
       
        public event EventHandler RedLightClicked;
        public  UserControl__Traffic_Light()
        {
            InitializeComponent();
        }



        private int GetRightTime(Color color)
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
       
      



        private Color GetNextColor(Color current, Color previous)
        {
            switch (current)
            {
                case Color.Green:
                    return Color.Yellow;
                case Color.Yellow:
                    return Color.Red;
                case Color.Red:
                    return Color.Green;
                default:
                    throw new ArgumentException("Invalid color");
            }
        }

        public async Task StartTrafficLight()
        {
            while (true)
            {
                Color previous = CurrentColor;
                await  RunLight(CurrentColor, GetRightTime(CurrentColor));
                CurrentColor = GetNextColor(CurrentColor, previous);
          
            }
        }

        private async Task RunLight(Color color, int duration)
        {
            RedLightClicked?.Invoke(this, EventArgs.Empty);
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

        private async void UserControl__Traffic_Light_Load(object sender, EventArgs e)
        {
             await StartTrafficLight();
            
        }
    }
}
