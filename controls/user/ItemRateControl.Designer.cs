using VisualSatisfactoryCalculator.code.Utility;

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
			this.ItemButton = new System.Windows.Forms.Button();
			this.NumberControl = new RationalNumberControl();
			this.MainPanel.SuspendLayout();
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
			this.MainPanel.Size = new System.Drawing.Size(237, 64);
			this.MainPanel.TabIndex = 0;
			// 
			// ItemButton
			// 
			this.ItemButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.ItemButton.AutoSize = true;
			this.ItemButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ItemButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ItemButton.Location = new System.Drawing.Point(135, 18);
			this.ItemButton.Name = "ItemButton";
			this.ItemButton.Size = new System.Drawing.Size(99, 27);
			this.ItemButton.TabIndex = 2;
			this.ItemButton.Text = "<item name>";
			this.ItemButton.UseVisualStyleBackColor = true;
			this.ItemButton.Click += new System.EventHandler(this.ItemButton_Click);
			//
			// NumberControl
			//
			this.NumberControl.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.NumberControl.AutoSize = true;
			this.NumberControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.NumberControl.Location = new System.Drawing.Point(0, 0);
			this.NumberControl.Name = "NumberControl";
			this.NumberControl.TabIndex = 3;
			// 
			// ItemRateControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.MainPanel);
			this.Name = "ItemRateControl";
			this.Size = new System.Drawing.Size(237, 64);
			this.MainPanel.ResumeLayout(false);
			this.MainPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion
		private System.Windows.Forms.FlowLayoutPanel MainPanel;
		public System.Windows.Forms.Button ItemButton;
		private RationalNumberControl NumberControl;
	}
}
