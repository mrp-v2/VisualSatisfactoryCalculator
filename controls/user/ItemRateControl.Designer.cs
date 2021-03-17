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
			this.RateNumeric = new System.Windows.Forms.NumericUpDown();
			this.MainPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.ItemButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.RateNumeric)).BeginInit();
			this.MainPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// RateNumeric
			// 
			this.RateNumeric.AutoSize = true;
			this.RateNumeric.DecimalPlaces = 3;
			this.RateNumeric.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RateNumeric.Location = new System.Drawing.Point(3, 3);
			this.RateNumeric.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
			this.RateNumeric.Name = "RateNumeric";
			this.RateNumeric.Size = new System.Drawing.Size(129, 24);
			this.RateNumeric.TabIndex = 1;
			this.RateNumeric.ThousandsSeparator = true;
			this.RateNumeric.ValueChanged += new System.EventHandler(this.RateNumeric_ValueChanged);
			// 
			// MainPanel
			// 
			this.MainPanel.AutoSize = true;
			this.MainPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MainPanel.Controls.Add(this.RateNumeric);
			this.MainPanel.Controls.Add(this.ItemButton);
			this.MainPanel.Location = new System.Drawing.Point(0, 0);
			this.MainPanel.Margin = new System.Windows.Forms.Padding(0);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(240, 33);
			this.MainPanel.TabIndex = 0;
			// 
			// ItemButton
			// 
			this.ItemButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.ItemButton.AutoSize = true;
			this.ItemButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ItemButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ItemButton.Location = new System.Drawing.Point(138, 3);
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
			this.Size = new System.Drawing.Size(240, 33);
			((System.ComponentModel.ISupportInitialize)(this.RateNumeric)).EndInit();
			this.MainPanel.ResumeLayout(false);
			this.MainPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.NumericUpDown RateNumeric;
		private System.Windows.Forms.FlowLayoutPanel MainPanel;
		private System.Windows.Forms.Button ItemButton;
	}
}
