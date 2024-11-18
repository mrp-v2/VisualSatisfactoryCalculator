using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.controls.user;
using VisualSatisfactoryCalculator.forms;
using VisualSatisfactoryCalculator.model.production;
using VisualSatisfactoryCalculator.satisfactory.model.production;
using VisualSatisfactoryCalculator.satisfactory.Production;

using Connection = VisualSatisfactoryCalculator.model.production.Connection<VisualSatisfactoryCalculator.satisfactory.model.production.Item, VisualSatisfactoryCalculator.satisfactory.Production.Step, VisualSatisfactoryCalculator.satisfactory.model.production.Recipe>;

namespace VisualSatisfactoryCalculator.satisfactory.Utility
{
	public class PlanLayoutMaker
	{
		private static PlanDrawingContext DRAWING_CONTEXT;

		public static void LayoutSteps(MainForm mainForm, Panel panel, Plan plan)
		{
			DRAWING_CONTEXT = new PlanDrawingContext();
			int yPosition = panel.GetPreferredSize(new Size()).Height, xPosition = 0;
			// setup normal connections
			for (int currentTier = plan.processedPlan.Get().Tiers - 1; currentTier >= 0; currentTier--)
			{
				foreach (Step step in plan.processedPlan.Get().GetStepsInTier(currentTier))
				{
					StepControl stepControl = new StepControl(step, mainForm);
					Dictionary<Step, Item> ingredientControls = new Dictionary<Step, Item>();
					foreach (Connection connection in step.GetIngredientConnections())
					{
						if (connection.Type == ConnectionType.SINGLE)
						{
							foreach (Step ingredientStep in connection.ProducerSteps)
							{
								if (ingredientStep.recipe.products.Count > 1)
								{
									foreach (Item item in ingredientStep.recipe.products.Keys)
									{
										if (connection.item == item)
										{
											goto Add;
										}
										else if (!ingredientStep.HasProductConnectionFor(item))
										{
											continue;
										}
										goto Continue;
									}
								}
							Add:
								ingredientControls.Add(ingredientStep, connection.item);
							Continue:
								continue;
							}
						}
					}
					StepAndIngredientsLayout layout = new StepAndIngredientsLayout(step, ingredientControls);
					DRAWING_CONTEXT.stepUIMap.Add(step, new Tuple<StepControl, StepAndIngredientsLayout>(stepControl, layout));
				}
			}
			// start placing things
			foreach (Step step in plan.processedPlan.Get().GetStepsInTier(0))
			{
				DRAWING_CONTEXT.stepUIMap[step].Item2.PrePlace();
			}
			foreach (Step step in DRAWING_CONTEXT.stepUIMap.Keys)
			{
				panel.Controls.Add(DRAWING_CONTEXT.stepUIMap[step].Item1);
			}
			foreach (Step step in plan.processedPlan.Get().GetStepsInTier(0))
			{
				StepAndIngredientsLayout layout = DRAWING_CONTEXT.stepUIMap[step].Item2;
				layout.Place(xPosition, yPosition);
				xPosition += layout.PreferredSize.Width;
			}
			foreach (Connection connection in plan.processedPlan.Get().GetMultiConnections())
			{
				foreach (Step step in connection.ProducerSteps)
				{
					ItemRateControl control = DRAWING_CONTEXT.stepUIMap[step].Item1.productRateControls[connection.item];
					DRAWING_CONTEXT.AddAlternateConnection(connection, control);
				}
				foreach (Step step in connection.ConsumerSteps)
				{
					ItemRateControl control = DRAWING_CONTEXT.stepUIMap[step].Item1.ingredientRateControls[connection.item];
					DRAWING_CONTEXT.AddAlternateConnection(connection, control);
				}
			}
			PlaceAlternateConnections(panel);
			DRAWING_CONTEXT = default;
		}

