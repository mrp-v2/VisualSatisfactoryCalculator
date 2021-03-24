using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MrpV2.GenericLibrary.code.dictionary.bidirectional.classes;

using VisualSatisfactoryCalculator.code.Production;
using VisualSatisfactoryCalculator.controls.user;
using VisualSatisfactoryCalculator.forms;

namespace VisualSatisfactoryCalculator.code.Utility
{
	public class PlanLayoutMaker
	{
		public static void LayoutSteps(MainForm mainForm, Panel panel, Plan plan)
		{
			int yPosition = panel.GetPreferredSize(new Size()).Height, xPosition = 0;
			Dictionary<Step, Tuple<StepControl, StepAndIngredientsLayout>> stepUIStuff = new Dictionary<Step, Tuple<StepControl, StepAndIngredientsLayout>>();
			for (int currentTier = plan.ProcessedPlan.Get().Tiers - 1; currentTier >= 0; currentTier--)
			{
				foreach (Step step in plan.ProcessedPlan.Get().GetStepsInTier(currentTier))
				{
					StepControl stepControl = new StepControl(step, mainForm);
					Dictionary<ILayoutControl, string> ingredientControls = new Dictionary<ILayoutControl, string>();
					foreach (Connection connection in step.NormalIngredientConnections.Get())
					{
						foreach (Step ingredientStep in connection.GetProducerSteps())
						{
							if (stepUIStuff.ContainsKey(ingredientStep))
							{
								ingredientControls.Add(stepUIStuff[ingredientStep].Item2, connection.ItemUID);
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
			foreach (Step step in plan.ProcessedPlan.Get().GetStepsInTier(0))
			{
				StepAndIngredientsLayout layout = stepUIStuff[step].Item2;
				layout.Place(xPosition, yPosition);
				xPosition += layout.PreferredSize.Width;
			}
		}

		private class StepAndIngredientsLayout : ILayoutControl
		{
			public StepControl TopControl { get; }
			private readonly Dictionary<ILayoutControl, string> ingredientControls;
			public Size PreferredSize { get; }
			private readonly Size productPreferredSize;
			private bool placed = false;

			public StepAndIngredientsLayout(StepControl productControl, Dictionary<ILayoutControl, string> ingredientControls)
			{
				TopControl = productControl;
				this.ingredientControls = ingredientControls;
				int width1 = 0, height = 0;
				foreach (ILayoutControl ingredientControl in ingredientControls.Keys)
				{
					Size preferredSize = ingredientControl.PreferredSize;
					width1 += preferredSize.Width;
					height = preferredSize.Height > height ? preferredSize.Height : height;
				}
				productPreferredSize = productControl.PreferredSize;
				int width2 = productPreferredSize.Width;
				int width3 = width1 > width2 ? width1 : width2;
				height += productPreferredSize.Height;
				PreferredSize = new Size(width3, height);
			}

			public void Place(int xStart, int yStart)
			{
				if (placed)
				{
					throw new InvalidOperationException("Can't place a layout piece that has already been placed");
				}
				TopControl.Place(xStart + (PreferredSize.Width / 2) - (productPreferredSize.Width / 2), yStart);
				int currentX = xStart, y = yStart + productPreferredSize.Height;
				Dictionary<string, ILayoutControl> reverseIngredientControls = new Dictionary<string, ILayoutControl>();
				foreach (ILayoutControl control in ingredientControls.Keys)
				{
					reverseIngredientControls.Add(ingredientControls[control], control);
				}
				List<ILayoutControl> orderedIngredients = new List<ILayoutControl>();
				foreach (string str in TopControl.BackingStep.Recipe.Ingredients.Keys)
				{
					if (reverseIngredientControls.ContainsKey(str))
					{
						orderedIngredients.Add(reverseIngredientControls[str]);
					}
				}
				if (PreferredSize.Width == productPreferredSize.Width)
				{
					int ingredientsWidth = 0;
					foreach (ILayoutControl control in ingredientControls.Keys)
					{
						ingredientsWidth += control.PreferredSize.Width;
					}
					currentX += (PreferredSize.Width - ingredientsWidth) / 2;
				}
				y += 10;
				foreach (ILayoutControl control in orderedIngredients)
				{
					control.Place(currentX, y);
					currentX += control.PreferredSize.Width;
				}
				Dictionary<string, Point> ingredientIRCConnectionPoints = new Dictionary<string, Point>();
				Dictionary<string, Point> productIRCConnectionPoints = new Dictionary<string, Point>();
				foreach (ILayoutControl control in orderedIngredients)
				{
					string item = ingredientControls[control];
					ItemRateControl productIRC = control.TopControl.ProductRateControls[item];
					Point productIRCLoc = AddParentPoints(productIRC, 4);
					productIRCConnectionPoints.Add(item, new Point(productIRCLoc.X + (productIRC.Size.Width / 2), productIRCLoc.Y));
					ItemRateControl ingredientIRC = TopControl.IngredientRateControls[item];
					Point ingredientIRCLoc = AddParentPoints(ingredientIRC, 4);
					ingredientIRCConnectionPoints.Add(item, new Point(ingredientIRCLoc.X + (ingredientIRC.Size.Width / 2), ingredientIRCLoc.Y + ingredientIRC.Size.Height));
				}
				Dictionary<string, Range> itemLineRanges = new Dictionary<string, Range>();
				foreach (ILayoutControl control in orderedIngredients)
				{
					string str = ingredientControls[control];
					int left1 = ingredientIRCConnectionPoints[str].X, left2 = productIRCConnectionPoints[str].X;
					int left3 = left1 < left2 ? left1 : left2;
					int right1 = ingredientIRCConnectionPoints[str].X, right2 = productIRCConnectionPoints[str].X;
					int right3 = right1 > right2 ? right1 : right2;
					itemLineRanges.Add(str, new Range(left3, right3));
				}
				int maxOverlaps = Range.MaxOverlaps(itemLineRanges.Values);
				Random rand = new Random();
				foreach (ILayoutControl control in orderedIngredients)
				{
					string str = ingredientControls[control];
					LineControl line1 = new LineControl(), line2 = new LineControl(), line3 = new LineControl();
					TopControl.Parent.Controls.AddRange(new Control[] { line1, line2, line3 });
					line1.BringToFront();
					line2.BringToFront();
					line3.BringToFront();
					Color color = Color.FromArgb((0xFF << 24) | (rand.Next(64, 197) << 16) | (rand.Next(64, 197) << 8) | (rand.Next(64, 197)));
					line1.BackColor = color;
					line2.BackColor = color;
					line3.BackColor = color;
					line1.Location = AddPoints(ingredientIRCConnectionPoints[str], new Point(-line1.Size.Width / 2, -line1.Size.Height / 2));
					line3.Location = AddPoints(productIRCConnectionPoints[str], new Point(-line3.Size.Width / 2, -line3.Size.Height / 2));
					int height = (line1.Location.Y + line3.Location.Y) / 2;
					line2.Location = new Point(itemLineRanges[str].Left - (line2.Size.Width / 2), (line1.Location.Y + line3.Location.Y) / 2);
					line2.Size = new Size(itemLineRanges[str].Length + line2.Size.Width, line2.Size.Height);
					line1.Size = new Size(line1.Size.Width, height - line1.Location.Y);
					line3.Size = new Size(line3.Size.Width, line3.Location.Y - height + line3.Size.Height);
					line3.Location = AddPoints(line3.Location, new Point(0, height - line3.Location.Y));
				}
				placed = true;
			}
		}

		private struct Range
		{
			public readonly int Left, Right;
			public readonly int Length;

			public Range(int left, int right)
			{
				if (left > right)
				{
					int temp = right;
					right = left;
					left = temp;
				}
				Left = left;
				Right = right;
				Length = Right - Left;
			}

			public bool Overlaps(Range other)
			{
				return !(Right < other.Left || Left > other.Right);
			}

			public static int MaxOverlaps(IEnumerable<Range> ranges)
			{
				int maxOverlaps = 0;
				Range totalRange = Total(ranges);
				for (int i = totalRange.Left; i <= totalRange.Right; i++)
				{
					int overlaps = OverlapsAt(ranges, i);
					if (overlaps > maxOverlaps)
					{
						maxOverlaps = overlaps;
					}
				}
				return maxOverlaps;
			}

			public bool Includes(int position)
			{
				return position > Left && position < Right;
			}

			public static int OverlapsAt(IEnumerable<Range> ranges, int position)
			{
				int overlaps = 0;
				foreach (Range range in ranges)
				{
					if (range.Includes(position))
					{
						overlaps++;
					}
				}
				return overlaps;
			}

			public static Range Total(IEnumerable<Range> ranges)
			{
				int left = 0, right = 0;
				foreach (Range range in ranges)
				{
					if (range.Left < left)
					{
						left = range.Left;
					}
					if (range.Right > right)
					{
						right = range.Right;
					}
				}
				return new Range(left, right);
			}
		}

		public interface ILayoutControl
		{
			Size PreferredSize { get; }
			void Place(int xStart, int yStart);
			StepControl TopControl { get; }
		}

		private static Point AddParentPoints(Control child, int parentCount)
		{
			if (parentCount == 0)
			{
				return child.Location;
			}
			return AddPoints(child.Location, AddParentPoints(child.Parent, parentCount - 1));
		}

		private static Point AddPoints(Point a, Point b)
		{
			return new Point(a.X + b.X, a.Y + b.Y);
		}
	}
}
