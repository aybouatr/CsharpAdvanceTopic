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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void uctrlPoolCluB1_OnAllEventRaised(AllEventArgs args)
        {
            MessageBox.Show($"Player Name: {args.NameOfPlayer}\nFees Per Hour: {args.FeesPerHour}\nTotal Fees: {args.TotalFees}\nTime By Second: {args.TimeBySecond}");
        }

        private void uctrlPoolCluB3_OnAllEventRaised(AllEventArgs args)
        {
            MessageBox.Show($"Player Name: {args.NameOfPlayer}\nFees Per Hour: {args.FeesPerHour}\nTotal Fees: {args.TotalFees}\nTime By Second: {args.TimeBySecond}");

        }

        private void uctrlPoolCluB2_OnAllEventRaised(AllEventArgs args)
        {
            MessageBox.Show($"Player Name: {args.NameOfPlayer}\nFees Per Hour: {args.FeesPerHour}\nTotal Fees: {args.TotalFees}\nTime By Second: {args.TimeBySecond}");
        }

        private void uctrlPoolCluB6_OnAllEventRaised(AllEventArgs args)
        {
            MessageBox.Show($"Player Name: {args.NameOfPlayer}\nFees Per Hour: {args.FeesPerHour}\nTotal Fees: {args.TotalFees}\nTime By Second: {args.TimeBySecond}");
        }

        private void uctrlPoolCluB4_OnAllEventRaised(AllEventArgs args)
        {
            MessageBox.Show($"Player Name: {args.NameOfPlayer}\nFees Per Hour: {args.FeesPerHour}\nTotal Fees: {args.TotalFees}\nTime By Second: {args.TimeBySecond}");
        }

        private void uctrlPoolCluB5_OnAllEventRaised(AllEventArgs args)
        {
            MessageBox.Show($"Player Name: {args.NameOfPlayer}\nFees Per Hour: {args.FeesPerHour}\nTotal Fees: {args.TotalFees}\nTime By Second: {args.TimeBySecond}");
        }
    }
}
