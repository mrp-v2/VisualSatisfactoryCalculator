using VisualSatisfactoryCalculator.code.Utility;

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
			this.DeleteStepButton = new System.Windows.Forms.Button();
			this.PowerConsumptionLabel = new System.Windows.Forms.Label();
			this.RecipeLabel = new System.Windows.Forms.Label();
			this.MachineCountLabel = new System.Windows.Forms.Label();
			this.MainPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.MainLayout = new System.Windows.Forms.FlowLayoutPanel();
			this.InfoPanel.SuspendLayout();
			this.MultiplierPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.MultiplierNumeric)).BeginInit();
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
			this.ProductsPanel.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.ProductsPanel_ControlAdded);
			// 
			// IngredientsPanel
			// 
			this.IngredientsPanel.AutoSize = true;
			this.IngredientsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.IngredientsPanel.Location = new System.Drawing.Point(3, 99);
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
			this.InfoPanel.Controls.Add(this.PowerConsumptionLabel);
			this.InfoPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.InfoPanel.Location = new System.Drawing.Point(3, 9);
			this.InfoPanel.Name = "InfoPanel";
			this.InfoPanel.Size = new System.Drawing.Size(451, 84);
			this.InfoPanel.TabIndex = 1;
			// 
			// MultiplierPanel
			// 
			this.MultiplierPanel.AutoSize = true;
			this.MultiplierPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MultiplierPanel.Controls.Add(this.MultiplierLable);
			this.MultiplierPanel.Controls.Add(this.MultiplierNumeric);
			this.MultiplierPanel.Controls.Add(this.DeleteStepButton);
			this.MultiplierPanel.Location = new System.Drawing.Point(0, 0);
			this.MultiplierPanel.Margin = new System.Windows.Forms.Padding(0);
			this.MultiplierPanel.Name = "MultiplierPanel";
			this.MultiplierPanel.Size = new System.Drawing.Size(314, 33);
			this.MultiplierPanel.TabIndex = 3;
			// 
			// MultiplierLable
			// 
			this.MultiplierLable.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.MultiplierLable.AutoSize = true;
			this.MultiplierLable.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MultiplierLable.Location = new System.Drawing.Point(3, 8);
			this.MultiplierLable.Name = "MultiplierLable";
			this.MultiplierLable.Size = new System.Drawing.Size(68, 17);
			this.MultiplierLable.TabIndex = 0;
			this.MultiplierLable.Text = "Multiplier:";
			// 
			// MultiplierNumeric
			// 
			this.MultiplierNumeric.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.MultiplierNumeric.AutoSize = true;
			this.MultiplierNumeric.DecimalPlaces = Constants.DECIMALS;
			this.MultiplierNumeric.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MultiplierNumeric.Location = new System.Drawing.Point(77, 5);
			this.MultiplierNumeric.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.MultiplierNumeric.Name = "MultiplierNumeric";
			this.MultiplierNumeric.Size = new System.Drawing.Size(136, 23);
			this.MultiplierNumeric.TabIndex = 1;
			this.MultiplierNumeric.ThousandsSeparator = true;
			this.MultiplierNumeric.ValueChanged += new System.EventHandler(this.MultiplierNumeric_ValueChanged);
			// 
			// DeleteStepButton
			// 
			this.DeleteStepButton.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.DeleteStepButton.AutoSize = true;
			this.DeleteStepButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.DeleteStepButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.DeleteStepButton.Location = new System.Drawing.Point(219, 3);
			this.DeleteStepButton.Name = "DeleteStepButton";
			this.DeleteStepButton.Size = new System.Drawing.Size(92, 27);
			this.DeleteStepButton.TabIndex = 2;
			this.DeleteStepButton.Text = "Delete Step";
			this.DeleteStepButton.UseVisualStyleBackColor = true;
			this.DeleteStepButton.Click += new System.EventHandler(this.DeleteStepButton_Click);
			// 
			// PowerConsumptionLabel
			// 
			this.PowerConsumptionLabel.AutoSize = true;
			this.PowerConsumptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PowerConsumptionLabel.Location = new System.Drawing.Point(3, 67);
			this.PowerConsumptionLabel.Name = "PowerConsumptionLabel";
			this.PowerConsumptionLabel.Size = new System.Drawing.Size(157, 17);
			this.PowerConsumptionLabel.TabIndex = 3;
			this.PowerConsumptionLabel.Text = "Power Consumption: <>";
			// 
			// RecipeLabel
			// 
			this.RecipeLabel.AutoSize = true;
			this.RecipeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RecipeLabel.Location = new System.Drawing.Point(3, 33);
			this.RecipeLabel.Name = "RecipeLabel";
			this.RecipeLabel.Size = new System.Drawing.Size(445, 17);
			this.RecipeLabel.TabIndex = 1;
			this.RecipeLabel.Text = "<itemcounts> -> <itemcounts in <seconds> seconds using <machine>";
			// 
			// MachineCountLabel
			// 
			this.MachineCountLabel.AutoSize = true;
			this.MachineCountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MachineCountLabel.Location = new System.Drawing.Point(3, 50);
			this.MachineCountLabel.Name = "MachineCountLabel";
			this.MachineCountLabel.Size = new System.Drawing.Size(241, 17);
			this.MachineCountLabel.TabIndex = 2;
			this.MachineCountLabel.Text = "<machine>s: <multiplier rounded up>";
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
			this.MainPanel.Location = new System.Drawing.Point(0, 0);
			this.MainPanel.Margin = new System.Windows.Forms.Padding(0);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(459, 104);
			this.MainPanel.TabIndex = 2;
			// 
			// MainLayout
			// 
			this.MainLayout.AutoSize = true;
			this.MainLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MainLayout.Controls.Add(this.MainPanel);
			this.MainLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.MainLayout.Location = new System.Drawing.Point(3, 3);
			this.MainLayout.Name = "MainLayout";
			this.MainLayout.Size = new System.Drawing.Size(459, 104);
			this.MainLayout.TabIndex = 4;
			// 
			// ProductionStepControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.MainLayout);
			this.Name = "ProductionStepControl";
			this.Size = new System.Drawing.Size(465, 110);
			this.InfoPanel.ResumeLayout(false);
			this.InfoPanel.PerformLayout();
			this.MultiplierPanel.ResumeLayout(false);
			this.MultiplierPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.MultiplierNumeric)).EndInit();
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
		private System.Windows.Forms.Label MultiplierLable;
		private System.Windows.Forms.Label RecipeLabel;
		private System.Windows.Forms.Label MachineCountLabel;
		public System.Windows.Forms.FlowLayoutPanel MainPanel;
		private System.Windows.Forms.FlowLayoutPanel MultiplierPanel;
		private System.Windows.Forms.NumericUpDown MultiplierNumeric;
		private System.Windows.Forms.Button DeleteStepButton;
		private System.Windows.Forms.FlowLayoutPanel MainLayout;
		private System.Windows.Forms.Label PowerConsumptionLabel;
	}
}
