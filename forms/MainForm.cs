using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualSatisfactoryCalculator.code;

namespace VisualSatisfactoryCalculator.forms
{
	public partial class MainForm : Form, IReceivesRecipe
	{
		[STAThread]
		public static void Main()
		{
			SuggestionsController.SC = new SuggestionsController();
			Application.EnableVisualStyles();
			Application.Run(new MainForm());
		}

		protected List<Recipe> currentRecipes;

		public MainForm()
		{
			InitializeComponent();
			currentRecipes = new List<Recipe>();
		}

		private void AddRecipeButton_Click(object sender, EventArgs e)
		{
			new RecipeForm(this).ShowDialog();
		}

		public void AddRecipe(Recipe recipe, string purpose = null)
		{
			currentRecipes.Add(recipe);
			UpdateGraphic();
		}

		protected void UpdateGraphic()
		{
			Bitmap map = new Bitmap(750, 750);
			using (Graphics g = Graphics.FromImage(map))
			{
				g.DrawString(currentRecipes.ToStringC(), SystemFonts.DefaultFont, Brushes.Black, new Point(10, 10));
			}
			CurrentChart.Image = map;
		}
	}
}
