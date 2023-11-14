namespace VisualSatisfactoryCalculator.controls.user
{
	partial class RationalNumberControl
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
			this.NumberTextBox = new System.Windows.Forms.TextBox();
			this.AlternateNumberLabel = new System.Windows.Forms.Label();
			this.MainPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainPanel
			// 
			this.MainPanel.AutoSize = true;
			this.MainPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MainPanel.Controls.Add(this.NumberTextBox);
			this.MainPanel.Controls.Add(this.AlternateNumberLabel);
			this.MainPanel.Location = new System.Drawing.Point(0, 0);
			this.MainPanel.Margin = new System.Windows.Forms.Padding(0);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(288, 29);
			this.MainPanel.TabIndex = 1;
			// 
			// NumberTextBox
			// 
			this.NumberTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.NumberTextBox.Location = new System.Drawing.Point(3, 3);
			this.NumberTextBox.Name = "NumberTextBox";
			this.NumberTextBox.Size = new System.Drawing.Size(100, 23);
			this.NumberTextBox.TabIndex = 7;
			this.NumberTextBox.WordWrap = false;
			this.NumberTextBox.TextChanged += new System.EventHandler(this.NumberTextBox_TextChanged);
			this.NumberTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NumberTextBox_KeyDown);
			this.NumberTextBox.LostFocus += new System.EventHandler(this.NumberTextBox_LostFocus);
			// 
			// AlternateNumberLabel
			// 
			this.AlternateNumberLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.AlternateNumberLabel.AutoSize = true;
			this.AlternateNumberLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.AlternateNumberLabel.Location = new System.Drawing.Point(109, 6);
			this.AlternateNumberLabel.Name = "AlternateNumberLabel";
			this.AlternateNumberLabel.Size = new System.Drawing.Size(176, 17);
			this.AlternateNumberLabel.TabIndex = 6;
			this.AlternateNumberLabel.Text = "<alternate number format>";
			// 
			// RationalNumberControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.MainPanel);
			this.Name = "RationalNumberControl";
			this.Size = new System.Drawing.Size(288, 29);
			this.MainPanel.ResumeLayout(false);
			this.MainPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel MainPanel;
		private System.Windows.Forms.Label AlternateNumberLabel;
		private System.Windows.Forms.TextBox NumberTextBox;
	}
}
