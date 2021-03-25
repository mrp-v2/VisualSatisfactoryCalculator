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
		private static PlanDrawingContext DrawingContext;

		public static void LayoutSteps(MainForm mainForm, Panel panel, Plan plan)
		{
			DrawingContext = new PlanDrawingContext();
			int yPosition = panel.GetPreferredSize(new Size()).Height, xPosition = 0;
			for (int currentTier = plan.ProcessedPlan.Get().Tiers - 1; currentTier >= 0; currentTier--)
			{
				foreach (Step step in plan.ProcessedPlan.Get().GetStepsInTier(currentTier))
				{
					StepControl stepControl = new StepControl(step, mainForm);
					Dictionary<Step, string> ingredientControls = new Dictionary<Step, string>();
					foreach (Connection connection in step.NormalIngredientConnections.Get())
					{
						foreach (Step ingredientStep in connection.GetProducerSteps())
						{
							if (ingredientStep.Recipe.Products.Count > 1)
							{
								foreach (string itemUID in ingredientStep.Recipe.Products.Keys)
								{
									if (connection.ItemUID == itemUID)
									{
										goto Add;
									}
									goto Continue;
								}
							}
						Add:
							ingredientControls.Add(ingredientStep, connection.ItemUID);
						Continue:
							continue;
						}
					}
					StepAndIngredientsLayout layout = new StepAndIngredientsLayout(step, ingredientControls);
					DrawingContext.StepUIMap.Add(step, new Tuple<StepControl, StepAndIngredientsLayout>(stepControl, layout));
				}
			}
			foreach (Step step in DrawingContext.StepUIMap.Keys)
			{
				DrawingContext.StepUIMap[step].Item2.PrePlace();
			}
			foreach (Step step in DrawingContext.StepUIMap.Keys)
			{
				panel.Controls.Add(DrawingContext.StepUIMap[step].Item1);
			}
			foreach (Step step in plan.ProcessedPlan.Get().GetStepsInTier(0))
			{
				StepAndIngredientsLayout layout = DrawingContext.StepUIMap[step].Item2;
				layout.Place(xPosition, yPosition);
				xPosition += layout.PreferredSize.Width;
			}
			DrawingContext = default;
		}

		private class PlanDrawingContext
		{
			public readonly Random Rand = new Random();
			public readonly Dictionary<Step, Tuple<StepControl, StepAndIngredientsLayout>> StepUIMap = new Dictionary<Step, Tuple<StepControl, StepAndIngredientsLayout>>();
			private readonly HashSet<Color> ingredientColors = new HashSet<Color>()
			{
				Color.FromArgb(255, 0, 0),
				Color.FromArgb(0, 255, 0),
				Color.FromArgb(0, 0, 255),
				Color.FromArgb(255, 255, 0)
			};
			private readonly HashSet<Color> usedIngredientColors = new HashSet<Color>();
			private readonly HashSet<Color> alternativeConnectionsColors = new HashSet<Color>()
			{
				Color.FromArgb(0, 255, 127),
				Color.FromArgb(0, 127, 255),
				Color.FromArgb(255, 127, 0)
			};
			private readonly HashSet<Color> usedAlternativeConnectionsColors = new HashSet<Color>();
			private readonly HashSet<char> alternateConnectionLabels = new HashSet<char>()
			{
				'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
			};
			private readonly HashSet<char> usedAlternatedConnectionLabels = new HashSet<char>();

			public char GetAlternateConnectionLabel()
			{
				HashSet<char> availableLabels = new HashSet<char>(alternateConnectionLabels);
				availableLabels.ExceptWith(usedAlternatedConnectionLabels);
				int chosenLabelIndex = Rand.Next(0, availableLabels.Count), counter = 0;
				foreach (char label in availableLabels)
				{
					if (counter++ == chosenLabelIndex)
					{
						usedAlternatedConnectionLabels.Add(label);
						if (usedAlternatedConnectionLabels.Count == alternateConnectionLabels.Count)
						{
							usedAlternatedConnectionLabels.Clear();
						}
						return label;
					}
				}
				throw new InvalidOperationException("This code should never be reached - something has gone wrong");
			}

			public Color GetNewAlternativeConnectionColor()
			{
				HashSet<Color> availableColors = new HashSet<Color>(alternativeConnectionsColors);
				availableColors.ExceptWith(usedAlternativeConnectionsColors);
				int chosenColorIndex = Rand.Next(0, availableColors.Count), counter = 0;
				foreach (Color color in availableColors)
				{
					if (counter++ == chosenColorIndex)
					{
						usedAlternativeConnectionsColors.Add(color);
						if (usedAlternativeConnectionsColors.Count == alternativeConnectionsColors.Count)
						{
							usedAlternativeConnectionsColors.Clear();
						}
						return color;
					}
				}
				throw new InvalidOperationException("This code should never be reached - something has gone wrong");
			}

			public void StartNewIngredientColorGroup()
			{
				usedIngredientColors.Clear();
			}

			public Color GetNewIngredientConnectionColor()
			{
				HashSet<Color> availableColors = new HashSet<Color>(ingredientColors);
				availableColors.ExceptWith(usedIngredientColors);
				int chosenColorIndex = Rand.Next(0, availableColors.Count), counter = 0;
				foreach (Color color in availableColors)
				{
					if (counter++ == chosenColorIndex)
					{
						usedIngredientColors.Add(color);
						return color;
					}
				}
				throw new InvalidOperationException("Ran out of ingredient colors");
			}
		}

		private class StepAndIngredientsLayout : ILayoutControl
		{
			private readonly Step productStep;
			private readonly Dictionary<Step, string> ingredientSteps;
			public StepControl TopControl { get; private set; }
			private readonly Dictionary<ILayoutControl, string> ingredientControls = new Dictionary<ILayoutControl, string>();
			public Size PreferredSize { get; private set; }
			private Size productPreferredSize;
			private bool placed = false;

			public StepAndIngredientsLayout(Step productStep, Dictionary<Step, string> ingredientSteps)
			{
				this.productStep = productStep;
				this.ingredientSteps = ingredientSteps;
			}

			public void PrePlace()
			{
				TopControl = DrawingContext.StepUIMap[productStep].Item1;
				foreach (Step step in ingredientSteps.Keys)
				{
					ingredientControls.Add(DrawingContext.StepUIMap[step].Item2, ingredientSteps[step]);
				}
				int width1 = 0, height = 0;
				foreach (ILayoutControl ingredientControl in ingredientControls.Keys)
				{
					Size preferredSize = ingredientControl.PreferredSize;
					width1 += preferredSize.Width;
					height = preferredSize.Height > height ? preferredSize.Height : height;
				}
				productPreferredSize = TopControl.PreferredSize;
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
					Point productIRCLoc = productIRC.GetTotalLocation();
					productIRCConnectionPoints.Add(item, new Point(productIRCLoc.X + (productIRC.Size.Width / 2), productIRCLoc.Y));
					ItemRateControl ingredientIRC = TopControl.IngredientRateControls[item];
					Point ingredientIRCLoc = ingredientIRC.GetTotalLocation();
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
				DrawingContext.StartNewIngredientColorGroup();
				foreach (ILayoutControl control in orderedIngredients)
				{
					string str = ingredientControls[control];
					LineControl line1 = new LineControl(), line2 = new LineControl(), line3 = new LineControl();
					TopControl.Parent.Controls.AddRange(new Control[] { line1, line2, line3 });
					line1.BringToFront();
					line2.BringToFront();
					line3.BringToFront();
					Color color = DrawingContext.GetNewIngredientConnectionColor();
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
				Dictionary<Step, string> ingredientSteps = new Dictionary<Step, string>();
				foreach (Connection ingredientConnection in TopControl.BackingStep.NormalIngredientConnections.Get())
				{
					if (ingredientConnection.Type.Get() != Connection.ConnectionType.NORMAL)
					{
						continue;
					}
					foreach (Step step in ingredientConnection.GetProducerSteps())
					{
						ingredientSteps.Add(step, ingredientConnection.ItemUID);
					}
				}
				Dictionary<Step, string> alternateConnectionIngredients = new Dictionary<Step, string>();
				foreach (Step ingredientStep in ingredientSteps.Keys)
				{
					foreach (ILayoutControl ingredientLayout in ingredientControls.Keys)
					{
						if (ingredientLayout.TopControl.BackingStep == ingredientStep)
						{
							goto Continue;
						}
					}
					alternateConnectionIngredients.Add(ingredientStep, ingredientSteps[ingredientStep]);
				Continue:
					continue;
				}
				foreach (Step alternateConnectionStep in alternateConnectionIngredients.Keys)
				{
					string str = alternateConnectionIngredients[alternateConnectionStep];
					LineControl line1 = new LineControl(), line2 = new LineControl();
					TopControl.Parent.Controls.AddRange(new Control[] { line1, line2 });
					line1.BringToFront();
					line2.BringToFront();
					char label = DrawingContext.GetAlternateConnectionLabel();
					line1.LineLabel.Text = new string(label, 1);
					line2.LineLabel.Text = line1.LineLabel.Text;
					line1.BackColor = DrawingContext.GetNewAlternativeConnectionColor();
					line2.BackColor = line1.BackColor;
					ItemRateControl ingredientIRC = TopControl.IngredientRateControls[str];
					line1.Location = AddPoints(ingredientIRC.GetTotalLocation(), new Point((ingredientIRC.Size.Width / 2) - line1.Size.Width, ingredientIRC.Size.Height - (line1.Size.Height / 2)));
					ItemRateControl irc = DrawingContext.StepUIMap[alternateConnectionStep].Item1.ProductRateControls[str];
					line2.Location = AddPoints(irc.GetTotalLocation(), new Point((irc.Size.Width / 2) - line2.Size.Width, -line2.Size.Height * 5 / 2));
					line1.Size = new Size(line1.Size.Width * 3, line1.Size.Height * 3);
					line2.Size = line1.Size;
					line1.LineLabel.Location = new Point(0, 0);
					line2.LineLabel.Location = line1.LineLabel.Location;
					line1.LineLabel.Size = line1.Size;
					line2.LineLabel.Size = line2.Size;
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

		public static Point AddParentPoints(Control child, int parentCount)
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
