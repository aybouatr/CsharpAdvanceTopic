using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoolClub
{

    public class AllEventArgs : EventArgs
    {
       public int FeesPerHour { get; set; }

        public string NameOfPlayer { get; set; }

        public double TotalFees { get; set; }

        public double TimeBySecond { get; set; }


        public AllEventArgs(int feesPerHour, string nameOfPlayer, double totalFees, double timeBySecond)
        {
            FeesPerHour = feesPerHour;
            NameOfPlayer = nameOfPlayer;
            TotalFees = totalFees;
            TimeBySecond = timeBySecond;
        }
    }

    public partial class uctrlPoolCluB : UserControl
    {

        public delegate void EventRaised(AllEventArgs args);

        public event EventRaised OnAllEventRaised;

        private bool _Start = false;
        private Timer time = new Timer();
        private int second ;
        private int munit;
        private double fees;
        

        public uctrlPoolCluB()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private string nameOfPlayer = "Player Name";
        [
            Category("Poll Config"),
            Description("Name of the player")
        ]
          public int FeesByHour { get ; set; }
        
        public string NameOfPlayer
        {
            get
            {
                return NamePlayer.Text;
            }

            set
            {
                NamePlayer.Text = value;
            }
        }

        public string NumberOfTable
        {
            get
            {
                return NameTable.Text;
            }
            set
            {
                NameTable.Text = value;
            }
        }

      

        private async Task StartCounter()
        {
            
            while (_Start)
            {
                await Task.Delay(1000);
                second++;
                if (second == 60)
                {
                    second = 0;
                    munit++;
                }
                Second.Text = second.ToString("00");
                this.munite.Text = munit.ToString("00");
            }
        }

        private async Task StopCounter()
        {
            _Start = false;
            await Task.Delay(1000);
           
        }

        private async Task Click(object sender, EventArgs e)
        {
                        //_Start = !_Start;
            if (_Start)
            {
                button1.Text = "Stop";
                _Start = false;
              await StopCounter();
            }
            else
            {
                _Start = true;
                button1.Text = "Start";
                await StartCounter();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Click(sender, e);
        }

        private void CalculteAllFees()
        {
            
            int totalSecound = (munit * 60) + second;
            double totalFees = totalSecound * (FeesByHour / 3600.0);
            OnAllEventRaised?.Invoke(new AllEventArgs(FeesByHour, NameOfPlayer, totalFees, totalSecound));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CalculteAllFees();
            second = 0;
            munit = 0;
            Second.Text = "00";
            this.munite.Text = "00";
            button1.Text = "Start";
            _Start = false;
        }
    }
}
