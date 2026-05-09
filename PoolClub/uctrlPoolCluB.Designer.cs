namespace PoolClub
{
    partial class uctrlPoolCluB
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.NamePlayer = new System.Windows.Forms.Label();
            this.NameTable = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.Second = new System.Windows.Forms.Label();
            this.munite = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::PoolClub.Properties.Resources.pool_balls;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.NamePlayer);
            this.panel1.Location = new System.Drawing.Point(6, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(260, 226);
            this.panel1.TabIndex = 0;
            // 
            // NamePlayer
            // 
            this.NamePlayer.AutoSize = true;
            this.NamePlayer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.NamePlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NamePlayer.ForeColor = System.Drawing.Color.Lime;
            this.NamePlayer.Location = new System.Drawing.Point(66, 172);
            this.NamePlayer.Name = "NamePlayer";
            this.NamePlayer.Size = new System.Drawing.Size(97, 31);
            this.NamePlayer.TabIndex = 2;
            this.NamePlayer.Text = "Player";
            // 
            // NameTable
            // 
            this.NameTable.AutoSize = true;
            this.NameTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.NameTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameTable.ForeColor = System.Drawing.Color.Lime;
            this.NameTable.Location = new System.Drawing.Point(7, -2);
            this.NameTable.Name = "NameTable";
            this.NameTable.Size = new System.Drawing.Size(87, 31);
            this.NameTable.TabIndex = 1;
            this.NameTable.Text = "Table";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Olive;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(277, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 43);
            this.button1.TabIndex = 1;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Olive;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(278, 85);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 43);
            this.button2.TabIndex = 2;
            this.button2.Text = "End";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Second
            // 
            this.Second.AutoSize = true;
            this.Second.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.Second.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Second.ForeColor = System.Drawing.Color.Lime;
            this.Second.Location = new System.Drawing.Point(324, 156);
            this.Second.Name = "Second";
            this.Second.Size = new System.Drawing.Size(46, 31);
            this.Second.TabIndex = 3;
            this.Second.Text = "00";
            // 
            // munite
            // 
            this.munite.AutoSize = true;
            this.munite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.munite.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.munite.ForeColor = System.Drawing.Color.Lime;
            this.munite.Location = new System.Drawing.Point(276, 156);
            this.munite.Name = "munite";
            this.munite.Size = new System.Drawing.Size(46, 31);
            this.munite.TabIndex = 4;
            this.munite.Text = "00";
            // 
            // uctrlPoolCluB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.munite);
            this.Controls.Add(this.Second);
            this.Controls.Add(this.NameTable);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Name = "uctrlPoolCluB";
            this.Size = new System.Drawing.Size(382, 236);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label NameTable;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label NamePlayer;
        private System.Windows.Forms.Label Second;
        private System.Windows.Forms.Label munite;
    }
}
