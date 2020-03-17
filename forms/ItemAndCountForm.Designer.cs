namespace VisualSatisfactoryCalculator.forms
{
	partial class ItemAndCountForm
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
			this.ItemCountNumeric = new System.Windows.Forms.NumericUpDown();
			this.ItemPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.ItemNameCombo = new System.Windows.Forms.ComboBox();
			this.YesButton = new System.Windows.Forms.Button();
			this.NoButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.ItemCountNumeric)).BeginInit();
			this.ItemPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// ItemCountNumeric
			// 
			this.ItemCountNumeric.Location = new System.Drawing.Point(3, 3);
			this.ItemCountNumeric.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
			this.ItemCountNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.ItemCountNumeric.Name = "ItemCountNumeric";
			this.ItemCountNumeric.Size = new System.Drawing.Size(48, 20);
			this.ItemCountNumeric.TabIndex = 0;
			this.ItemCountNumeric.ThousandsSeparator = true;
			this.ItemCountNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// ItemPanel
			// 
			this.ItemPanel.AutoSize = true;
			this.ItemPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ItemPanel.Controls.Add(this.ItemCountNumeric);
			this.ItemPanel.Controls.Add(this.ItemNameCombo);
			this.ItemPanel.Location = new System.Drawing.Point(12, 12);
			this.ItemPanel.Name = "ItemPanel";
			this.ItemPanel.Size = new System.Drawing.Size(466, 27);
			this.ItemPanel.TabIndex = 2;
			// 
			// ItemNameCombo
			// 
			this.ItemNameCombo.FormattingEnabled = true;
			this.ItemNameCombo.Location = new System.Drawing.Point(57, 3);
			this.ItemNameCombo.Name = "ItemNameCombo";
			this.ItemNameCombo.Size = new System.Drawing.Size(406, 21);
			this.ItemNameCombo.TabIndex = 5;
			// 
			// YesButton
			// 
			this.YesButton.Location = new System.Drawing.Point(12, 45);
			this.YesButton.Name = "YesButton";
			this.YesButton.Size = new System.Drawing.Size(75, 23);
			this.YesButton.TabIndex = 3;
			this.YesButton.Text = "Add";
			this.YesButton.UseVisualStyleBackColor = true;
			this.YesButton.Click += new System.EventHandler(this.OkButton_Click);
			// 
			// NoButton
			// 
			this.NoButton.Location = new System.Drawing.Point(93, 45);
			this.NoButton.Name = "NoButton";
			this.NoButton.Size = new System.Drawing.Size(75, 23);
			this.NoButton.TabIndex = 4;
			this.NoButton.Text = "Cancel";
			this.NoButton.UseVisualStyleBackColor = true;
			this.NoButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// ItemAndCountForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(487, 77);
			this.Controls.Add(this.NoButton);
			this.Controls.Add(this.YesButton);
			this.Controls.Add(this.ItemPanel);
			this.Name = "ItemAndCountForm";
			this.Text = "Item Count Prompt";
			((System.ComponentModel.ISupportInitialize)(this.ItemCountNumeric)).EndInit();
			this.ItemPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.FlowLayoutPanel ItemPanel;
		public System.Windows.Forms.Button NoButton;
		public System.Windows.Forms.Button YesButton;
		public System.Windows.Forms.NumericUpDown ItemCountNumeric;
		public System.Windows.Forms.ComboBox ItemNameCombo;
	}
}