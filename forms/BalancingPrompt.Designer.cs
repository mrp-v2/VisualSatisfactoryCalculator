
namespace VisualSatisfactoryCalculator.forms
{
	partial class BalancingPrompt
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.MainLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.InFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.OutFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.OutGroup = new System.Windows.Forms.GroupBox();
			this.InGroup = new System.Windows.Forms.GroupBox();
			this.ButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.DoneButton = new System.Windows.Forms.Button();
			this.CancelButton = new System.Windows.Forms.Button();
			this.MainLayoutPanel.SuspendLayout();
			this.OutGroup.SuspendLayout();
			this.InGroup.SuspendLayout();
			this.ButtonPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainLayoutPanel
			// 
			this.MainLayoutPanel.AutoSize = true;
			this.MainLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MainLayoutPanel.Controls.Add(this.OutGroup);
			this.MainLayoutPanel.Controls.Add(this.InGroup);
			this.MainLayoutPanel.Controls.Add(this.ButtonPanel);
			this.MainLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.MainLayoutPanel.Location = new System.Drawing.Point(12, 12);
			this.MainLayoutPanel.Name = "MainLayoutPanel";
			this.MainLayoutPanel.Size = new System.Drawing.Size(131, 127);
			this.MainLayoutPanel.TabIndex = 0;
			// 
			// InFlowPanel
			// 
			this.InFlowPanel.AutoSize = true;
			this.InFlowPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.InFlowPanel.Location = new System.Drawing.Point(6, 19);
			this.InFlowPanel.Name = "InFlowPanel";
			this.InFlowPanel.Size = new System.Drawing.Size(0, 0);
			this.InFlowPanel.TabIndex = 1;
			// 
			// OutFlowPanel
			// 
			this.OutFlowPanel.AutoSize = true;
			this.OutFlowPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.OutFlowPanel.Location = new System.Drawing.Point(6, 19);
			this.OutFlowPanel.Name = "OutFlowPanel";
			this.OutFlowPanel.Size = new System.Drawing.Size(0, 0);
			this.OutFlowPanel.TabIndex = 2;
			// 
			// OutGroup
			// 
			this.OutGroup.AutoSize = true;
			this.OutGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.OutGroup.Controls.Add(this.OutFlowPanel);
			this.OutGroup.Location = new System.Drawing.Point(3, 3);
			this.OutGroup.Name = "OutGroup";
			this.OutGroup.Size = new System.Drawing.Size(12, 38);
			this.OutGroup.TabIndex = 1;
			this.OutGroup.TabStop = false;
			this.OutGroup.Text = "Outputs";
			// 
			// InGroup
			// 
			this.InGroup.AutoSize = true;
			this.InGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.InGroup.Controls.Add(this.InFlowPanel);
			this.InGroup.Location = new System.Drawing.Point(3, 47);
			this.InGroup.Name = "InGroup";
			this.InGroup.Size = new System.Drawing.Size(12, 38);
			this.InGroup.TabIndex = 2;
			this.InGroup.TabStop = false;
			this.InGroup.Text = "Inputs";
			// 
			// ButtonPanel
			// 
			this.ButtonPanel.AutoSize = true;
			this.ButtonPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ButtonPanel.Controls.Add(this.DoneButton);
			this.ButtonPanel.Controls.Add(this.CancelButton);
			this.ButtonPanel.Location = new System.Drawing.Point(3, 91);
			this.ButtonPanel.Name = "ButtonPanel";
			this.ButtonPanel.Size = new System.Drawing.Size(125, 33);
			this.ButtonPanel.TabIndex = 3;
			// 
			// DoneButton
			// 
			this.DoneButton.AutoSize = true;
			this.DoneButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.DoneButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.DoneButton.Location = new System.Drawing.Point(3, 3);
			this.DoneButton.Name = "DoneButton";
			this.DoneButton.Size = new System.Drawing.Size(52, 27);
			this.DoneButton.TabIndex = 0;
			this.DoneButton.Text = "Done";
			this.DoneButton.UseVisualStyleBackColor = true;
			this.DoneButton.Click += new System.EventHandler(this.DoneButton_Click);
			// 
			// CancelButton
			// 
			this.CancelButton.AutoSize = true;
			this.CancelButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.CancelButton.Location = new System.Drawing.Point(61, 3);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(61, 27);
			this.CancelButton.TabIndex = 1;
			this.CancelButton.Text = "Cancel";
			this.CancelButton.UseVisualStyleBackColor = true;
			this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// BalancingPrompt
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(160, 152);
			this.Controls.Add(this.MainLayoutPanel);
			this.Name = "BalancingPrompt";
			this.ShowIcon = false;
			this.Text = "Balancing";
			this.MainLayoutPanel.ResumeLayout(false);
			this.MainLayoutPanel.PerformLayout();
			this.OutGroup.ResumeLayout(false);
			this.OutGroup.PerformLayout();
			this.InGroup.ResumeLayout(false);
			this.InGroup.PerformLayout();
			this.ButtonPanel.ResumeLayout(false);
			this.ButtonPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel MainLayoutPanel;
		private System.Windows.Forms.FlowLayoutPanel OutFlowPanel;
		private System.Windows.Forms.FlowLayoutPanel InFlowPanel;
		private System.Windows.Forms.GroupBox OutGroup;
		private System.Windows.Forms.GroupBox InGroup;
		private System.Windows.Forms.FlowLayoutPanel ButtonPanel;
		private System.Windows.Forms.Button DoneButton;
		private System.Windows.Forms.Button CancelButton;
	}
}