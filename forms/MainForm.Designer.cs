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
			this.SelectFirstRecipeButton = new System.Windows.Forms.Button();
			this.SaveChartButton = new System.Windows.Forms.Button();
			this.ProductionPlanPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.LoadChartButton = new System.Windows.Forms.Button();
			this.MainPanel.SuspendLayout();
			this.ButtonPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainPanel
			// 
			this.MainPanel.AutoScroll = true;
			this.MainPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MainPanel.Controls.Add(this.ButtonPanel);
			this.MainPanel.Controls.Add(this.ProductionPlanPanel);
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
			this.ButtonPanel.Controls.Add(this.SelectFirstRecipeButton);
			this.ButtonPanel.Controls.Add(this.SaveChartButton);
			this.ButtonPanel.Controls.Add(this.LoadChartButton);
			this.ButtonPanel.Location = new System.Drawing.Point(3, 3);
			this.ButtonPanel.Name = "ButtonPanel";
			this.ButtonPanel.Size = new System.Drawing.Size(330, 33);
			this.ButtonPanel.TabIndex = 0;
			// 
			// SelectFirstRecipeButton
			// 
			this.SelectFirstRecipeButton.AutoSize = true;
			this.SelectFirstRecipeButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.SelectFirstRecipeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SelectFirstRecipeButton.Location = new System.Drawing.Point(3, 3);
			this.SelectFirstRecipeButton.Name = "SelectFirstRecipeButton";
			this.SelectFirstRecipeButton.Size = new System.Drawing.Size(136, 27);
			this.SelectFirstRecipeButton.TabIndex = 2;
			this.SelectFirstRecipeButton.Text = "Select First Recipe";
			this.SelectFirstRecipeButton.UseVisualStyleBackColor = true;
			this.SelectFirstRecipeButton.Click += new System.EventHandler(this.SelectFirstRecipeButton_Click);
			// 
			// SaveChartButton
			// 
			this.SaveChartButton.AutoSize = true;
			this.SaveChartButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.SaveChartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SaveChartButton.Location = new System.Drawing.Point(145, 3);
			this.SaveChartButton.Name = "SaveChartButton";
			this.SaveChartButton.Size = new System.Drawing.Size(88, 27);
			this.SaveChartButton.TabIndex = 3;
			this.SaveChartButton.Text = "Save Chart";
			this.SaveChartButton.UseVisualStyleBackColor = true;
			this.SaveChartButton.Click += new System.EventHandler(this.SaveChartButton_Click);
			// 
			// ProductionPlanPanel
			// 
			this.ProductionPlanPanel.AutoSize = true;
			this.ProductionPlanPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ProductionPlanPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.ProductionPlanPanel.Location = new System.Drawing.Point(0, 39);
			this.ProductionPlanPanel.Margin = new System.Windows.Forms.Padding(0);
			this.ProductionPlanPanel.Name = "ProductionPlanPanel";
			this.ProductionPlanPanel.Size = new System.Drawing.Size(0, 0);
			this.ProductionPlanPanel.TabIndex = 1;
			// 
			// LoadChartButton
			// 
			this.LoadChartButton.AutoSize = true;
			this.LoadChartButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.LoadChartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LoadChartButton.Location = new System.Drawing.Point(239, 3);
			this.LoadChartButton.Name = "LoadChartButton";
			this.LoadChartButton.Size = new System.Drawing.Size(88, 27);
			this.LoadChartButton.TabIndex = 4;
			this.LoadChartButton.Text = "Load Chart";
			this.LoadChartButton.UseVisualStyleBackColor = true;
			this.LoadChartButton.Click += new System.EventHandler(this.LoadChartButton_Click);
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
		private System.Windows.Forms.Button SelectFirstRecipeButton;
		private System.Windows.Forms.FlowLayoutPanel ProductionPlanPanel;
		private System.Windows.Forms.Button SaveChartButton;
		private System.Windows.Forms.Button LoadChartButton;
	}
}