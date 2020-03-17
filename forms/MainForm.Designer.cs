namespace VisualSatisfactoryCalculator.forms
{
	partial class MainForm
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
			this.MainPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.ButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.AddRecipeButton = new System.Windows.Forms.Button();
			this.CurrentChart = new System.Windows.Forms.PictureBox();
			this.MainPanel.SuspendLayout();
			this.ButtonPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.CurrentChart)).BeginInit();
			this.SuspendLayout();
			// 
			// MainPanel
			// 
			this.MainPanel.AutoSize = true;
			this.MainPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MainPanel.Controls.Add(this.ButtonPanel);
			this.MainPanel.Controls.Add(this.CurrentChart);
			this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainPanel.Location = new System.Drawing.Point(0, 0);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(1184, 636);
			this.MainPanel.TabIndex = 0;
			// 
			// ButtonPanel
			// 
			this.ButtonPanel.AutoSize = true;
			this.ButtonPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ButtonPanel.Controls.Add(this.AddRecipeButton);
			this.ButtonPanel.Location = new System.Drawing.Point(3, 3);
			this.ButtonPanel.Name = "ButtonPanel";
			this.ButtonPanel.Size = new System.Drawing.Size(81, 29);
			this.ButtonPanel.TabIndex = 0;
			// 
			// AddRecipeButton
			// 
			this.AddRecipeButton.Location = new System.Drawing.Point(3, 3);
			this.AddRecipeButton.Name = "AddRecipeButton";
			this.AddRecipeButton.Size = new System.Drawing.Size(75, 23);
			this.AddRecipeButton.TabIndex = 0;
			this.AddRecipeButton.Text = "Add Recipe";
			this.AddRecipeButton.UseVisualStyleBackColor = true;
			this.AddRecipeButton.Click += new System.EventHandler(this.AddRecipeButton_Click);
			// 
			// CurrentChart
			// 
			this.CurrentChart.Image = global::VisualSatisfactoryCalculator.Properties.Resources.DefaultImage;
			this.CurrentChart.Location = new System.Drawing.Point(90, 3);
			this.CurrentChart.Name = "CurrentChart";
			this.CurrentChart.Size = new System.Drawing.Size(1082, 621);
			this.CurrentChart.TabIndex = 1;
			this.CurrentChart.TabStop = false;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1184, 636);
			this.Controls.Add(this.MainPanel);
			this.Name = "MainForm";
			this.Text = "Visual Satisfactory Calculator";
			this.MainPanel.ResumeLayout(false);
			this.MainPanel.PerformLayout();
			this.ButtonPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.CurrentChart)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel MainPanel;
		private System.Windows.Forms.FlowLayoutPanel ButtonPanel;
		private System.Windows.Forms.PictureBox CurrentChart;
		private System.Windows.Forms.Button AddRecipeButton;
	}
}