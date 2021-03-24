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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.MainPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.ButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.AddStepButton = new System.Windows.Forms.Button();
			this.ClearStepsButton = new System.Windows.Forms.Button();
			this.LoadChartButton = new System.Windows.Forms.Button();
			this.SaveChartButton = new System.Windows.Forms.Button();
			this.PlanPanel = new System.Windows.Forms.Panel();
			this.MainPanel.SuspendLayout();
			this.ButtonPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainPanel
			// 
			this.MainPanel.AutoScroll = true;
			this.MainPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MainPanel.Controls.Add(this.ButtonPanel);
			this.MainPanel.Controls.Add(this.PlanPanel);
			this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.MainPanel.Location = new System.Drawing.Point(0, 0);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(1184, 636);
			this.MainPanel.TabIndex = 0;
			this.MainPanel.WrapContents = false;
			// 
			// ButtonPanel
			// 
			this.ButtonPanel.AutoSize = true;
			this.ButtonPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ButtonPanel.Controls.Add(this.AddStepButton);
			this.ButtonPanel.Controls.Add(this.ClearStepsButton);
			this.ButtonPanel.Controls.Add(this.LoadChartButton);
			this.ButtonPanel.Controls.Add(this.SaveChartButton);
			this.ButtonPanel.Location = new System.Drawing.Point(3, 3);
			this.ButtonPanel.Name = "ButtonPanel";
			this.ButtonPanel.Size = new System.Drawing.Size(367, 33);
			this.ButtonPanel.TabIndex = 0;
			// 
			// AddStepButton
			// 
			this.AddStepButton.AutoSize = true;
			this.AddStepButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.AddStepButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.AddStepButton.Location = new System.Drawing.Point(3, 3);
			this.AddStepButton.Name = "AddStepButton";
			this.AddStepButton.Size = new System.Drawing.Size(76, 27);
			this.AddStepButton.TabIndex = 2;
			this.AddStepButton.Text = "Add Step";
			this.AddStepButton.UseVisualStyleBackColor = true;
			this.AddStepButton.Click += new System.EventHandler(this.AddStepButton_Click);
			// 
			// ClearStepsButton
			// 
			this.ClearStepsButton.AutoSize = true;
			this.ClearStepsButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClearStepsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.ClearStepsButton.Location = new System.Drawing.Point(85, 3);
			this.ClearStepsButton.Name = "ClearStepsButton";
			this.ClearStepsButton.Size = new System.Drawing.Size(91, 27);
			this.ClearStepsButton.TabIndex = 5;
			this.ClearStepsButton.Text = "Clear Steps";
			this.ClearStepsButton.UseVisualStyleBackColor = true;
			this.ClearStepsButton.Click += new System.EventHandler(this.ClearStepsButton_Click);
			// 
			// LoadChartButton
			// 
			this.LoadChartButton.AutoSize = true;
			this.LoadChartButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.LoadChartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LoadChartButton.Location = new System.Drawing.Point(182, 3);
			this.LoadChartButton.Name = "LoadChartButton";
			this.LoadChartButton.Size = new System.Drawing.Size(88, 27);
			this.LoadChartButton.TabIndex = 4;
			this.LoadChartButton.Text = "Load Chart";
			this.LoadChartButton.UseVisualStyleBackColor = true;
			this.LoadChartButton.Click += new System.EventHandler(this.LoadChartButton_Click);
			// 
			// SaveChartButton
			// 
			this.SaveChartButton.AutoSize = true;
			this.SaveChartButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.SaveChartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SaveChartButton.Location = new System.Drawing.Point(276, 3);
			this.SaveChartButton.Name = "SaveChartButton";
			this.SaveChartButton.Size = new System.Drawing.Size(88, 27);
			this.SaveChartButton.TabIndex = 3;
			this.SaveChartButton.Text = "Save Chart";
			this.SaveChartButton.UseVisualStyleBackColor = true;
			this.SaveChartButton.Click += new System.EventHandler(this.SaveChartButton_Click);
			// 
			// PlanPanel
			// 
			this.PlanPanel.AutoSize = true;
			this.PlanPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.PlanPanel.Location = new System.Drawing.Point(3, 42);
			this.PlanPanel.Name = "PlanPanel";
			this.PlanPanel.Size = new System.Drawing.Size(0, 0);
			this.PlanPanel.TabIndex = 1;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1184, 636);
			this.Controls.Add(this.MainPanel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.Text = "Visual Satisfactory Calculator";
			this.MainPanel.ResumeLayout(false);
			this.MainPanel.PerformLayout();
			this.ButtonPanel.ResumeLayout(false);
			this.ButtonPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel MainPanel;
		private System.Windows.Forms.FlowLayoutPanel ButtonPanel;
		private System.Windows.Forms.Button AddStepButton;
		private System.Windows.Forms.Button SaveChartButton;
		private System.Windows.Forms.Button LoadChartButton;
		public System.Windows.Forms.Panel PlanPanel;
		private System.Windows.Forms.Button ClearStepsButton;
	}
}