namespace VisualSatisfactoryCalculator.forms
{
	partial class UseSaveFilePrompt
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
			this.MainLayout = new System.Windows.Forms.FlowLayoutPanel();
			this.Label = new System.Windows.Forms.Label();
			this.ButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.UseFileButton = new System.Windows.Forms.Button();
			this.DontUseFileButton = new System.Windows.Forms.Button();
			this.MainLayout.SuspendLayout();
			this.ButtonPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainLayout
			// 
			this.MainLayout.AutoSize = true;
			this.MainLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MainLayout.Controls.Add(this.Label);
			this.MainLayout.Controls.Add(this.ButtonPanel);
			this.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.MainLayout.Location = new System.Drawing.Point(0, 0);
			this.MainLayout.Name = "MainLayout";
			this.MainLayout.Size = new System.Drawing.Size(411, 101);
			this.MainLayout.TabIndex = 0;
			// 
			// Label
			// 
			this.Label.AutoSize = true;
			this.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label.Location = new System.Drawing.Point(3, 0);
			this.Label.Name = "Label";
			this.Label.Size = new System.Drawing.Size(312, 17);
			this.Label.TabIndex = 0;
			this.Label.Text = "Use a save file to decide which recipes to show?";
			// 
			// ButtonPanel
			// 
			this.ButtonPanel.AutoSize = true;
			this.ButtonPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ButtonPanel.Controls.Add(this.UseFileButton);
			this.ButtonPanel.Controls.Add(this.DontUseFileButton);
			this.ButtonPanel.Location = new System.Drawing.Point(3, 20);
			this.ButtonPanel.Name = "ButtonPanel";
			this.ButtonPanel.Size = new System.Drawing.Size(162, 29);
			this.ButtonPanel.TabIndex = 1;
			// 
			// UseFileButton
			// 
			this.UseFileButton.Location = new System.Drawing.Point(3, 3);
			this.UseFileButton.Name = "UseFileButton";
			this.UseFileButton.Size = new System.Drawing.Size(75, 23);
			this.UseFileButton.TabIndex = 0;
			this.UseFileButton.Text = "Yes";
			this.UseFileButton.UseVisualStyleBackColor = true;
			this.UseFileButton.Click += new System.EventHandler(this.UseFileButton_Click);
			// 
			// DontUseFileButton
			// 
			this.DontUseFileButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.DontUseFileButton.Location = new System.Drawing.Point(84, 3);
			this.DontUseFileButton.Name = "DontUseFileButton";
			this.DontUseFileButton.Size = new System.Drawing.Size(75, 23);
			this.DontUseFileButton.TabIndex = 1;
			this.DontUseFileButton.Text = "No";
			this.DontUseFileButton.UseVisualStyleBackColor = true;
			this.DontUseFileButton.Click += new System.EventHandler(this.DontUseFileButton_Click);
			// 
			// UseSaveFilePrompt
			// 
			this.AcceptButton = this.UseFileButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.DontUseFileButton;
			this.ClientSize = new System.Drawing.Size(411, 101);
			this.Controls.Add(this.MainLayout);
			this.Name = "UseSaveFilePrompt";
			this.Text = "UseSaveFilePrompt";
			this.MainLayout.ResumeLayout(false);
			this.MainLayout.PerformLayout();
			this.ButtonPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel MainLayout;
		private System.Windows.Forms.Label Label;
		private System.Windows.Forms.FlowLayoutPanel ButtonPanel;
		private System.Windows.Forms.Button UseFileButton;
		private System.Windows.Forms.Button DontUseFileButton;
	}
}