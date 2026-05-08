using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace TrafficLightProject
{
    public partial class uctlTrafficLight : UserControl
    {
        public uctlTrafficLight()
        {
            InitializeComponent();
        }

        private int Counter = 0;

        public event Action<TrafficLightColor> OnColorChanged;

        public enum TrafficLightColor
        {
            Red,
            Yellow,
            Green
        }

        private TrafficLightColor _currentColor;

        public TrafficLightColor CurrentColor
        {
            get { return _currentColor; }
            set
            {
                _currentColor = value;
                UpdateTrafficLightUI(value);
            }
        }

        public int TimeRed { get; set; } = 5;
        public int TimeYellow { get; set; } = 2;
        public int TimeGreen { get; set; } = 5;

        private void UpdateTrafficLightUI(TrafficLightColor color)
        {
            switch (color)
            {
                case TrafficLightColor.Green:
                    pictureBox1.Image = Properties.Resources.Green;
                    break;

                case TrafficLightColor.Yellow:
                    pictureBox1.Image = Properties.Resources.Yellow;
                    break;

                case TrafficLightColor.Red:
                    pictureBox1.Image = Properties.Resources.Red;
                    break;
            }
        }

        private int GetTimeForColor(TrafficLightColor color)
        {
          switch (color)
            {
                case TrafficLightColor.Green:
                    return TimeGreen;
                case TrafficLightColor.Yellow:
                    return TimeYellow;
                case TrafficLightColor.Red:
                    return TimeRed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }
        }

        private async Task RunTimerForCurrentLight(int time)
        {
            Counter = time;

            while (Counter > 0)
            {
                TraffickCounter.Text = Counter.ToString();
                await Task.Delay(1000);
                Counter--;
            }
        }

        private async Task ChangeColor()
        {
            while (true)
            {
                // ✅ use CurrentColor (not local variable)
                int time = GetTimeForColor(CurrentColor);

                await RunTimerForCurrentLight(time);

                // next color
                CurrentColor = (TrafficLightColor)(((int)CurrentColor + 1) % 3);

                OnColorChanged?.Invoke(CurrentColor);
            }
        }

        // ✅ Start method (fixed)
        public async void Start()
        {
            CurrentColor = TrafficLightColor.Red; // ✅ set initial color
            await ChangeColor();
        }
    }
}