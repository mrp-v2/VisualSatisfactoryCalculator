namespace VisualSatisfactoryCalculator.controls.user
{
	partial class PlanTotalViewControl
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
			this.MainPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.ProductsLabel = new System.Windows.Forms.Label();
			this.MachinesLabel = new System.Windows.Forms.Label();
			this.IngredientsLabel = new System.Windows.Forms.Label();
			this.PowerDrawLabel = new System.Windows.Forms.Label();
			this.MainPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainPanel
			// 
			this.MainPanel.AutoSize = true;
			this.MainPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MainPanel.Controls.Add(this.ProductsLabel);
			this.MainPanel.Controls.Add(this.PowerDrawLabel);
			this.MainPanel.Controls.Add(this.MachinesLabel);
			this.MainPanel.Controls.Add(this.IngredientsLabel);
			this.MainPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.MainPanel.Location = new System.Drawing.Point(0, 0);
			this.MainPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(101, 80);
			this.MainPanel.TabIndex = 0;
			// 
			// ProductsLabel
			// 
			this.ProductsLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.ProductsLabel.AutoSize = true;
			this.ProductsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ProductsLabel.Location = new System.Drawing.Point(4, 0);
			this.ProductsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.ProductsLabel.Name = "ProductsLabel";
			this.ProductsLabel.Size = new System.Drawing.Size(74, 20);
			this.ProductsLabel.TabIndex = 0;
			this.ProductsLabel.Text = "products";
			// 
			// MachinesLabel
			// 
			this.MachinesLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.MachinesLabel.AutoSize = true;
			this.MachinesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MachinesLabel.Location = new System.Drawing.Point(4, 40);
			this.MachinesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.MachinesLabel.Name = "MachinesLabel";
			this.MachinesLabel.Size = new System.Drawing.Size(81, 20);
			this.MachinesLabel.TabIndex = 1;
			this.MachinesLabel.Text = "machines";
			// 
			// IngredientsLabel
			// 
			this.IngredientsLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IngredientsLabel.AutoSize = true;
			this.IngredientsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.IngredientsLabel.Location = new System.Drawing.Point(4, 60);
			this.IngredientsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.IngredientsLabel.Name = "IngredientsLabel";
			this.IngredientsLabel.Size = new System.Drawing.Size(91, 20);
			this.IngredientsLabel.TabIndex = 2;
			this.IngredientsLabel.Text = "ingredients";
			// 
			// PowerDrawLabel
			// 
			this.PowerDrawLabel.AutoSize = true;
			this.PowerDrawLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.PowerDrawLabel.Location = new System.Drawing.Point(3, 20);
			this.PowerDrawLabel.Name = "PowerDrawLabel";
			this.PowerDrawLabel.Size = new System.Drawing.Size(95, 20);
			this.PowerDrawLabel.TabIndex = 3;
			this.PowerDrawLabel.Text = "power draw";
			// 
			// PlanTotalViewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.MainPanel);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "PlanTotalViewControl";
			this.Size = new System.Drawing.Size(105, 84);
			this.MainPanel.ResumeLayout(false);
			this.MainPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel MainPanel;
		public System.Windows.Forms.Label ProductsLabel;
		public System.Windows.Forms.Label MachinesLabel;
		public System.Windows.Forms.Label IngredientsLabel;
		public System.Windows.Forms.Label PowerDrawLabel;
	}
}
