namespace VisualSatisfactoryCalculator.forms
{
	partial class SelectRecipePrompt
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
			this.RecipesList = new System.Windows.Forms.ListBox();
			this.YesButton = new System.Windows.Forms.Button();
			this.NoButton = new System.Windows.Forms.Button();
			this.ButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.MainPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.ButtonPanel.SuspendLayout();
			this.MainPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// RecipesList
			// 
			this.RecipesList.FormattingEnabled = true;
			this.RecipesList.Location = new System.Drawing.Point(3, 3);
			this.RecipesList.Name = "RecipesList";
			this.RecipesList.Size = new System.Drawing.Size(808, 277);
			this.RecipesList.TabIndex = 0;
			// 
			// YesButton
			// 
			this.YesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.YesButton.AutoSize = true;
			this.YesButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.YesButton.Location = new System.Drawing.Point(3, 3);
			this.YesButton.Name = "YesButton";
			this.YesButton.Size = new System.Drawing.Size(51, 23);
			this.YesButton.TabIndex = 1;
			this.YesButton.Text = "Accept";
			this.YesButton.UseVisualStyleBackColor = true;
			this.YesButton.Click += new System.EventHandler(this.YesButton_Click);
			// 
			// NoButton
			// 
			this.NoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.NoButton.AutoSize = true;
			this.NoButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.NoButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.NoButton.Location = new System.Drawing.Point(60, 3);
			this.NoButton.Name = "NoButton";
			this.NoButton.Size = new System.Drawing.Size(50, 23);
			this.NoButton.TabIndex = 2;
			this.NoButton.Text = "Cancel";
			this.NoButton.UseVisualStyleBackColor = true;
			this.NoButton.Click += new System.EventHandler(this.NoButton_Click);
			// 
			// ButtonPanel
			// 
			this.ButtonPanel.AutoSize = true;
			this.ButtonPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ButtonPanel.Controls.Add(this.YesButton);
			this.ButtonPanel.Controls.Add(this.NoButton);
			this.ButtonPanel.Location = new System.Drawing.Point(3, 286);
			this.ButtonPanel.Name = "ButtonPanel";
			this.ButtonPanel.Size = new System.Drawing.Size(113, 29);
			this.ButtonPanel.TabIndex = 3;
			// 
			// MainPanel
			// 
			this.MainPanel.AutoSize = true;
			this.MainPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MainPanel.Controls.Add(this.RecipesList);
			this.MainPanel.Controls.Add(this.ButtonPanel);
			this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.MainPanel.Location = new System.Drawing.Point(0, 0);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(823, 328);
			this.MainPanel.TabIndex = 4;
			// 
			// SelectRecipePrompt
			// 
			this.AcceptButton = this.YesButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelButton = this.NoButton;
			this.ClientSize = new System.Drawing.Size(823, 328);
			this.Controls.Add(this.MainPanel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SelectRecipePrompt";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Recipe";
			this.ButtonPanel.ResumeLayout(false);
			this.ButtonPanel.PerformLayout();
			this.MainPanel.ResumeLayout(false);
			this.MainPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox RecipesList;
		private System.Windows.Forms.Button YesButton;
		private System.Windows.Forms.Button NoButton;
		private System.Windows.Forms.FlowLayoutPanel ButtonPanel;
		private System.Windows.Forms.FlowLayoutPanel MainPanel;
	}
}