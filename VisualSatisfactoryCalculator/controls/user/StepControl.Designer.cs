using VisualSatisfactoryCalculator.satisfactory.Utility;

namespace VisualSatisfactoryCalculator.controls.user
{
	partial class StepControl
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
			this.ProductsPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.IngredientsPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.InfoPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.StepActionsPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.MachineCountLabel = new System.Windows.Forms.Label();
			this.DeleteStepButton = new System.Windows.Forms.Button();
			this.RecipeLabel = new System.Windows.Forms.Label();
			this.PowerConsumptionLabel = new System.Windows.Forms.Label();
			this.MainPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.MainLayout = new System.Windows.Forms.FlowLayoutPanel();
			this.MachinePanel = new System.Windows.Forms.FlowLayoutPanel();
			this.ClockSpeedPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.MachineCountPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.ClockSpeedLabel = new System.Windows.Forms.Label();
			this.ClockSpeedNumeric = new System.Windows.Forms.NumericUpDown();
			this.MachineCountNumeric = new System.Windows.Forms.NumericUpDown();
			this.InfoPanel.SuspendLayout();
			this.StepActionsPanel.SuspendLayout();
			this.MainPanel.SuspendLayout();
			this.MainLayout.SuspendLayout();
			this.MachinePanel.SuspendLayout();
			this.ClockSpeedPanel.SuspendLayout();
			this.MachineCountPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ClockSpeedNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.MachineCountNumeric)).BeginInit();
			this.SuspendLayout();
			// 
			// ProductsPanel
			// 
			this.ProductsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ProductsPanel.AutoSize = true;
			this.ProductsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ProductsPanel.Location = new System.Drawing.Point(4, 4);
			this.ProductsPanel.Margin = new System.Windows.Forms.Padding(4);
			this.ProductsPanel.Name = "ProductsPanel";
			this.ProductsPanel.Size = new System.Drawing.Size(0, 0);
			this.ProductsPanel.TabIndex = 0;
			// 
			// IngredientsPanel
			// 
			this.IngredientsPanel.AutoSize = true;
			this.IngredientsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.IngredientsPanel.Location = new System.Drawing.Point(4, 142);
			this.IngredientsPanel.Margin = new System.Windows.Forms.Padding(4);
			this.IngredientsPanel.Name = "IngredientsPanel";
			this.IngredientsPanel.Size = new System.Drawing.Size(0, 0);
			this.IngredientsPanel.TabIndex = 0;
			// 
			// InfoPanel
			// 
			this.InfoPanel.AutoSize = true;
			this.InfoPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.InfoPanel.Controls.Add(this.StepActionsPanel);
			this.InfoPanel.Controls.Add(this.RecipeLabel);
			this.InfoPanel.Controls.Add(this.PowerConsumptionLabel);
			this.InfoPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.InfoPanel.Location = new System.Drawing.Point(4, 12);
			this.InfoPanel.Margin = new System.Windows.Forms.Padding(4);
			this.InfoPanel.Name = "InfoPanel";
			this.InfoPanel.Size = new System.Drawing.Size(544, 122);
			this.InfoPanel.TabIndex = 1;
			// 
			// StepActionsPanel
			// 
			this.StepActionsPanel.AutoSize = true;
			this.StepActionsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.StepActionsPanel.Controls.Add(this.MachinePanel);
			this.StepActionsPanel.Controls.Add(this.DeleteStepButton);
			this.StepActionsPanel.Location = new System.Drawing.Point(0, 0);
			this.StepActionsPanel.Margin = new System.Windows.Forms.Padding(0);
			this.StepActionsPanel.Name = "StepActionsPanel";
			this.StepActionsPanel.Size = new System.Drawing.Size(387, 82);
			this.StepActionsPanel.TabIndex = 3;
			// 
			// MachineCountLabel
			// 
			this.MachineCountLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.MachineCountLabel.AutoSize = true;
			this.MachineCountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MachineCountLabel.Location = new System.Drawing.Point(4, 6);
			this.MachineCountLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.MachineCountLabel.Name = "MachineCountLabel";
			this.MachineCountLabel.Size = new System.Drawing.Size(126, 20);
			this.MachineCountLabel.TabIndex = 0;
			this.MachineCountLabel.Text = "Machine Count:";
			// 
			// DeleteStepButton
			// 
			this.DeleteStepButton.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.DeleteStepButton.AutoSize = true;
			this.DeleteStepButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.DeleteStepButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.DeleteStepButton.Location = new System.Drawing.Point(276, 26);
			this.DeleteStepButton.Margin = new System.Windows.Forms.Padding(4);
			this.DeleteStepButton.Name = "DeleteStepButton";
			this.DeleteStepButton.Size = new System.Drawing.Size(107, 30);
			this.DeleteStepButton.TabIndex = 2;
			this.DeleteStepButton.Text = "Delete Step";
			this.DeleteStepButton.UseVisualStyleBackColor = true;
			this.DeleteStepButton.Click += new System.EventHandler(this.DeleteStepButton_Click);
			// 
			// RecipeLabel
			// 
			this.RecipeLabel.AutoSize = true;
			this.RecipeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RecipeLabel.Location = new System.Drawing.Point(4, 82);
			this.RecipeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.RecipeLabel.Name = "RecipeLabel";
			this.RecipeLabel.Size = new System.Drawing.Size(536, 20);
			this.RecipeLabel.TabIndex = 1;
			this.RecipeLabel.Text = "<itemcounts> -> <itemcounts in <seconds> seconds using <machine>";
			// 
			// PowerConsumptionLabel
			// 
			this.PowerConsumptionLabel.AutoSize = true;
			this.PowerConsumptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PowerConsumptionLabel.Location = new System.Drawing.Point(4, 102);
			this.PowerConsumptionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.PowerConsumptionLabel.Name = "PowerConsumptionLabel";
			this.PowerConsumptionLabel.Size = new System.Drawing.Size(189, 20);
			this.PowerConsumptionLabel.TabIndex = 3;
			this.PowerConsumptionLabel.Text = "Power Consumption: <>";
			// 
			// MainPanel
			// 
			this.MainPanel.AutoSize = true;
			this.MainPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.MainPanel.Controls.Add(this.ProductsPanel);
			this.MainPanel.Controls.Add(this.InfoPanel);
			this.MainPanel.Controls.Add(this.IngredientsPanel);
			this.MainPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.MainPanel.Location = new System.Drawing.Point(4, 4);
			this.MainPanel.Margin = new System.Windows.Forms.Padding(4);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(554, 148);
			this.MainPanel.TabIndex = 2;
			// 
			// MainLayout
			// 
			this.MainLayout.AutoSize = true;
			this.MainLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MainLayout.Controls.Add(this.MainPanel);
			this.MainLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.MainLayout.Location = new System.Drawing.Point(0, 0);
			this.MainLayout.Margin = new System.Windows.Forms.Padding(0);
			this.MainLayout.Name = "MainLayout";
			this.MainLayout.Size = new System.Drawing.Size(562, 156);
			this.MainLayout.TabIndex = 4;
			// 
			// MachinePanel
			// 
			this.MachinePanel.AutoSize = true;
			this.MachinePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MachinePanel.Controls.Add(this.MachineCountPanel);
			this.MachinePanel.Controls.Add(this.ClockSpeedPanel);
			this.MachinePanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.MachinePanel.Location = new System.Drawing.Point(3, 3);
			this.MachinePanel.Name = "MachinePanel";
			this.MachinePanel.Size = new System.Drawing.Size(266, 76);
			this.MachinePanel.TabIndex = 4;
			// 
			// ClockSpeedPanel
			// 
			this.ClockSpeedPanel.AutoSize = true;
			this.ClockSpeedPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClockSpeedPanel.Controls.Add(this.ClockSpeedLabel);
			this.ClockSpeedPanel.Controls.Add(this.ClockSpeedNumeric);
			this.ClockSpeedPanel.Location = new System.Drawing.Point(3, 41);
			this.ClockSpeedPanel.Name = "ClockSpeedPanel";
			this.ClockSpeedPanel.Size = new System.Drawing.Size(240, 32);
			this.ClockSpeedPanel.TabIndex = 2;
			// 
			// MachineCountPanel
			// 
			this.MachineCountPanel.AutoSize = true;
			this.MachineCountPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MachineCountPanel.Controls.Add(this.MachineCountLabel);
			this.MachineCountPanel.Controls.Add(this.MachineCountNumeric);
			this.MachineCountPanel.Location = new System.Drawing.Point(3, 3);
			this.MachineCountPanel.Name = "MachineCountPanel";
			this.MachineCountPanel.Size = new System.Drawing.Size(260, 32);
			this.MachineCountPanel.TabIndex = 3;
			// 
			// ClockSpeedLabel
			// 
			this.ClockSpeedLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ClockSpeedLabel.AutoSize = true;
			this.ClockSpeedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ClockSpeedLabel.Location = new System.Drawing.Point(3, 6);
			this.ClockSpeedLabel.Name = "ClockSpeedLabel";
			this.ClockSpeedLabel.Size = new System.Drawing.Size(108, 20);
			this.ClockSpeedLabel.TabIndex = 0;
			this.ClockSpeedLabel.Text = "Clock Speed:";
			// 
			// ClockSpeedNumeric
			// 
			this.ClockSpeedNumeric.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.ClockSpeedNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
			this.ClockSpeedNumeric.Location = new System.Drawing.Point(117, 3);
			this.ClockSpeedNumeric.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
			this.ClockSpeedNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
			this.ClockSpeedNumeric.Name = "ClockSpeedNumeric";
			this.ClockSpeedNumeric.Size = new System.Drawing.Size(120, 26);
			this.ClockSpeedNumeric.TabIndex = 1;
			this.ClockSpeedNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
			// 
			// MachineCountNumeric
			// 
			this.MachineCountNumeric.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.MachineCountNumeric.Location = new System.Drawing.Point(137, 3);
			this.MachineCountNumeric.Maximum = new decimal(new int[] {
            276447231,
            23283,
            0,
            0});
			this.MachineCountNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.MachineCountNumeric.Name = "MachineCountNumeric";
			this.MachineCountNumeric.Size = new System.Drawing.Size(120, 26);
			this.MachineCountNumeric.TabIndex = 1;
			this.MachineCountNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// StepControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.MainLayout);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "StepControl";
			this.Size = new System.Drawing.Size(562, 156);
			this.InfoPanel.ResumeLayout(false);
			this.InfoPanel.PerformLayout();
			this.StepActionsPanel.ResumeLayout(false);
			this.StepActionsPanel.PerformLayout();
			this.MainPanel.ResumeLayout(false);
			this.MainPanel.PerformLayout();
			this.MainLayout.ResumeLayout(false);
			this.MainLayout.PerformLayout();
			this.MachinePanel.ResumeLayout(false);
			this.MachinePanel.PerformLayout();
			this.ClockSpeedPanel.ResumeLayout(false);
			this.ClockSpeedPanel.PerformLayout();
			this.MachineCountPanel.ResumeLayout(false);
			this.MachineCountPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ClockSpeedNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.MachineCountNumeric)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel ProductsPanel;
		private System.Windows.Forms.FlowLayoutPanel IngredientsPanel;
		private System.Windows.Forms.FlowLayoutPanel InfoPanel;
		private System.Windows.Forms.Label RecipeLabel;
		private System.Windows.Forms.Label MachineCountLabel;
		public System.Windows.Forms.FlowLayoutPanel MainPanel;
		private System.Windows.Forms.FlowLayoutPanel StepActionsPanel;
		private System.Windows.Forms.Button DeleteStepButton;
		private System.Windows.Forms.FlowLayoutPanel MainLayout;
		private System.Windows.Forms.Label PowerConsumptionLabel;
		private System.Windows.Forms.FlowLayoutPanel MachinePanel;
		private System.Windows.Forms.FlowLayoutPanel MachineCountPanel;
		private System.Windows.Forms.FlowLayoutPanel ClockSpeedPanel;
		private System.Windows.Forms.Label ClockSpeedLabel;
		private System.Windows.Forms.NumericUpDown MachineCountNumeric;
		private System.Windows.Forms.NumericUpDown ClockSpeedNumeric;
	}
}
