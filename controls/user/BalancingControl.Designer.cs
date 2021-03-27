
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
			this.Numeric = new System.Windows.Forms.NumericUpDown();
			this.LockBox = new System.Windows.Forms.CheckBox();
			this.MainLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this.Numeric)).BeginInit();
			this.MainLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// Numeric
			// 
			this.Numeric.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.Numeric.AutoSize = true;
			this.Numeric.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.Numeric.Location = new System.Drawing.Point(10, 23);
			this.Numeric.Margin = new System.Windows.Forms.Padding(0);
			this.Numeric.Name = "Numeric";
			this.Numeric.Size = new System.Drawing.Size(48, 23);
			this.Numeric.TabIndex = 0;
			this.Numeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
			// 
			// LockBox
			// 
			this.LockBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.LockBox.AutoSize = true;
			this.LockBox.Location = new System.Drawing.Point(3, 3);
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
			this.MainLayoutPanel.Controls.Add(this.Numeric);
			this.MainLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.MainLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.MainLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.MainLayoutPanel.Name = "MainLayoutPanel";
			this.MainLayoutPanel.Size = new System.Drawing.Size(68, 46);
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
			this.Size = new System.Drawing.Size(68, 46);
			((System.ComponentModel.ISupportInitialize)(this.Numeric)).EndInit();
			this.MainLayoutPanel.ResumeLayout(false);
			this.MainLayoutPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.NumericUpDown Numeric;
		private System.Windows.Forms.CheckBox LockBox;
		private System.Windows.Forms.FlowLayoutPanel MainLayoutPanel;
	}
}
