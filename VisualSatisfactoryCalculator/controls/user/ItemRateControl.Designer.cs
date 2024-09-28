using System.Windows.Forms;

using VisualSatisfactoryCalculator.satisfactory.Utility;

namespace VisualSatisfactoryCalculator.controls.user
{
	partial class ItemRateControl
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
			this.NumberControl = new System.Windows.Forms.NumericUpDown();
			this.ItemButton = new System.Windows.Forms.Button();
			this.MainPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.NumberControl)).BeginInit();
			this.SuspendLayout();
			// 
			// MainPanel
			// 
			this.MainPanel.AutoSize = true;
			this.MainPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MainPanel.Controls.Add(this.NumberControl);
			this.MainPanel.Controls.Add(this.ItemButton);
			this.MainPanel.Location = new System.Drawing.Point(0, 0);
			this.MainPanel.Margin = new System.Windows.Forms.Padding(0);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(159, 33);
			this.MainPanel.TabIndex = 0;
			// 
			// NumberControl
			// 
			this.NumberControl.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.NumberControl.AutoSize = true;
			this.NumberControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.NumberControl.Location = new System.Drawing.Point(3, 5);
			this.NumberControl.Name = "NumberControl";
			this.NumberControl.Size = new System.Drawing.Size(48, 23);
			this.NumberControl.TabIndex = 3;
			// 
			// ItemButton
			// 
			this.ItemButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.ItemButton.AutoSize = true;
			this.ItemButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ItemButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ItemButton.Location = new System.Drawing.Point(57, 3);
			this.ItemButton.Name = "ItemButton";
			this.ItemButton.Size = new System.Drawing.Size(99, 27);
			this.ItemButton.TabIndex = 2;
			this.ItemButton.Text = "<item name>";
			this.ItemButton.UseVisualStyleBackColor = true;
			this.ItemButton.Click += new System.EventHandler(this.ItemButton_Click);
			// 
			// ItemRateControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.MainPanel);
			this.Name = "ItemRateControl";
			this.Size = new System.Drawing.Size(159, 33);
			this.MainPanel.ResumeLayout(false);
			this.MainPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.NumberControl)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.FlowLayoutPanel MainPanel;
		public System.Windows.Forms.Button ItemButton;
		private NumericUpDown NumberControl;
	}
}
