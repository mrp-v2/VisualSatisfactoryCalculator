
using System.Windows.Forms;

using VisualSatisfactoryCalculator.code.Utility;
using VisualSatisfactoryCalculator.model.production;

namespace VisualSatisfactoryCalculator.controls.user
{
	partial class BalancingControl<ItemType> : UserControl where ItemType : AbstractItem
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
			this.NumberControl = new VisualSatisfactoryCalculator.controls.user.RationalNumberControl();
			this.MainLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.MainLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// NumberControl
			// 
			this.NumberControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.NumberControl.AutoSize = true;
			this.NumberControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.NumberControl.Location = new System.Drawing.Point(0, 0);
			this.NumberControl.Margin = new System.Windows.Forms.Padding(0);
			this.NumberControl.Name = "NumberControl";
			this.NumberControl.Size = new System.Drawing.Size(356, 34);
			this.NumberControl.TabIndex = 0;
			// 
			// MainLayoutPanel
			// 
			this.MainLayoutPanel.AutoSize = true;
			this.MainLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MainLayoutPanel.Controls.Add(this.NumberControl);
			this.MainLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.MainLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.MainLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.MainLayoutPanel.Name = "MainLayoutPanel";
			this.MainLayoutPanel.Size = new System.Drawing.Size(356, 34);
			this.MainLayoutPanel.TabIndex = 2;
			// 
			// BalancingControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.MainLayoutPanel);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "BalancingControl";
			this.Size = new System.Drawing.Size(356, 34);
			this.MainLayoutPanel.ResumeLayout(false);
			this.MainLayoutPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private RationalNumberControl NumberControl;
		private System.Windows.Forms.FlowLayoutPanel MainLayoutPanel;
	}
}
