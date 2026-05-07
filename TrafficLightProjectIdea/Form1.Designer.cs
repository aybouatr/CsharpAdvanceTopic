namespace TrafficLightProjectIdea
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
            this.userControl__Traffic_Light1 = new TrafficLightProjectIdea.UserControl__Traffic_Light();
            this.SuspendLayout();
            // 
            // userControl__Traffic_Light1
            // 
            this.userControl__Traffic_Light1.CurrentColor = TrafficLightProjectIdea.UserControl__Traffic_Light.Color.Red;
            this.userControl__Traffic_Light1.LightGreenTime = 0;
            this.userControl__Traffic_Light1.LightRedTime = 0;
            this.userControl__Traffic_Light1.LightYellowTime = 54;
            this.userControl__Traffic_Light1.Location = new System.Drawing.Point(86, 49);
            this.userControl__Traffic_Light1.Name = "userControl__Traffic_Light1";
            this.userControl__Traffic_Light1.Size = new System.Drawing.Size(108, 341);
            this.userControl__Traffic_Light1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 568);
            this.Controls.Add(this.userControl__Traffic_Light1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private UserControl__Traffic_Light userControl__Traffic_Light1;
    }
}