		private static void PlaceAlternateConnections(Panel panel)
		{
			foreach (HashSet<ItemRateControl> controls in DRAWING_CONTEXT.GetAlternateConnectionGroups())
			{
				string label = new string(DRAWING_CONTEXT.GetAlternateConnectionLabel(), 1);
				Color color = DRAWING_CONTEXT.GetNewAlternativeConnectionColor();
				foreach (ItemRateControl control in controls)
				{
					LineControl line = new LineControl();
					panel.Controls.Add(line);
					line.BringToFront();
					line.LineLabel.Text = label;
					line.BackColor = color;
					if (control.IsProduct)
					{
						line.Location = AddPoints(control.GetTotalLocation(), new Point((control.Size.Width / 2) - line.Size.Width, -line.Size.Height * 5 / 2));
					}
					else
					{
						line.Location = AddPoints(control.GetTotalLocation(), new Point((control.Size.Width / 2) - line.Size.Width, control.Size.Height - (line.Size.Height / 2)));
					}
					line.Size = new Size(line.Size.Width * 3, line.Size.Height * 3);
					line.LineLabel.Location = new Point(0, 0);
					line.LineLabel.Size = line.Size;
				}
			}
		}

		private class PlanDrawingContext
		{
			public readonly Random rand = new Random();
			public readonly Dictionary<Step, Tuple<StepControl, StepAndIngredientsLayout>> stepUIMap = new Dictionary<Step, Tuple<StepControl, StepAndIngredientsLayout>>();
			private readonly Dictionary<Connection, HashSet<ItemRateControl>> _scheduledAlternateConnections = new Dictionary<Connection, HashSet<ItemRateControl>>();
			private readonly HashSet<Color> _ingredientColors = new HashSet<Color>()
			{
				Color.FromArgb(255, 0, 0),
				Color.FromArgb(0, 255, 0),
				Color.FromArgb(0, 0, 255),
				Color.FromArgb(255, 255, 0)
			};
			private readonly HashSet<Color> _usedIngredientColors = new HashSet<Color>();
			private readonly HashSet<Color> _alternativeConnectionsColors = new HashSet<Color>()
			{
				Color.FromArgb(0, 255, 127),
				Color.FromArgb(0, 127, 255),
				Color.FromArgb(255, 127, 0)
			};
			private readonly HashSet<Color> _usedAlternativeConnectionsColors = new HashSet<Color>();
			private readonly HashSet<char> _alternateConnectionLabels = new HashSet<char>()
			{
				'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
			};
			private readonly HashSet<char> _usedAlternatedConnectionLabels = new HashSet<char>();

			public void AddAlternateConnection(Connection connection, ItemRateControl control)
			{
				if (_scheduledAlternateConnections.TryGetValue(connection, out HashSet<ItemRateControl> controls))
				{
					controls.Add(control);
				}
				else
				{
					_scheduledAlternateConnections.Add(connection, new HashSet<ItemRateControl> { control });
				}
			}

			public IEnumerable<HashSet<ItemRateControl>> GetAlternateConnectionGroups()
			{
				return _scheduledAlternateConnections.Values;
			}

			public char GetAlternateConnectionLabel()
			{
				HashSet<char> availableLabels = new HashSet<char>(_alternateConnectionLabels);
				availableLabels.ExceptWith(_usedAlternatedConnectionLabels);
				int chosenLabelIndex = rand.Next(0, availableLabels.Count), counter = 0;
				foreach (char label in availableLabels)
				{
					if (counter++ == chosenLabelIndex)
					{
						_usedAlternatedConnectionLabels.Add(label);
						if (_usedAlternatedConnectionLabels.Count == _alternateConnectionLabels.Count)
						{
							_usedAlternatedConnectionLabels.Clear();
						}
						return label;
					}
				}
				throw new InvalidOperationException("This code should never be reached - something has gone wrong");
			}

			public Color GetNewAlternativeConnectionColor()
			{
				HashSet<Color> availableColors = new HashSet<Color>(_alternativeConnectionsColors);
				availableColors.ExceptWith(_usedAlternativeConnectionsColors);
				int chosenColorIndex = rand.Next(0, availableColors.Count), counter = 0;
				foreach (Color color in availableColors)
				{
					if (counter++ == chosenColorIndex)
					{
						_usedAlternativeConnectionsColors.Add(color);
						if (_usedAlternativeConnectionsColors.Count == _alternativeConnectionsColors.Count)
						{
							_usedAlternativeConnectionsColors.Clear();
						}
						return color;
					}
				}
				throw new InvalidOperationException("This code should never be reached - something has gone wrong");
			}

			public void StartNewIngredientColorGroup()
			{
				_usedIngredientColors.Clear();
			}

