namespace VisualSatisfactoryCalculator.controls.user
{
	partial class ProductionStepControl
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
			this.MultiplierPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.MultiplierLable = new System.Windows.Forms.Label();
			this.MultiplierNumeric = new System.Windows.Forms.NumericUpDown();
			this.RecipeLabel = new System.Windows.Forms.Label();
			this.MachineCountLabel = new System.Windows.Forms.Label();
			this.MainPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.InfoPanel.SuspendLayout();
			this.MultiplierPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.MultiplierNumeric)).BeginInit();
			this.MainPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// ProductsPanel
			// 
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
			this.IngredientsPanel.Location = new System.Drawing.Point(3, 67);
			this.IngredientsPanel.Name = "IngredientsPanel";
			this.IngredientsPanel.Size = new System.Drawing.Size(0, 0);
			this.IngredientsPanel.TabIndex = 0;
			// 
			// InfoPanel
			// 
			this.InfoPanel.AutoSize = true;
			this.InfoPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.InfoPanel.Controls.Add(this.MultiplierPanel);
			this.InfoPanel.Controls.Add(this.RecipeLabel);
			this.InfoPanel.Controls.Add(this.MachineCountLabel);
			this.InfoPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.InfoPanel.Location = new System.Drawing.Point(3, 9);
			this.InfoPanel.Name = "InfoPanel";
			this.InfoPanel.Size = new System.Drawing.Size(340, 52);
			this.InfoPanel.TabIndex = 1;
			// 
			// MultiplierPanel
			// 
			this.MultiplierPanel.AutoSize = true;
			this.MultiplierPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MultiplierPanel.Controls.Add(this.MultiplierLable);
			this.MultiplierPanel.Controls.Add(this.MultiplierNumeric);
			this.MultiplierPanel.Location = new System.Drawing.Point(0, 0);
			this.MultiplierPanel.Margin = new System.Windows.Forms.Padding(0);
			this.MultiplierPanel.Name = "MultiplierPanel";
			this.MultiplierPanel.Size = new System.Drawing.Size(158, 26);
			this.MultiplierPanel.TabIndex = 3;
			// 
			// MultiplierLable
			// 
			this.MultiplierLable.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.MultiplierLable.AutoSize = true;
			this.MultiplierLable.Location = new System.Drawing.Point(3, 6);
			this.MultiplierLable.Name = "MultiplierLable";
			this.MultiplierLable.Size = new System.Drawing.Size(51, 13);
			this.MultiplierLable.TabIndex = 0;
			this.MultiplierLable.Text = "Multiplier:";
			// 
			// MultiplierNumeric
			// 
			this.MultiplierNumeric.AutoSize = true;
			this.MultiplierNumeric.DecimalPlaces = 5;
			this.MultiplierNumeric.Location = new System.Drawing.Point(60, 3);
			this.MultiplierNumeric.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.MultiplierNumeric.Name = "MultiplierNumeric";
			this.MultiplierNumeric.Size = new System.Drawing.Size(95, 20);
			this.MultiplierNumeric.TabIndex = 1;
			this.MultiplierNumeric.ThousandsSeparator = true;
			this.MultiplierNumeric.ValueChanged += new System.EventHandler(this.MultiplierNumeric_ValueChanged);
			// 
			// RecipeLabel
			// 
			this.RecipeLabel.AutoSize = true;
			this.RecipeLabel.Location = new System.Drawing.Point(3, 26);
			this.RecipeLabel.Name = "RecipeLabel";
			this.RecipeLabel.Size = new System.Drawing.Size(334, 13);
			this.RecipeLabel.TabIndex = 1;
			this.RecipeLabel.Text = "<itemcounts> -> <itemcounts in <seconds> seconds using <machine>";
			// 
			// MachineCountLabel
			// 
			this.MachineCountLabel.AutoSize = true;
			this.MachineCountLabel.Location = new System.Drawing.Point(3, 39);
			this.MachineCountLabel.Name = "MachineCountLabel";
			this.MachineCountLabel.Size = new System.Drawing.Size(179, 13);
			this.MachineCountLabel.TabIndex = 2;
			this.MachineCountLabel.Text = "<machine>s: <multiplier rounded up>";
			// 
			// MainPanel
			// 
			this.MainPanel.AutoSize = true;
			this.MainPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MainPanel.Controls.Add(this.ProductsPanel);
			this.MainPanel.Controls.Add(this.InfoPanel);
			this.MainPanel.Controls.Add(this.IngredientsPanel);
			this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.MainPanel.Location = new System.Drawing.Point(0, 0);
			this.MainPanel.Margin = new System.Windows.Forms.Padding(0);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(346, 70);
			this.MainPanel.TabIndex = 2;
			// 
			// ProductionStepControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.MainPanel);
			this.Name = "ProductionStepControl";
			this.Size = new System.Drawing.Size(346, 70);
			this.InfoPanel.ResumeLayout(false);
			this.InfoPanel.PerformLayout();
			this.MultiplierPanel.ResumeLayout(false);
			this.MultiplierPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.MultiplierNumeric)).EndInit();
			this.MainPanel.ResumeLayout(false);
			this.MainPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel ProductsPanel;
		private System.Windows.Forms.FlowLayoutPanel IngredientsPanel;
		private System.Windows.Forms.FlowLayoutPanel InfoPanel;
		private System.Windows.Forms.Label MultiplierLable;
		private System.Windows.Forms.Label RecipeLabel;
		private System.Windows.Forms.Label MachineCountLabel;
		public System.Windows.Forms.FlowLayoutPanel MainPanel;
		private System.Windows.Forms.FlowLayoutPanel MultiplierPanel;
		private System.Windows.Forms.NumericUpDown MultiplierNumeric;
	}
}
