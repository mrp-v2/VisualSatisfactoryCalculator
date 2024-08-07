using VisualSatisfactoryCalculator.model.production;

namespace VisualSatisfactoryCalculator.controls.user
{
	partial class SingleConnectedStepGroupControl<ItemType> where ItemType : AbstractItem
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
			this.NetGroupRate = new VisualSatisfactoryCalculator.controls.user.RationalNumberControl();
			this.MainPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.ProducersPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.ConsumersPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.MainPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// NetGroupRate
			// 
			this.NetGroupRate.AutoSize = true;
			this.NetGroupRate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.NetGroupRate.Location = new System.Drawing.Point(4, 10);
			this.NetGroupRate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.NetGroupRate.Name = "NetGroupRate";
			this.NetGroupRate.Size = new System.Drawing.Size(356, 34);
			this.NetGroupRate.TabIndex = 0;
			// 
			// MainPanel
			// 
			this.MainPanel.AutoSize = true;
			this.MainPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MainPanel.Controls.Add(this.ConsumersPanel);
			this.MainPanel.Controls.Add(this.NetGroupRate);
			this.MainPanel.Controls.Add(this.ProducersPanel);
			this.MainPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.MainPanel.Location = new System.Drawing.Point(0, 0);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(364, 54);
			this.MainPanel.TabIndex = 3;
			// 
			// ProducersPanel
			// 
			this.ProducersPanel.AutoSize = true;
			this.ProducersPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ProducersPanel.Location = new System.Drawing.Point(3, 51);
			this.ProducersPanel.Name = "ProducersPanel";
			this.ProducersPanel.Size = new System.Drawing.Size(0, 0);
			this.ProducersPanel.TabIndex = 0;
			// 
			// ConsumersPanel
			// 
			this.ConsumersPanel.AutoSize = true;
			this.ConsumersPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ConsumersPanel.Location = new System.Drawing.Point(3, 3);
			this.ConsumersPanel.Name = "ConsumersPanel";
			this.ConsumersPanel.Size = new System.Drawing.Size(0, 0);
			this.ConsumersPanel.TabIndex = 0;
			// 
			// SingleConnectedStepGroupControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.MainPanel);
			this.Name = "SingleConnectedStepGroupControl";
			this.Size = new System.Drawing.Size(367, 57);
			this.MainPanel.ResumeLayout(false);
			this.MainPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private RationalNumberControl NetGroupRate;
		private System.Windows.Forms.FlowLayoutPanel MainPanel;
		private System.Windows.Forms.FlowLayoutPanel ProducersPanel;
		private System.Windows.Forms.FlowLayoutPanel ConsumersPanel;
	}
}