			public Color GetNewIngredientConnectionColor()
			{
				HashSet<Color> availableColors = new HashSet<Color>(_ingredientColors);
				availableColors.ExceptWith(_usedIngredientColors);
				int chosenColorIndex = rand.Next(0, availableColors.Count), counter = 0;
				foreach (Color color in availableColors)
				{
					if (counter++ == chosenColorIndex)
					{
						_usedIngredientColors.Add(color);
						return color;
					}
				}
				throw new InvalidOperationException("Ran out of ingredient colors");
			}
		}

		private class StepAndIngredientsLayout : ILayoutControl
		{
			private readonly Step _step;
			private readonly Dictionary<Step, Item> _ingredientSteps;
			public StepControl Control { get; protected set; }
			private readonly Dictionary<ILayoutControl, Item> _ingredientControls = new Dictionary<ILayoutControl, Item>();
			public Size PreferredSize { get; protected set; }
			protected Size size;
			protected bool placed = false;

			public StepAndIngredientsLayout(Step productStep, Dictionary<Step, Item> ingredientSteps)
			{
				_step = productStep;
				_ingredientSteps = ingredientSteps;
			}

			public virtual void PrePlace()
			{
				Control = DRAWING_CONTEXT.stepUIMap[_step].Item1;
				foreach (Step step in _ingredientSteps.Keys)
				{
					_ingredientControls.Add(DRAWING_CONTEXT.stepUIMap[step].Item2, _ingredientSteps[step]);
				}
				int width1 = 0, height = 0;
				foreach (ILayoutControl ingredientControl in _ingredientControls.Keys)
				{
					if (ingredientControl is StepAndIngredientsLayout layout)
					{
						layout.PrePlace();
					}
					Size size = ingredientControl.PreferredSize;
					width1 += size.Width;
					if (size.Height > height)
					{
						height = size.Height;
					}
				}
				size = Control.PreferredSize;
				int width2 = size.Width;
				int width3 = width1 > width2 ? width1 : width2;
				height += size.Height;
				PreferredSize = new Size(width3, height);
			}

