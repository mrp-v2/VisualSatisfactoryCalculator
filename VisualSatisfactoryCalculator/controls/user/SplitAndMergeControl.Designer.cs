
namespace VisualSatisfactoryCalculator.controls.user
{
	partial class SplitAndMergeControl
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
			this.OutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.SplitMergeLabel = new System.Windows.Forms.Label();
			this.InPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.PromptButton = new System.Windows.Forms.Button();
			this.MainPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainPanel
			// 
			this.MainPanel.AutoSize = true;
			this.MainPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.MainPanel.Controls.Add(this.OutPanel);
			this.MainPanel.Controls.Add(this.SplitMergeLabel);
			this.MainPanel.Controls.Add(this.PromptButton);
			this.MainPanel.Controls.Add(this.InPanel);
			this.MainPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.MainPanel.Location = new System.Drawing.Point(3, 3);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(100, 64);
			this.MainPanel.TabIndex = 0;
			// 
			// OutPanel
			// 
			this.OutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.OutPanel.AutoSize = true;
			this.OutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.OutPanel.Location = new System.Drawing.Point(3, 3);
			this.OutPanel.Name = "OutPanel";
			this.OutPanel.Size = new System.Drawing.Size(0, 0);
			this.OutPanel.TabIndex = 0;
			// 
			// SplitMergeLabel
			// 
			this.SplitMergeLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.SplitMergeLabel.AutoSize = true;
			this.SplitMergeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.SplitMergeLabel.Location = new System.Drawing.Point(3, 6);
			this.SplitMergeLabel.Name = "SplitMergeLabel";
			this.SplitMergeLabel.Size = new System.Drawing.Size(92, 17);
			this.SplitMergeLabel.TabIndex = 1;
			this.SplitMergeLabel.Text = "Split && Merge";
			// 
			// InPanel
			// 
			this.InPanel.AutoSize = true;
			this.InPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.InPanel.Location = new System.Drawing.Point(3, 59);
			this.InPanel.Name = "InPanel";
			this.InPanel.Size = new System.Drawing.Size(0, 0);
			this.InPanel.TabIndex = 2;
			// 
			// PromptButton
			// 
			this.PromptButton.AutoSize = true;
			this.PromptButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.PromptButton.Location = new System.Drawing.Point(3, 26);
			this.PromptButton.Name = "PromptButton";
			this.PromptButton.Size = new System.Drawing.Size(75, 27);
			this.PromptButton.TabIndex = 3;
			this.PromptButton.Text = "Balance";
			this.PromptButton.UseVisualStyleBackColor = true;
			this.PromptButton.Click += new System.EventHandler(this.PromptButton_Click);
			// 
			// SplitAndMergeControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.MainPanel);
			this.Name = "SplitAndMergeControl";
			this.Size = new System.Drawing.Size(106, 70);
			this.MainPanel.ResumeLayout(false);
			this.MainPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.FlowLayoutPanel MainPanel;
		private System.Windows.Forms.FlowLayoutPanel OutPanel;
		private System.Windows.Forms.Label SplitMergeLabel;
		private System.Windows.Forms.FlowLayoutPanel InPanel;
		private System.Windows.Forms.Button PromptButton;
	}
}
