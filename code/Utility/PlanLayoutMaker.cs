using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.code.Production;
using VisualSatisfactoryCalculator.controls.user;
using VisualSatisfactoryCalculator.forms;

namespace VisualSatisfactoryCalculator.code.Utility
{
	public class PlanLayoutMaker
	{
		public static void LayoutSteps(MainForm mainForm, Panel panel, Plan plan)
		{
			int startingY = panel.GetPreferredSize(new Size()).Height;
			Dictionary<Step, Tuple<StepControl, StepAndIngredientsLayout>> stepUIStuff = new Dictionary<Step, Tuple<StepControl, StepAndIngredientsLayout>>();
			for (int currentTier = plan.ProcessedPlan.Get().Tiers - 1; currentTier >= 0; currentTier--)
			{
				foreach (Step step in plan.ProcessedPlan.Get().GetStepsInTier(currentTier))
				{
					StepControl stepControl = new StepControl(step, mainForm);
					HashSet<ILayoutControl> ingredientControls = new HashSet<ILayoutControl>();
					foreach (Connection connection in step.NormalIngredientConnections.Get())
					{
						foreach (Step ingredientStep in connection.GetProducerSteps())
						{
							if (stepUIStuff.ContainsKey(ingredientStep))
							{
								ingredientControls.Add(stepUIStuff[ingredientStep].Item2);
							}
						}
					}
					StepAndIngredientsLayout layout = new StepAndIngredientsLayout(stepControl, ingredientControls);
					stepUIStuff.Add(step, new Tuple<StepControl, StepAndIngredientsLayout>(stepControl, layout));
				}
			}
			foreach (Step step in stepUIStuff.Keys)
			{
				panel.Controls.Add(stepUIStuff[step].Item1);
			}
			int xPosition = 0, yPosition = panel.GetPreferredSize(new Size()).Height;
			foreach (Step step in plan.ProcessedPlan.Get().GetStepsInTier(0))
			{
				StepAndIngredientsLayout layout = stepUIStuff[step].Item2;
				layout.Place(xPosition, yPosition);
				xPosition += layout.GetPreferredSize().Width;
			}
		}

		private class StepAndIngredientsLayout : ILayoutControl
		{
			ILayoutControl productControl;
			List<ILayoutControl> ingredientControls;
			private readonly Size totalSize;
			private readonly Size productPreferredSize;

			public StepAndIngredientsLayout(ILayoutControl productControl, IEnumerable<ILayoutControl> ingredientControls)
			{
				this.productControl = productControl;
				this.ingredientControls = new List<ILayoutControl>(ingredientControls);
				int width1 = 0, height = 0;
				foreach (ILayoutControl ingredientControl in ingredientControls)
				{
					Size preferredSize = ingredientControl.GetPreferredSize();
					width1 += preferredSize.Width;
					height = preferredSize.Height > height ? preferredSize.Height : height;
				}
				productPreferredSize = productControl.GetPreferredSize();
				int width2 = productPreferredSize.Width;
				int width3 = width1 > width2 ? width1 : width2;
				height += productPreferredSize.Height;
				totalSize = new Size(width3, height);
			}

			public void Place(int xStart, int yStart)
			{
				productControl.Place(xStart + (totalSize.Width / 2) - (productPreferredSize.Width / 2), yStart);
				int currentX = xStart, y = yStart + productPreferredSize.Height;
				foreach (ILayoutControl control in ingredientControls)
				{
					control.Place(currentX, y);
					currentX += control.GetPreferredSize().Width;
				}
			}

			public Size GetPreferredSize()
			{
				return totalSize;
			}
		}

		public interface ILayoutControl
		{
			Size GetPreferredSize();
			void Place(int xStart, int yStart);
		}
	}
}