			public virtual void Place(int xStart, int yStart)
			{
				if (placed)
				{
					throw new InvalidOperationException("Can't place a layout piece that has already been placed");
				}
				Control.Place(xStart + (PreferredSize.Width / 2) - (size.Width / 2), yStart);
				int currentX = xStart, y = yStart + size.Height;
				Dictionary<Item, ILayoutControl> reverseIngredientControls = new Dictionary<Item, ILayoutControl>();
				foreach (ILayoutControl control in _ingredientControls.Keys)
				{
					reverseIngredientControls.Add(_ingredientControls[control], control);
				}
				List<ILayoutControl> orderedIngredients = new List<ILayoutControl>();
				foreach (Item item in Control.backingStep.recipe.ingredients.Keys)
				{
					if (reverseIngredientControls.ContainsKey(item))
					{
						orderedIngredients.Add(reverseIngredientControls[item]);
					}
				}
				if (PreferredSize.Width == size.Width)
				{
					int ingredientsWidth = 0;
					foreach (ILayoutControl control in _ingredientControls.Keys)
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
				Dictionary<Item, Point> ingredientIRCConnectionPoints = new Dictionary<Item, Point>();
				Dictionary<Item, Point> productIRCConnectionPoints = new Dictionary<Item, Point>();
				foreach (ILayoutControl control in orderedIngredients)
				{
					Item item = _ingredientControls[control];
					ItemRateControl productIRC = control.Control.productRateControls[item];
					Point productIRCLoc = productIRC.GetTotalLocation();
					productIRCConnectionPoints.Add(item, new Point(productIRCLoc.X + (productIRC.Size.Width / 2), productIRCLoc.Y));
					ItemRateControl ingredientIRC = Control.ingredientRateControls[item];
					Point ingredientIRCLoc = ingredientIRC.GetTotalLocation();
					ingredientIRCConnectionPoints.Add(item, new Point(ingredientIRCLoc.X + (ingredientIRC.Size.Width / 2), ingredientIRCLoc.Y + ingredientIRC.Size.Height));
				}
				Dictionary<Item, Range> itemLineRanges = new Dictionary<Item, Range>();
				foreach (ILayoutControl control in orderedIngredients)
				{
					Item item = _ingredientControls[control];
					int left1 = ingredientIRCConnectionPoints[item].X, left2 = productIRCConnectionPoints[item].X;
					int left3 = left1 < left2 ? left1 : left2;
					int right1 = ingredientIRCConnectionPoints[item].X, right2 = productIRCConnectionPoints[item].X;
					int right3 = right1 > right2 ? right1 : right2;
					itemLineRanges.Add(item, new Range(left3, right3));
				}
				DRAWING_CONTEXT.StartNewIngredientColorGroup();
				foreach (ILayoutControl control in orderedIngredients)
				{
					Item item = _ingredientControls[control];
					LineControl line1 = new LineControl(), line2 = new LineControl(), line3 = new LineControl();
					Control.Parent.Controls.AddRange(new Control[] { line1, line2, line3 });
					line1.BringToFront();
					line2.BringToFront();
					line3.BringToFront();
					Color color = DRAWING_CONTEXT.GetNewIngredientConnectionColor();
					line1.BackColor = color;
					line2.BackColor = color;
					line3.BackColor = color;
					line1.Location = AddPoints(ingredientIRCConnectionPoints[item], new Point(-line1.Size.Width / 2, -line1.Size.Height / 2));
					line3.Location = AddPoints(productIRCConnectionPoints[item], new Point(-line3.Size.Width / 2, -line3.Size.Height / 2));
					int height = (line1.Location.Y + line3.Location.Y) / 2;
					line2.Location = new Point(itemLineRanges[item].left - (line2.Size.Width / 2), (line1.Location.Y + line3.Location.Y) / 2);
					line2.Size = new Size(itemLineRanges[item].length + line2.Size.Width, line2.Size.Height);
					line1.Size = new Size(line1.Size.Width, height - line1.Location.Y);
					line3.Size = new Size(line3.Size.Width, line3.Location.Y - height + line3.Size.Height);
					line3.Location = AddPoints(line3.Location, new Point(0, height - line3.Location.Y));
				}
				Dictionary<Step, Item> ingredientSteps = new Dictionary<Step, Item>();
				foreach (Connection ingredientConnection in Control.backingStep.normalIngredientConnections.Get())
				{
					foreach (Step step in ingredientConnection.ProducerSteps)
					{
						ingredientSteps.Add(step, ingredientConnection.item);
					}
				}
				Dictionary<Step, Item> alternateConnectionIngredients = new Dictionary<Step, Item>();
				foreach (Step ingredientStep in ingredientSteps.Keys)
				{
					foreach (ILayoutControl ingredientLayout in _ingredientControls.Keys)
					{
						if (ingredientLayout.Control.backingStep == ingredientStep)
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
					Item item = alternateConnectionIngredients[alternateConnectionStep];
					ItemRateControl ingredientIRC = Control.ingredientRateControls[item];
					ItemRateControl productIRC = DRAWING_CONTEXT.stepUIMap[alternateConnectionStep].Item1.productRateControls[item];
					Connection connection = _step.GetIngredientConnection(item);
					DRAWING_CONTEXT.AddAlternateConnection(connection, ingredientIRC);
					DRAWING_CONTEXT.AddAlternateConnection(connection, productIRC);
				}
				placed = true;
			}
		}

		private readonly struct Range
		{
			public readonly int left, right;
			public readonly int length;

			public Range(int left, int right)
			{
				if (left > right)
				{
					(left, right) = (right, left);
				}
				this.left = left;
				this.right = right;
				length = this.right - this.left;
			}

			public readonly bool Overlaps(Range other)
			{
				return !(right < other.left || left > other.right);
			}

			public static int MaxOverlaps(IEnumerable<Range> ranges)
			{
				int maxOverlaps = 0;
				Range totalRange = Total(ranges);
				for (int i = totalRange.left; i <= totalRange.right; i++)
				{
					int overlaps = OverlapsAt(ranges, i);
					if (overlaps > maxOverlaps)
					{
						maxOverlaps = overlaps;
					}
				}
				return maxOverlaps;
			}

			public readonly bool Includes(int position)
			{
				return position > left && position < right;
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
					if (range.left < left)
					{
						left = range.left;
					}
					if (range.right > right)
					{
						right = range.right;
					}
				}
				return new Range(left, right);
			}
		}

		public interface ILayoutControl
		{
			Size PreferredSize { get; }
			void Place(int xStart, int yStart);
			StepControl Control { get; }
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
