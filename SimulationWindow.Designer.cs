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
            this.SuspendLayout();
            // 
            // SimulationClock
            // 
            this.SimulationClock.Tick += new System.EventHandler(this.SimulationClock_Tick);
            // 
            // SimulationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "SimulationWindow";
            this.Text = "SimulationWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer SimulationClock;
    }
}