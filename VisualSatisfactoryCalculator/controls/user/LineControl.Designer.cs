
namespace VisualSatisfactoryCalculator.controls.user
{
	partial class LineControl
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
			this.LineLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// LineLabel
			// 
			this.LineLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.LineLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F);
			this.LineLabel.Location = new System.Drawing.Point(0, 0);
			this.LineLabel.Margin = new System.Windows.Forms.Padding(0);
			this.LineLabel.Name = "LineLabel";
			this.LineLabel.Size = new System.Drawing.Size(5, 5);
			this.LineLabel.TabIndex = 0;
			this.LineLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LineControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.DimGray;
			this.Controls.Add(this.LineLabel);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "LineControl";
			this.Size = new System.Drawing.Size(5, 5);
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.Label LineLabel;
	}
}
