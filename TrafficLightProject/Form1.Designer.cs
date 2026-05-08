namespace TrafficLightProject
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TrafficLight = new TrafficLightProject.uctlTrafficLight();
            this.button1 = new System.Windows.Forms.Button();
            this.uctlTrafficLight1 = new TrafficLightProject.uctlTrafficLight();
            this.uctlTrafficLight2 = new TrafficLightProject.uctlTrafficLight();
            this.uctlTrafficLight3 = new TrafficLightProject.uctlTrafficLight();
            this.SuspendLayout();
            // 
            // TrafficLight
            // 
            this.TrafficLight.CurrentColor = TrafficLightProject.uctlTrafficLight.TrafficLightColor.Yellow;
            this.TrafficLight.Location = new System.Drawing.Point(12, 23);
            this.TrafficLight.Name = "TrafficLight";
            this.TrafficLight.Size = new System.Drawing.Size(123, 351);
            this.TrafficLight.TabIndex = 0;
            this.TrafficLight.TimeGreen = 5;
            this.TrafficLight.TimeRed = 5;
            this.TrafficLight.TimeYellow = 3;
            this.TrafficLight.OnColorChanged += new System.Action<TrafficLightProject.uctlTrafficLight.TrafficLightColor>(this.TrafficLight_OnColorChanged);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button1.Location = new System.Drawing.Point(644, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(144, 88);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // uctlTrafficLight1
            // 
            this.uctlTrafficLight1.CurrentColor = TrafficLightProject.uctlTrafficLight.TrafficLightColor.Yellow;
            this.uctlTrafficLight1.Location = new System.Drawing.Point(170, 23);
            this.uctlTrafficLight1.Name = "uctlTrafficLight1";
            this.uctlTrafficLight1.Size = new System.Drawing.Size(123, 351);
            this.uctlTrafficLight1.TabIndex = 2;
            this.uctlTrafficLight1.TimeGreen = 5;
            this.uctlTrafficLight1.TimeRed = 5;
            this.uctlTrafficLight1.TimeYellow = 3;
            // 
            // uctlTrafficLight2
            // 
            this.uctlTrafficLight2.CurrentColor = TrafficLightProject.uctlTrafficLight.TrafficLightColor.Yellow;
            this.uctlTrafficLight2.Location = new System.Drawing.Point(317, 23);
            this.uctlTrafficLight2.Name = "uctlTrafficLight2";
            this.uctlTrafficLight2.Size = new System.Drawing.Size(123, 351);
            this.uctlTrafficLight2.TabIndex = 3;
            this.uctlTrafficLight2.TimeGreen = 5;
            this.uctlTrafficLight2.TimeRed = 5;
            this.uctlTrafficLight2.TimeYellow = 3;
            // 
            // uctlTrafficLight3
            // 
            this.uctlTrafficLight3.CurrentColor = TrafficLightProject.uctlTrafficLight.TrafficLightColor.Yellow;
            this.uctlTrafficLight3.Location = new System.Drawing.Point(483, 23);
            this.uctlTrafficLight3.Name = "uctlTrafficLight3";
            this.uctlTrafficLight3.Size = new System.Drawing.Size(123, 351);
            this.uctlTrafficLight3.TabIndex = 4;
            this.uctlTrafficLight3.TimeGreen = 5;
            this.uctlTrafficLight3.TimeRed = 5;
            this.uctlTrafficLight3.TimeYellow = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.uctlTrafficLight3);
            this.Controls.Add(this.uctlTrafficLight2);
            this.Controls.Add(this.uctlTrafficLight1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.TrafficLight);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private uctlTrafficLight TrafficLight;
        private System.Windows.Forms.Button button1;
        private uctlTrafficLight uctlTrafficLight1;
        private uctlTrafficLight uctlTrafficLight2;
        private uctlTrafficLight uctlTrafficLight3;
    }
}

