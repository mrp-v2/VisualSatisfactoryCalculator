
using VisualSatisfactoryCalculator.code.Utility;

namespace VisualSatisfactoryCalculator.controls.user
{
	partial class BalancingControl
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
			this.NumberControl = new RationalNumberControl();
			this.LockBox = new System.Windows.Forms.CheckBox();
			this.MainLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.MainLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// NumberControl
			// 
			this.NumberControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.NumberControl.Location = new System.Drawing.Point(0, 23);
			this.NumberControl.Margin = new System.Windows.Forms.Padding(0);
			this.NumberControl.Name = "NumberControl";
			this.NumberControl.TabIndex = 0;
			// 
			// LockBox
			// 
			this.LockBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.LockBox.AutoSize = true;
			this.LockBox.Location = new System.Drawing.Point(11, 3);
			this.LockBox.Name = "LockBox";
			this.LockBox.Size = new System.Drawing.Size(62, 17);
			this.LockBox.TabIndex = 1;
			this.LockBox.Text = "Locked";
			this.LockBox.UseVisualStyleBackColor = true;
			this.LockBox.CheckedChanged += new System.EventHandler(this.LockBox_CheckedChanged);
			// 
			// MainLayoutPanel
			// 
			this.MainLayoutPanel.AutoSize = true;
			this.MainLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MainLayoutPanel.Controls.Add(this.LockBox);
			this.MainLayoutPanel.Controls.Add(this.NumberControl);
			this.MainLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.MainLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.MainLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.MainLayoutPanel.Name = "MainLayoutPanel";
			this.MainLayoutPanel.Size = new System.Drawing.Size(84, 46);
			this.MainLayoutPanel.TabIndex = 2;
			// 
			// BalancingControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.MainLayoutPanel);
			this.Name = "BalancingControl";
			this.Size = new System.Drawing.Size(84, 46);
			this.MainLayoutPanel.ResumeLayout(false);
			this.MainLayoutPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private RationalNumberControl NumberControl;
		private System.Windows.Forms.CheckBox LockBox;
		private System.Windows.Forms.FlowLayoutPanel MainLayoutPanel;
	}
}
