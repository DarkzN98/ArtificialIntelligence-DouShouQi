﻿namespace DouShouQi
{
    partial class Game
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.giliranToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectedNullToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.giliranToolStripMenuItem,
            this.selectedNullToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(497, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // giliranToolStripMenuItem
            // 
            this.giliranToolStripMenuItem.Enabled = false;
            this.giliranToolStripMenuItem.Name = "giliranToolStripMenuItem";
            this.giliranToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.giliranToolStripMenuItem.Text = "Giliran: 0";
            // 
            // selectedNullToolStripMenuItem
            // 
            this.selectedNullToolStripMenuItem.Name = "selectedNullToolStripMenuItem";
            this.selectedNullToolStripMenuItem.Size = new System.Drawing.Size(89, 20);
            this.selectedNullToolStripMenuItem.Text = "Selected: null";
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 497);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Game";
            this.Text = "Game";
            this.Load += new System.EventHandler(this.Game_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem giliranToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectedNullToolStripMenuItem;
    }
}