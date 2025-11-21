namespace Fluid_Sim_0._4
{
    partial class SimulationWindow
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
            this.components = new System.ComponentModel.Container();
            this.SimulationClock = new System.Windows.Forms.Timer(this.components);
            this.EndSim_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SimulationClock
            // 
            this.SimulationClock.Enabled = true;
            this.SimulationClock.Tick += new System.EventHandler(this.SimulationClock_Tick);
            // 
            // EndSim_btn
            // 
            this.EndSim_btn.Location = new System.Drawing.Point(713, 12);
            this.EndSim_btn.Name = "EndSim_btn";
            this.EndSim_btn.Size = new System.Drawing.Size(75, 23);
            this.EndSim_btn.TabIndex = 0;
            this.EndSim_btn.Text = "End";
            this.EndSim_btn.UseVisualStyleBackColor = true;
            this.EndSim_btn.Click += new System.EventHandler(this.EndSim_btn_Click);
            // 
            // SimulationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.EndSim_btn);
            this.Name = "SimulationWindow";
            this.Text = "SimulationWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer SimulationClock;
        private System.Windows.Forms.Button EndSim_btn;
    }
}