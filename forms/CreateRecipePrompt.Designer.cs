namespace VisualSatisfactoryCalculator.forms
{
	partial class CreateRecipePrompt
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateRecipePrompt));
			this.RemoveProductButton = new System.Windows.Forms.Button();
			this.ItemsPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.ProductsPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.ProductsLabel = new System.Windows.Forms.Label();
			this.ProductsList = new System.Windows.Forms.ListBox();
			this.AddProductButton = new System.Windows.Forms.Button();
			this.IngredientsPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.IngredientsLabel = new System.Windows.Forms.Label();
			this.IngredientsList = new System.Windows.Forms.ListBox();
			this.RemoveIngredientButton = new System.Windows.Forms.Button();
			this.AddIngredientButton = new System.Windows.Forms.Button();
			this.NoButton = new System.Windows.Forms.Button();
			this.YesButton = new System.Windows.Forms.Button();
			this.CraftTimeNumeric = new System.Windows.Forms.NumericUpDown();
			this.CraftTimeLabel = new System.Windows.Forms.Label();
			this.MachineNameCombo = new System.Windows.Forms.ComboBox();
			this.MachineNameLabel = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
			this.CraftTimePanel = new System.Windows.Forms.FlowLayoutPanel();
			this.MachineNamePanel = new System.Windows.Forms.FlowLayoutPanel();
			this.ButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.ItemsPanel.SuspendLayout();
			this.ProductsPanel.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.IngredientsPanel.SuspendLayout();
			this.flowLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.CraftTimeNumeric)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel4.SuspendLayout();
			this.CraftTimePanel.SuspendLayout();
			this.MachineNamePanel.SuspendLayout();
			this.ButtonPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// RemoveProductButton
			// 
			this.RemoveProductButton.Location = new System.Drawing.Point(3, 123);
			this.RemoveProductButton.Name = "RemoveProductButton";
			this.RemoveProductButton.Size = new System.Drawing.Size(110, 25);
			this.RemoveProductButton.TabIndex = 0;
			this.RemoveProductButton.Text = "Remove Product";
			this.RemoveProductButton.UseVisualStyleBackColor = true;
			this.RemoveProductButton.Click += new System.EventHandler(this.RemoveProductButton_Click);
			// 
			// ItemsPanel
			// 
			this.ItemsPanel.AutoSize = true;
			this.ItemsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ItemsPanel.Controls.Add(this.ProductsPanel);
			this.ItemsPanel.Controls.Add(this.IngredientsPanel);
			this.ItemsPanel.Location = new System.Drawing.Point(3, 3);
			this.ItemsPanel.Name = "ItemsPanel";
			this.ItemsPanel.Size = new System.Drawing.Size(424, 188);
			this.ItemsPanel.TabIndex = 0;
			// 
			// ProductsPanel
			// 
			this.ProductsPanel.AutoSize = true;
			this.ProductsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ProductsPanel.Controls.Add(this.flowLayoutPanel2);
			this.ProductsPanel.Controls.Add(this.ProductsList);
			this.ProductsPanel.Controls.Add(this.RemoveProductButton);
			this.ProductsPanel.Controls.Add(this.AddProductButton);
			this.ProductsPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.ProductsPanel.Location = new System.Drawing.Point(3, 3);
			this.ProductsPanel.Name = "ProductsPanel";
			this.ProductsPanel.Size = new System.Drawing.Size(206, 182);
			this.ProductsPanel.TabIndex = 0;
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.AutoSize = true;
			this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel2.Controls.Add(this.ProductsLabel);
			this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(55, 13);
			this.flowLayoutPanel2.TabIndex = 1;
			// 
			// ProductsLabel
			// 
			this.ProductsLabel.AutoSize = true;
			this.ProductsLabel.Location = new System.Drawing.Point(3, 0);
			this.ProductsLabel.Name = "ProductsLabel";
			this.ProductsLabel.Size = new System.Drawing.Size(49, 13);
			this.ProductsLabel.TabIndex = 0;
			this.ProductsLabel.Text = "Products";
			// 
			// ProductsList
			// 
			this.ProductsList.FormattingEnabled = true;
			this.ProductsList.Location = new System.Drawing.Point(3, 22);
			this.ProductsList.Name = "ProductsList";
			this.ProductsList.Size = new System.Drawing.Size(200, 95);
			this.ProductsList.TabIndex = 0;
			// 
			// AddProductButton
			// 
			this.AddProductButton.Location = new System.Drawing.Point(3, 154);
			this.AddProductButton.Name = "AddProductButton";
			this.AddProductButton.Size = new System.Drawing.Size(110, 25);
			this.AddProductButton.TabIndex = 0;
			this.AddProductButton.Text = "Add Product";
			this.AddProductButton.UseVisualStyleBackColor = true;
			this.AddProductButton.Click += new System.EventHandler(this.AddProductButton_Click);
			// 
			// IngredientsPanel
			// 
			this.IngredientsPanel.AutoSize = true;
			this.IngredientsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.IngredientsPanel.Controls.Add(this.flowLayoutPanel3);
			this.IngredientsPanel.Controls.Add(this.IngredientsList);
			this.IngredientsPanel.Controls.Add(this.RemoveIngredientButton);
			this.IngredientsPanel.Controls.Add(this.AddIngredientButton);
			this.IngredientsPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.IngredientsPanel.Location = new System.Drawing.Point(215, 3);
			this.IngredientsPanel.Name = "IngredientsPanel";
			this.IngredientsPanel.Size = new System.Drawing.Size(206, 182);
			this.IngredientsPanel.TabIndex = 0;
			// 
			// flowLayoutPanel3
			// 
			this.flowLayoutPanel3.AutoSize = true;
			this.flowLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel3.Controls.Add(this.IngredientsLabel);
			this.flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 3);
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			this.flowLayoutPanel3.Size = new System.Drawing.Size(65, 13);
			this.flowLayoutPanel3.TabIndex = 5;
			// 
			// IngredientsLabel
			// 
			this.IngredientsLabel.AutoSize = true;
			this.IngredientsLabel.Location = new System.Drawing.Point(3, 0);
			this.IngredientsLabel.Name = "IngredientsLabel";
			this.IngredientsLabel.Size = new System.Drawing.Size(59, 13);
			this.IngredientsLabel.TabIndex = 0;
			this.IngredientsLabel.Text = "Ingredients";
			// 
			// IngredientsList
			// 
			this.IngredientsList.FormattingEnabled = true;
			this.IngredientsList.Location = new System.Drawing.Point(3, 22);
			this.IngredientsList.Name = "IngredientsList";
			this.IngredientsList.Size = new System.Drawing.Size(200, 95);
			this.IngredientsList.TabIndex = 0;
			// 
			// RemoveIngredientButton
			// 
			this.RemoveIngredientButton.Location = new System.Drawing.Point(3, 123);
			this.RemoveIngredientButton.Name = "RemoveIngredientButton";
			this.RemoveIngredientButton.Size = new System.Drawing.Size(110, 25);
			this.RemoveIngredientButton.TabIndex = 0;
			this.RemoveIngredientButton.Text = "Remove Ingredient";
			this.RemoveIngredientButton.UseVisualStyleBackColor = true;
			this.RemoveIngredientButton.Click += new System.EventHandler(this.RemoveIngredientButton_Click);
			// 
			// AddIngredientButton
			// 
			this.AddIngredientButton.Location = new System.Drawing.Point(3, 154);
			this.AddIngredientButton.Name = "AddIngredientButton";
			this.AddIngredientButton.Size = new System.Drawing.Size(110, 25);
			this.AddIngredientButton.TabIndex = 0;
			this.AddIngredientButton.Text = "Add Ingredient";
			this.AddIngredientButton.UseVisualStyleBackColor = true;
			this.AddIngredientButton.Click += new System.EventHandler(this.AddIngredientButton_Click);
			// 
			// NoButton
			// 
			this.NoButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.NoButton.Location = new System.Drawing.Point(84, 3);
			this.NoButton.Name = "NoButton";
			this.NoButton.Size = new System.Drawing.Size(75, 23);
			this.NoButton.TabIndex = 4;
			this.NoButton.Text = "Cancel";
			this.NoButton.UseVisualStyleBackColor = true;
			this.NoButton.Click += new System.EventHandler(this.NoButton_Click);
			// 
			// YesButton
			// 
			this.YesButton.Location = new System.Drawing.Point(3, 3);
			this.YesButton.Name = "YesButton";
			this.YesButton.Size = new System.Drawing.Size(75, 23);
			this.YesButton.TabIndex = 3;
			this.YesButton.Text = "Add";
			this.YesButton.UseVisualStyleBackColor = true;
			this.YesButton.Click += new System.EventHandler(this.YesButton_Click);
			// 
			// CraftTimeNumeric
			// 
			this.CraftTimeNumeric.Location = new System.Drawing.Point(3, 16);
			this.CraftTimeNumeric.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
			this.CraftTimeNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.CraftTimeNumeric.Name = "CraftTimeNumeric";
			this.CraftTimeNumeric.Size = new System.Drawing.Size(55, 20);
			this.CraftTimeNumeric.TabIndex = 2;
			this.CraftTimeNumeric.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// CraftTimeLabel
			// 
			this.CraftTimeLabel.AutoSize = true;
			this.CraftTimeLabel.Location = new System.Drawing.Point(3, 0);
			this.CraftTimeLabel.Name = "CraftTimeLabel";
			this.CraftTimeLabel.Size = new System.Drawing.Size(111, 13);
			this.CraftTimeLabel.TabIndex = 0;
			this.CraftTimeLabel.Text = "Craft Time in Seconds";
			// 
			// MachineNameCombo
			// 
			this.MachineNameCombo.FormattingEnabled = true;
			this.MachineNameCombo.Location = new System.Drawing.Point(3, 16);
			this.MachineNameCombo.Name = "MachineNameCombo";
			this.MachineNameCombo.Size = new System.Drawing.Size(176, 21);
			this.MachineNameCombo.TabIndex = 1;
			// 
			// MachineNameLabel
			// 
			this.MachineNameLabel.AutoSize = true;
			this.MachineNameLabel.Location = new System.Drawing.Point(3, 0);
			this.MachineNameLabel.Name = "MachineNameLabel";
			this.MachineNameLabel.Size = new System.Drawing.Size(79, 13);
			this.MachineNameLabel.TabIndex = 0;
			this.MachineNameLabel.Text = "Machine Name";
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.ItemsPanel);
			this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel4);
			this.flowLayoutPanel1.Controls.Add(this.ButtonPanel);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(435, 282);
			this.flowLayoutPanel1.TabIndex = 0;
			// 
			// flowLayoutPanel4
			// 
			this.flowLayoutPanel4.AutoSize = true;
			this.flowLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel4.Controls.Add(this.CraftTimePanel);
			this.flowLayoutPanel4.Controls.Add(this.MachineNamePanel);
			this.flowLayoutPanel4.Location = new System.Drawing.Point(3, 197);
			this.flowLayoutPanel4.Name = "flowLayoutPanel4";
			this.flowLayoutPanel4.Size = new System.Drawing.Size(311, 46);
			this.flowLayoutPanel4.TabIndex = 0;
			// 
			// CraftTimePanel
			// 
			this.CraftTimePanel.AutoSize = true;
			this.CraftTimePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CraftTimePanel.Controls.Add(this.CraftTimeLabel);
			this.CraftTimePanel.Controls.Add(this.CraftTimeNumeric);
			this.CraftTimePanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.CraftTimePanel.Location = new System.Drawing.Point(3, 3);
			this.CraftTimePanel.Name = "CraftTimePanel";
			this.CraftTimePanel.Size = new System.Drawing.Size(117, 39);
			this.CraftTimePanel.TabIndex = 0;
			// 
			// MachineNamePanel
			// 
			this.MachineNamePanel.AutoSize = true;
			this.MachineNamePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MachineNamePanel.Controls.Add(this.MachineNameLabel);
			this.MachineNamePanel.Controls.Add(this.MachineNameCombo);
			this.MachineNamePanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.MachineNamePanel.Location = new System.Drawing.Point(126, 3);
			this.MachineNamePanel.Name = "MachineNamePanel";
			this.MachineNamePanel.Size = new System.Drawing.Size(182, 40);
			this.MachineNamePanel.TabIndex = 0;
			// 
			// ButtonPanel
			// 
			this.ButtonPanel.AutoSize = true;
			this.ButtonPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ButtonPanel.Controls.Add(this.YesButton);
			this.ButtonPanel.Controls.Add(this.NoButton);
			this.ButtonPanel.Location = new System.Drawing.Point(3, 249);
			this.ButtonPanel.Name = "ButtonPanel";
			this.ButtonPanel.Size = new System.Drawing.Size(162, 29);
			this.ButtonPanel.TabIndex = 0;
			// 
			// CreateRecipePrompt
			// 
			this.AcceptButton = this.YesButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.CancelButton = this.NoButton;
			this.ClientSize = new System.Drawing.Size(435, 282);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CreateRecipePrompt";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Recipe Prompt";
			this.ItemsPanel.ResumeLayout(false);
			this.ItemsPanel.PerformLayout();
			this.ProductsPanel.ResumeLayout(false);
			this.ProductsPanel.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.IngredientsPanel.ResumeLayout(false);
			this.IngredientsPanel.PerformLayout();
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.CraftTimeNumeric)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.flowLayoutPanel4.ResumeLayout(false);
			this.flowLayoutPanel4.PerformLayout();
			this.CraftTimePanel.ResumeLayout(false);
			this.CraftTimePanel.PerformLayout();
			this.MachineNamePanel.ResumeLayout(false);
			this.MachineNamePanel.PerformLayout();
			this.ButtonPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button RemoveProductButton;
		private System.Windows.Forms.FlowLayoutPanel ItemsPanel;
		private System.Windows.Forms.FlowLayoutPanel ProductsPanel;
		private System.Windows.Forms.Button AddProductButton;
		private System.Windows.Forms.FlowLayoutPanel IngredientsPanel;
		private System.Windows.Forms.Button AddIngredientButton;
		private System.Windows.Forms.Button RemoveIngredientButton;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Label ProductsLabel;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
		private System.Windows.Forms.Label IngredientsLabel;
		private System.Windows.Forms.Label CraftTimeLabel;
		private System.Windows.Forms.Label MachineNameLabel;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.FlowLayoutPanel CraftTimePanel;
		private System.Windows.Forms.FlowLayoutPanel MachineNamePanel;
		private System.Windows.Forms.FlowLayoutPanel ButtonPanel;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
		public System.Windows.Forms.Button NoButton;
		public System.Windows.Forms.Button YesButton;
		public System.Windows.Forms.NumericUpDown CraftTimeNumeric;
		public System.Windows.Forms.ComboBox MachineNameCombo;
		public System.Windows.Forms.ListBox ProductsList;
		public System.Windows.Forms.ListBox IngredientsList;
	}
}