
namespace VisualSatisfactoryCalculator.controls.user
{
	partial class AddConnectionControl
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
			this.AddConnectionButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// AddConnectionButton
			// 
			this.AddConnectionButton.BackgroundImage = global::VisualSatisfactoryCalculator.Properties.Resources.plus_button;
			this.AddConnectionButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.AddConnectionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.AddConnectionButton.Location = new System.Drawing.Point(0, 0);
			this.AddConnectionButton.Margin = new System.Windows.Forms.Padding(0);
			this.AddConnectionButton.Name = "AddConnectionButton";
			this.AddConnectionButton.Size = new System.Drawing.Size(32, 32);
			this.AddConnectionButton.TabIndex = 0;
			this.AddConnectionButton.UseVisualStyleBackColor = true;
			this.AddConnectionButton.Click += new System.EventHandler(this.AddConnectionButton_Click);
			// 
			// AddConnectionControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.AddConnectionButton);
			this.Name = "AddConnectionControl";
			this.Size = new System.Drawing.Size(32, 32);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button AddConnectionButton;
	}
}
