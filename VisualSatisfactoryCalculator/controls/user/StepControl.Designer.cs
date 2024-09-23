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
			this.MachinePanel = new System.Windows.Forms.FlowLayoutPanel();
			this.MachineCountPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.MachineLabel = new System.Windows.Forms.Label();
			this.MachineCountNumeric = new System.Windows.Forms.NumericUpDown();
			this.ClockSpeedPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.ClockPercentLabelA = new System.Windows.Forms.Label();
			this.ClockSpeedNumeric = new System.Windows.Forms.NumericUpDown();
			this.DeleteStepButton = new System.Windows.Forms.Button();
			this.RecipeLabel = new System.Windows.Forms.Label();
			this.PowerConsumptionLabel = new System.Windows.Forms.Label();
			this.MainPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.MainLayout = new System.Windows.Forms.FlowLayoutPanel();
			this.ClockSpeedPercentLabelB = new System.Windows.Forms.Label();
			this.InfoPanel.SuspendLayout();
			this.StepActionsPanel.SuspendLayout();
			this.MachinePanel.SuspendLayout();
			this.MachineCountPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.MachineCountNumeric)).BeginInit();
			this.ClockSpeedPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ClockSpeedNumeric)).BeginInit();
			this.MainPanel.SuspendLayout();
			this.MainLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// ProductsPanel
			// 
			this.ProductsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ProductsPanel.AutoSize = true;
			this.ProductsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ProductsPanel.Location = new System.Drawing.Point(3, 3);
			this.ProductsPanel.Name = "ProductsPanel";
			this.ProductsPanel.Size = new System.Drawing.Size(0, 0);
			this.ProductsPanel.TabIndex = 0;
			// 
			// IngredientsPanel
			// 
			this.IngredientsPanel.AutoSize = true;
			this.IngredientsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.IngredientsPanel.Location = new System.Drawing.Point(3, 115);
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
			this.InfoPanel.Location = new System.Drawing.Point(3, 9);
			this.InfoPanel.Name = "InfoPanel";
			this.InfoPanel.Size = new System.Drawing.Size(451, 100);
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
			this.StepActionsPanel.Size = new System.Drawing.Size(293, 66);
			this.StepActionsPanel.TabIndex = 3;
			// 
			// MachinePanel
			// 
			this.MachinePanel.AutoSize = true;
			this.MachinePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MachinePanel.Controls.Add(this.MachineCountPanel);
			this.MachinePanel.Controls.Add(this.ClockSpeedPanel);
			this.MachinePanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.MachinePanel.Location = new System.Drawing.Point(2, 2);
			this.MachinePanel.Margin = new System.Windows.Forms.Padding(2);
			this.MachinePanel.Name = "MachinePanel";
			this.MachinePanel.Size = new System.Drawing.Size(191, 62);
			this.MachinePanel.TabIndex = 4;
			// 
			// MachineCountPanel
			// 
			this.MachineCountPanel.AutoSize = true;
			this.MachineCountPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MachineCountPanel.Controls.Add(this.MachineLabel);
			this.MachineCountPanel.Controls.Add(this.MachineCountNumeric);
			this.MachineCountPanel.Location = new System.Drawing.Point(2, 2);
			this.MachineCountPanel.Margin = new System.Windows.Forms.Padding(2);
			this.MachineCountPanel.Name = "MachineCountPanel";
			this.MachineCountPanel.Size = new System.Drawing.Size(187, 27);
			this.MachineCountPanel.TabIndex = 3;
			// 
			// MachineLabel
			// 
			this.MachineLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.MachineLabel.AutoSize = true;
			this.MachineLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MachineLabel.Location = new System.Drawing.Point(3, 5);
			this.MachineLabel.Name = "MachineLabel";
			this.MachineLabel.Size = new System.Drawing.Size(87, 17);
			this.MachineLabel.TabIndex = 0;
			this.MachineLabel.Text = "<machine> x";
			// 
			// MachineCountNumeric
			// 
			this.MachineCountNumeric.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.MachineCountNumeric.Location = new System.Drawing.Point(95, 2);
			this.MachineCountNumeric.Margin = new System.Windows.Forms.Padding(2);
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
			this.MachineCountNumeric.Size = new System.Drawing.Size(90, 23);
			this.MachineCountNumeric.TabIndex = 1;
			this.MachineCountNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// ClockSpeedPanel
			// 
			this.ClockSpeedPanel.AutoSize = true;
			this.ClockSpeedPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClockSpeedPanel.Controls.Add(this.ClockPercentLabelA);
			this.ClockSpeedPanel.Controls.Add(this.ClockSpeedNumeric);
			this.ClockSpeedPanel.Controls.Add(this.ClockSpeedPercentLabelB);
			this.ClockSpeedPanel.Location = new System.Drawing.Point(2, 33);
			this.ClockSpeedPanel.Margin = new System.Windows.Forms.Padding(2);
			this.ClockSpeedPanel.Name = "ClockSpeedPanel";
			this.ClockSpeedPanel.Size = new System.Drawing.Size(178, 27);
			this.ClockSpeedPanel.TabIndex = 2;
			// 
			// ClockPercentLabelA
			// 
			this.ClockPercentLabelA.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ClockPercentLabelA.AutoSize = true;
			this.ClockPercentLabelA.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ClockPercentLabelA.Location = new System.Drawing.Point(2, 5);
			this.ClockPercentLabelA.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.ClockPercentLabelA.Name = "ClockPercentLabelA";
			this.ClockPercentLabelA.Size = new System.Drawing.Size(56, 17);
			this.ClockPercentLabelA.TabIndex = 0;
			this.ClockPercentLabelA.Text = "Each at";
			// 
			// ClockSpeedNumeric
			// 
			this.ClockSpeedNumeric.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.ClockSpeedNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
			this.ClockSpeedNumeric.Location = new System.Drawing.Point(62, 2);
			this.ClockSpeedNumeric.Margin = new System.Windows.Forms.Padding(2);
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
			this.ClockSpeedNumeric.Size = new System.Drawing.Size(90, 23);
			this.ClockSpeedNumeric.TabIndex = 1;
			this.ClockSpeedNumeric.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// DeleteStepButton
			// 
			this.DeleteStepButton.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.DeleteStepButton.AutoSize = true;
			this.DeleteStepButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.DeleteStepButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.DeleteStepButton.Location = new System.Drawing.Point(198, 19);
			this.DeleteStepButton.Name = "DeleteStepButton";
			this.DeleteStepButton.Size = new System.Drawing.Size(92, 27);
			this.DeleteStepButton.TabIndex = 2;
			this.DeleteStepButton.Text = "Delete Step";
			this.DeleteStepButton.UseVisualStyleBackColor = true;
			this.DeleteStepButton.Click += new System.EventHandler(this.DeleteStepButton_Click);
			// 
			// RecipeLabel
			// 
			this.RecipeLabel.AutoSize = true;
			this.RecipeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RecipeLabel.Location = new System.Drawing.Point(3, 66);
			this.RecipeLabel.Name = "RecipeLabel";
			this.RecipeLabel.Size = new System.Drawing.Size(445, 17);
			this.RecipeLabel.TabIndex = 1;
			this.RecipeLabel.Text = "<itemcounts> -> <itemcounts in <seconds> seconds using <machine>";
			// 
			// PowerConsumptionLabel
			// 
			this.PowerConsumptionLabel.AutoSize = true;
			this.PowerConsumptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PowerConsumptionLabel.Location = new System.Drawing.Point(3, 83);
			this.PowerConsumptionLabel.Name = "PowerConsumptionLabel";
			this.PowerConsumptionLabel.Size = new System.Drawing.Size(157, 17);
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
			this.MainPanel.Location = new System.Drawing.Point(3, 3);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(459, 120);
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
			this.MainLayout.Size = new System.Drawing.Size(465, 126);
			this.MainLayout.TabIndex = 4;
			// 
			// ClockSpeedPercentLabelB
			// 
			this.ClockSpeedPercentLabelB.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ClockSpeedPercentLabelB.AutoSize = true;
			this.ClockSpeedPercentLabelB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.ClockSpeedPercentLabelB.Location = new System.Drawing.Point(156, 5);
			this.ClockSpeedPercentLabelB.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.ClockSpeedPercentLabelB.Name = "ClockSpeedPercentLabelB";
			this.ClockSpeedPercentLabelB.Size = new System.Drawing.Size(20, 17);
			this.ClockSpeedPercentLabelB.TabIndex = 2;
			this.ClockSpeedPercentLabelB.Text = "%";
			// 
			// StepControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.MainLayout);
			this.Name = "StepControl";
			this.Size = new System.Drawing.Size(465, 126);
			this.InfoPanel.ResumeLayout(false);
			this.InfoPanel.PerformLayout();
			this.StepActionsPanel.ResumeLayout(false);
			this.StepActionsPanel.PerformLayout();
			this.MachinePanel.ResumeLayout(false);
			this.MachinePanel.PerformLayout();
			this.MachineCountPanel.ResumeLayout(false);
			this.MachineCountPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.MachineCountNumeric)).EndInit();
			this.ClockSpeedPanel.ResumeLayout(false);
			this.ClockSpeedPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ClockSpeedNumeric)).EndInit();
			this.MainPanel.ResumeLayout(false);
			this.MainPanel.PerformLayout();
			this.MainLayout.ResumeLayout(false);
			this.MainLayout.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel ProductsPanel;
		private System.Windows.Forms.FlowLayoutPanel IngredientsPanel;
		private System.Windows.Forms.FlowLayoutPanel InfoPanel;
		private System.Windows.Forms.Label RecipeLabel;
		private System.Windows.Forms.Label MachineLabel;
		public System.Windows.Forms.FlowLayoutPanel MainPanel;
		private System.Windows.Forms.FlowLayoutPanel StepActionsPanel;
		private System.Windows.Forms.Button DeleteStepButton;
		private System.Windows.Forms.FlowLayoutPanel MainLayout;
		private System.Windows.Forms.Label PowerConsumptionLabel;
		private System.Windows.Forms.FlowLayoutPanel MachinePanel;
		private System.Windows.Forms.FlowLayoutPanel MachineCountPanel;
		private System.Windows.Forms.FlowLayoutPanel ClockSpeedPanel;
		private System.Windows.Forms.Label ClockPercentLabelA;
		private System.Windows.Forms.NumericUpDown MachineCountNumeric;
		private System.Windows.Forms.NumericUpDown ClockSpeedNumeric;
		private System.Windows.Forms.Label ClockSpeedPercentLabelB;
	}
}
