namespace VisualSatisfactoryCalculator.forms
{
	partial class RecipeListForm
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
			this.RecipeList = new System.Windows.Forms.ListBox();
			this.AddRecipeButton = new System.Windows.Forms.Button();
			this.RemoveRecipeButton = new System.Windows.Forms.Button();
			this.FinishButton = new System.Windows.Forms.Button();
			this.CancelOperationButton = new System.Windows.Forms.Button();
			this.RecipeInteractionPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.ControlPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.MainPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.RecipeInteractionPanel.SuspendLayout();
			this.ControlPanel.SuspendLayout();
			this.MainPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// RecipeList
			// 
			this.RecipeList.FormattingEnabled = true;
			this.RecipeList.Location = new System.Drawing.Point(3, 38);
			this.RecipeList.Name = "RecipeList";
			this.RecipeList.Size = new System.Drawing.Size(948, 563);
			this.RecipeList.TabIndex = 0;
			// 
			// AddRecipeButton
			// 
			this.AddRecipeButton.AutoSize = true;
			this.AddRecipeButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.AddRecipeButton.Location = new System.Drawing.Point(3, 3);
			this.AddRecipeButton.Name = "AddRecipeButton";
			this.AddRecipeButton.Size = new System.Drawing.Size(73, 23);
			this.AddRecipeButton.TabIndex = 1;
			this.AddRecipeButton.Text = "Add Recipe";
			this.AddRecipeButton.UseVisualStyleBackColor = true;
			this.AddRecipeButton.Click += new System.EventHandler(this.AddRecipeButton_Click);
			// 
			// RemoveRecipeButton
			// 
			this.RemoveRecipeButton.AutoSize = true;
			this.RemoveRecipeButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.RemoveRecipeButton.Location = new System.Drawing.Point(82, 3);
			this.RemoveRecipeButton.Name = "RemoveRecipeButton";
			this.RemoveRecipeButton.Size = new System.Drawing.Size(94, 23);
			this.RemoveRecipeButton.TabIndex = 2;
			this.RemoveRecipeButton.Text = "Remove Recipe";
			this.RemoveRecipeButton.UseVisualStyleBackColor = true;
			this.RemoveRecipeButton.Click += new System.EventHandler(this.RemoveRecipeButton_Click);
			// 
			// FinishButton
			// 
			this.FinishButton.AutoSize = true;
			this.FinishButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.FinishButton.Location = new System.Drawing.Point(3, 3);
			this.FinishButton.Name = "FinishButton";
			this.FinishButton.Size = new System.Drawing.Size(44, 23);
			this.FinishButton.TabIndex = 3;
			this.FinishButton.Text = "Finish";
			this.FinishButton.UseVisualStyleBackColor = true;
			this.FinishButton.Click += new System.EventHandler(this.FinishButton_Click);
			// 
			// CancelOperationButton
			// 
			this.CancelOperationButton.AutoSize = true;
			this.CancelOperationButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelOperationButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelOperationButton.Location = new System.Drawing.Point(53, 3);
			this.CancelOperationButton.Name = "CancelOperationButton";
			this.CancelOperationButton.Size = new System.Drawing.Size(50, 23);
			this.CancelOperationButton.TabIndex = 4;
			this.CancelOperationButton.Text = "Cancel";
			this.CancelOperationButton.UseVisualStyleBackColor = true;
			this.CancelOperationButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// RecipeInteractionPanel
			// 
			this.RecipeInteractionPanel.AutoSize = true;
			this.RecipeInteractionPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.RecipeInteractionPanel.Controls.Add(this.AddRecipeButton);
			this.RecipeInteractionPanel.Controls.Add(this.RemoveRecipeButton);
			this.RecipeInteractionPanel.Location = new System.Drawing.Point(3, 3);
			this.RecipeInteractionPanel.Name = "RecipeInteractionPanel";
			this.RecipeInteractionPanel.Size = new System.Drawing.Size(179, 29);
			this.RecipeInteractionPanel.TabIndex = 5;
			// 
			// ControlPanel
			// 
			this.ControlPanel.AutoSize = true;
			this.ControlPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ControlPanel.Controls.Add(this.FinishButton);
			this.ControlPanel.Controls.Add(this.CancelOperationButton);
			this.ControlPanel.Location = new System.Drawing.Point(3, 607);
			this.ControlPanel.Name = "ControlPanel";
			this.ControlPanel.Size = new System.Drawing.Size(106, 29);
			this.ControlPanel.TabIndex = 6;
			// 
			// MainPanel
			// 
			this.MainPanel.Controls.Add(this.RecipeInteractionPanel);
			this.MainPanel.Controls.Add(this.RecipeList);
			this.MainPanel.Controls.Add(this.ControlPanel);
			this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.MainPanel.Location = new System.Drawing.Point(0, 0);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(963, 641);
			this.MainPanel.TabIndex = 7;
			// 
			// RecipeListForm
			// 
			this.AcceptButton = this.FinishButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.CancelButton = this.CancelOperationButton;
			this.ClientSize = new System.Drawing.Size(963, 641);
			this.Controls.Add(this.MainPanel);
			this.Name = "RecipeListForm";
			this.Text = "RecipeListForm";
			this.RecipeInteractionPanel.ResumeLayout(false);
			this.RecipeInteractionPanel.PerformLayout();
			this.ControlPanel.ResumeLayout(false);
			this.ControlPanel.PerformLayout();
			this.MainPanel.ResumeLayout(false);
			this.MainPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox RecipeList;
		private System.Windows.Forms.Button AddRecipeButton;
		private System.Windows.Forms.Button RemoveRecipeButton;
		private System.Windows.Forms.Button FinishButton;
		private System.Windows.Forms.Button CancelOperationButton;
		private System.Windows.Forms.FlowLayoutPanel RecipeInteractionPanel;
		private System.Windows.Forms.FlowLayoutPanel ControlPanel;
		private System.Windows.Forms.FlowLayoutPanel MainPanel;
	}
}