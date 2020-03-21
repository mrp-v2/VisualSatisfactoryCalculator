namespace VisualSatisfactoryCalculator.controls.user
{
	partial class ProductionPlanTotalViewControl
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
			this.NetProductsLabel = new System.Windows.Forms.Label();
			this.MachinesLabel = new System.Windows.Forms.Label();
			this.NetIngredientsLabel = new System.Windows.Forms.Label();
			this.MainPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainPanel
			// 
			this.MainPanel.AutoSize = true;
			this.MainPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MainPanel.Controls.Add(this.NetProductsLabel);
			this.MainPanel.Controls.Add(this.MachinesLabel);
			this.MainPanel.Controls.Add(this.NetIngredientsLabel);
			this.MainPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.MainPanel.Location = new System.Drawing.Point(0, 0);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(64, 39);
			this.MainPanel.TabIndex = 0;
			// 
			// NetProductsLabel
			// 
			this.NetProductsLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.NetProductsLabel.AutoSize = true;
			this.NetProductsLabel.Location = new System.Drawing.Point(3, 0);
			this.NetProductsLabel.Name = "NetProductsLabel";
			this.NetProductsLabel.Size = new System.Drawing.Size(48, 13);
			this.NetProductsLabel.TabIndex = 0;
			this.NetProductsLabel.Text = "products";
			// 
			// MachinesLabel
			// 
			this.MachinesLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.MachinesLabel.AutoSize = true;
			this.MachinesLabel.Location = new System.Drawing.Point(3, 13);
			this.MachinesLabel.Name = "MachinesLabel";
			this.MachinesLabel.Size = new System.Drawing.Size(52, 13);
			this.MachinesLabel.TabIndex = 1;
			this.MachinesLabel.Text = "machines";
			// 
			// NetIngredientsLabel
			// 
			this.NetIngredientsLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.NetIngredientsLabel.AutoSize = true;
			this.NetIngredientsLabel.Location = new System.Drawing.Point(3, 26);
			this.NetIngredientsLabel.Name = "NetIngredientsLabel";
			this.NetIngredientsLabel.Size = new System.Drawing.Size(58, 13);
			this.NetIngredientsLabel.TabIndex = 2;
			this.NetIngredientsLabel.Text = "ingredients";
			// 
			// ProductionPlanTotalViewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.MainPanel);
			this.Name = "ProductionPlanTotalViewControl";
			this.Size = new System.Drawing.Size(67, 42);
			this.MainPanel.ResumeLayout(false);
			this.MainPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel MainPanel;
		public System.Windows.Forms.Label NetProductsLabel;
		public System.Windows.Forms.Label MachinesLabel;
		public System.Windows.Forms.Label NetIngredientsLabel;
	}
}
