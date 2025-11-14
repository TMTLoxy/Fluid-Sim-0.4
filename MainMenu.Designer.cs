namespace Fluid_Sim_0._4
{
    partial class MainMenu
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
            this.StartSimulation_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // StartSimulation_btn
            // 
            this.StartSimulation_btn.Location = new System.Drawing.Point(360, 192);
            this.StartSimulation_btn.Name = "StartSimulation_btn";
            this.StartSimulation_btn.Size = new System.Drawing.Size(75, 23);
            this.StartSimulation_btn.TabIndex = 0;
            this.StartSimulation_btn.Text = "Start";
            this.StartSimulation_btn.UseVisualStyleBackColor = true;
            this.StartSimulation_btn.Click += new System.EventHandler(this.StartSimulation_btn_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.StartSimulation_btn);
            this.Name = "MainMenu";
            this.Text = "MainMenu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button StartSimulation_btn;
    }
}