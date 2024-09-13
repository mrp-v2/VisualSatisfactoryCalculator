using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.satisfactory.Production;
using VisualSatisfactoryCalculator.controls.user;
using VisualSatisfactoryCalculator.forms;

using Connection = VisualSatisfactoryCalculator.model.production.Connection<VisualSatisfactoryCalculator.satisfactory.JSONClasses.JSONItem, VisualSatisfactoryCalculator.satisfactory.Production.Step, VisualSatisfactoryCalculator.satisfactory.DataStorage.BasicRecipe>;
using VisualSatisfactoryCalculator.satisfactory.JSONClasses;

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
					Dictionary<Step, JSONItem> ingredientControls = new Dictionary<Step, JSONItem>();
					Dictionary<JSONItem, Connection> abnormalConnectionIngredients = new Dictionary<JSONItem, Connection>();
					foreach (Connection connection in step.GetIngredientConnections())
					{
						if (connection.Type == model.production.ConnectionType.SINGLE)
						{
							foreach (Step ingredientStep in connection.ProducerSteps)
							{
								if (ingredientStep.recipe.products.Count > 1)
								{
									foreach (JSONItem item in ingredientStep.recipe.products.Keys)
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
						if (connection.Type == model.production.ConnectionType.MULTI && connection.ConsumerSteps.Count() == 1)
						{
							abnormalConnectionIngredients.Add(connection.item, connection);
						}
					}
					StepAndIngredientsLayout layout = new StepAndIngredientsLayout(step, ingredientControls, abnormalConnectionIngredients);
					DRAWING_CONTEXT.stepUIMap.Add(step, new Tuple<StepControl, StepAndIngredientsLayout>(stepControl, layout));
				}
			}
			// setup abnormal connections
			foreach (Connection connection in plan.processedPlan.Get().GetAbnormalConnections())
			{
				throw new NotImplementedException();
				//DRAWING_CONTEXT.abnormalConnectionUIMap.Add(connection, new Tuple<SplitAndMergeControl, SplitAndMergeLayout>(new SplitAndMergeControl(connection, mainForm), new SplitAndMergeLayout(connection)));
				//DRAWING_CONTEXT.abnormalConnectionsRequiringIndependentDrawing.Add(connection);

			}
			// start placing things
			foreach (Step step in plan.processedPlan.Get().GetStepsInTier(0))
			{
				DRAWING_CONTEXT.stepUIMap[step].Item2.PrePlace();
			}
			foreach (Connection connection in DRAWING_CONTEXT.abnormalConnectionsRequiringIndependentDrawing)
			{
				//DRAWING_CONTEXT.abnormalConnectionUIMap[connection].Item2.PrePlace();
			}
			foreach (Step step in DRAWING_CONTEXT.stepUIMap.Keys)
			{
				panel.Controls.Add(DRAWING_CONTEXT.stepUIMap[step].Item1);
			}
			//foreach (Connection connection in DRAWING_CONTEXT.abnormalConnectionUIMap.Keys)
			//{
			//	panel.Controls.Add(DRAWING_CONTEXT.abnormalConnectionUIMap[connection].Item1);
			//}
			foreach (Step step in plan.processedPlan.Get().GetStepsInTier(0))
			{
				StepAndIngredientsLayout layout = DRAWING_CONTEXT.stepUIMap[step].Item2;
				layout.Place(xPosition, yPosition);
				xPosition += layout.PreferredSize.Width;
			}
			foreach (Connection connection in DRAWING_CONTEXT.abnormalConnectionsRequiringIndependentDrawing)
			{
				//SplitAndMergeLayout layout = DRAWING_CONTEXT.abnormalConnectionUIMap[connection].Item2;
				//layout.Place(xPosition, yPosition);
				//xPosition += layout.PreferredSize.Width;
			}
			PlaceAlternateConnections(panel);
			DRAWING_CONTEXT = default;
		}

		private static void PlaceAlternateConnections(Panel panel)
		{
			foreach (Tuple<ItemRateControl, ItemRateControl> controlPair in DRAWING_CONTEXT.scheduledAlternateConnections)
			{
				ItemRateControl controlA = controlPair.Item1, controlB = controlPair.Item2;
				LineControl lineA = new LineControl(), lineB = new LineControl();
				panel.Controls.AddRange(new Control[] { lineA, lineB });
				lineA.BringToFront();
				lineB.BringToFront();
				lineA.LineLabel.Text = new string(DRAWING_CONTEXT.GetAlternateConnectionLabel(), 1);
				lineB.LineLabel.Text = lineA.LineLabel.Text;
				lineA.BackColor = DRAWING_CONTEXT.GetNewAlternativeConnectionColor();
				lineB.BackColor = lineA.BackColor;
				if (controlA.IsProduct)
				{
					lineA.Location = AddPoints(controlA.GetTotalLocation(), new Point((controlA.Size.Width / 2) - lineA.Size.Width, -lineA.Size.Height * 5 / 2));
				}
				else
				{
					lineA.Location = AddPoints(controlA.GetTotalLocation(), new Point((controlA.Size.Width / 2) - lineA.Size.Width, controlA.Size.Height - (lineA.Size.Height / 2)));
				}
				if (controlB.IsProduct)
				{
					lineB.Location = AddPoints(controlB.GetTotalLocation(), new Point((controlB.Size.Width / 2) - lineB.Size.Width, -lineB.Size.Height * 5 / 2));
				}
				else
				{
					lineB.Location = AddPoints(controlB.GetTotalLocation(), new Point((controlB.Size.Width / 2) - lineB.Size.Width, controlB.Size.Height - (lineB.Size.Height / 2)));
				}
				lineA.Size = new Size(lineA.Size.Width * 3, lineA.Size.Height * 3);
				lineB.Size = lineA.Size;
				lineA.LineLabel.Location = new Point(0, 0);
				lineB.LineLabel.Location = lineA.LineLabel.Location;
				lineA.LineLabel.Size = lineA.Size;
				lineB.LineLabel.Size = lineB.Size;
			}
		}

		private class PlanDrawingContext
		{
			public readonly Random rand = new Random();
			public readonly Dictionary<Step, Tuple<StepControl, StepAndIngredientsLayout>> stepUIMap = new Dictionary<Step, Tuple<StepControl, StepAndIngredientsLayout>>();
			public readonly HashSet<Tuple<ItemRateControl, ItemRateControl>> scheduledAlternateConnections = new HashSet<Tuple<ItemRateControl, ItemRateControl>>();
			//public readonly Dictionary<Connection, Tuple<SplitAndMergeControl, SplitAndMergeLayout>> abnormalConnectionUIMap = new Dictionary<Connection, Tuple<SplitAndMergeControl, SplitAndMergeLayout>>();
			public readonly List<Connection> abnormalConnectionsRequiringIndependentDrawing = new List<Connection>();
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

		private class SplitAndMergeLayout : StepAndIngredientsLayout
		{
			private readonly Connection _connection;
			//new public SplitAndMergeControl TopControl { get; private set; }
			private readonly List<Step> _inputs = new List<Step>();
			private readonly Dictionary<Step, ILayoutControl> _inputControls = new Dictionary<Step, ILayoutControl>();

			public SplitAndMergeLayout(Connection connection) : base(null, null, null)
			{
				_connection = connection;
				foreach (Step step in connection.ProducerSteps)
				{
					if (step.recipe.products.Count > 1)
					{
						foreach (JSONItem str in step.recipe.products.Keys)
						{
							if (str == connection.item)
							{
								goto Add;
							}
							else if (!step.HasProductConnectionFor(str))
							{
								continue;
							}
							goto Continue;
						}
					}
				Add:
					_inputs.Add(step);
				Continue:
					continue;
				}
			}

			public override void PrePlace()
			{
				//TopControl = DRAWING_CONTEXT.abnormalConnectionUIMap[_connection].Item1;
				foreach (Step step in _inputs)
				{
					_inputControls.Add(step, DRAWING_CONTEXT.stepUIMap[step].Item2);
				}
				int width1 = 0, height = 0;
				foreach (ILayoutControl inputControl in _inputControls.Values)
				{
					if (inputControl is StepAndIngredientsLayout layout)
					{
						layout.PrePlace();
					}
					Size size = inputControl.PreferredSize;
					width1 += size.Width;
					if (size.Height > height)
					{
						height = size.Height;
					}
				}
				topSize = TopControl.PreferredSize;
				int width2 = topSize.Width;
				int width3 = width1 > width2 ? width1 : width2;
				height += topSize.Height;
				PreferredSize = new Size(width3, height);
			}

			public override void Place(int xStart, int yStart)
			{
				if (placed)
				{
					throw new InvalidOperationException("Can't place a layout piece that has already been placed");
				}
				TopControl.Place(xStart + (PreferredSize.Width / 2) - (topSize.Width / 2), yStart);
				int currentX = xStart, y = yStart + topSize.Height;
				y += 10;
				foreach (ILayoutControl control in _inputControls.Values)
				{
					control.Place(currentX, y);
					currentX += control.PreferredSize.Width;
				}
				Dictionary<Step, Point> ingredientIRCConnectionPoints = new Dictionary<Step, Point>();
				Dictionary<Step, Point> productIRCConnectionPoints = new Dictionary<Step, Point>();
				foreach (Step step in _inputControls.Keys)
				{
					ILayoutControl control = _inputControls[step];
					ItemRateControl productIRC = control.TopControl.productRateControls[_connection.item];
					Point productIRCLoc = productIRC.GetTotalLocation();
					productIRCConnectionPoints.Add(step, new Point(productIRCLoc.X + (productIRC.Size.Width / 2), productIRCLoc.Y));
					//ItemRateControl ingredientIRC = TopControl.inControls[step];
					//Point ingredientIRCLoc = ingredientIRC.GetTotalLocation();
					//ingredientIRCConnectionPoints.Add(step, new Point(ingredientIRCLoc.X + (ingredientIRC.Size.Width / 2), ingredientIRCLoc.Y + ingredientIRC.Size.Height));
				}
				Dictionary<Step, Range> itemLineRanges = new Dictionary<Step, Range>();
				foreach (Step step in _inputControls.Keys)
				{
					int left1 = ingredientIRCConnectionPoints[step].X, left2 = productIRCConnectionPoints[step].X;
					int left3 = left1 < left2 ? left1 : left2;
					int right1 = ingredientIRCConnectionPoints[step].X, right2 = productIRCConnectionPoints[step].X;
					int right3 = right1 > right2 ? right1 : right2;
					itemLineRanges.Add(step, new Range(left3, right3));
				}
				DRAWING_CONTEXT.StartNewIngredientColorGroup();
				foreach (Step step in _inputControls.Keys)
				{
					LineControl line1 = new LineControl(), line2 = new LineControl(), line3 = new LineControl();
					TopControl.Parent.Controls.AddRange(new Control[] { line1, line2, line3 });
					line1.BringToFront();
					line2.BringToFront();
					line3.BringToFront();
					Color color = DRAWING_CONTEXT.GetNewIngredientConnectionColor();
					line1.BackColor = color;
					line2.BackColor = color;
					line3.BackColor = color;
					line1.Location = AddPoints(ingredientIRCConnectionPoints[step], new Point(-line1.Size.Width / 2, -line1.Size.Height / 2));
					line3.Location = AddPoints(productIRCConnectionPoints[step], new Point(-line3.Size.Width / 2, -line3.Size.Height / 2));
					int height = (line1.Location.Y + line3.Location.Y) / 2;
					line2.Location = new Point(itemLineRanges[step].left - (line2.Size.Width / 2), (line1.Location.Y + line3.Location.Y) / 2);
					line2.Size = new Size(itemLineRanges[step].length + line2.Size.Width, line2.Size.Height);
					line1.Size = new Size(line1.Size.Width, height - line1.Location.Y);
					line3.Size = new Size(line3.Size.Width, line3.Location.Y - height + line3.Size.Height);
					line3.Location = AddPoints(line3.Location, new Point(0, height - line3.Location.Y));
				}
				if (_connection.ConsumerSteps.Count() > 1)
				{
					foreach (Step step in _connection.ConsumerSteps)
					{
						ItemRateControl a = DRAWING_CONTEXT.stepUIMap[step].Item1.ingredientRateControls[_connection.item];
						//ItemRateControl b = DRAWING_CONTEXT.abnormalConnectionUIMap[_connection].Item1.outControls[step];
						//DRAWING_CONTEXT.scheduledAlternateConnections.Add(new Tuple<ItemRateControl, ItemRateControl>(a, b));
					}
				}
				if (_connection.ProducerSteps.Count() > _inputs.Count)
				{
					foreach (Step step in _connection.ProducerSteps)
					{
						if (_inputs.Contains(step))
						{
							continue;
						}
						ItemRateControl a = DRAWING_CONTEXT.stepUIMap[step].Item1.productRateControls[_connection.item];
						//ItemRateControl b = DRAWING_CONTEXT.abnormalConnectionUIMap[_connection].Item1.inControls[step];
						//DRAWING_CONTEXT.scheduledAlternateConnections.Add(new Tuple<ItemRateControl, ItemRateControl>(a, b));
					}
				}
				placed = true;
			}
		}

		private class StepAndIngredientsLayout : ILayoutControl
		{
			private readonly Step _productStep;
			private readonly Dictionary<Step, JSONItem> _ingredientSteps;
			private readonly Dictionary<JSONItem, Connection> _abnormalConnectionIngredients;
			public StepControl TopControl { get; protected set; }
			private readonly Dictionary<ILayoutControl, JSONItem> _ingredientControls = new Dictionary<ILayoutControl, JSONItem>();
			public Size PreferredSize { get; protected set; }
			protected Size topSize;
			protected bool placed = false;

			public StepAndIngredientsLayout(Step productStep, Dictionary<Step, JSONItem> ingredientSteps, Dictionary<JSONItem, Connection> abnormalConnectionIngredients)
			{
				_productStep = productStep;
				_ingredientSteps = ingredientSteps;
				_abnormalConnectionIngredients = abnormalConnectionIngredients;
			}

			public virtual void PrePlace()
			{
				TopControl = DRAWING_CONTEXT.stepUIMap[_productStep].Item1;
				foreach (Step step in _ingredientSteps.Keys)
				{
					_ingredientControls.Add(DRAWING_CONTEXT.stepUIMap[step].Item2, _ingredientSteps[step]);
				}
				foreach (JSONItem item in _abnormalConnectionIngredients.Keys)
				{
					Connection connection = _abnormalConnectionIngredients[item];
					//_ingredientControls.Add(DRAWING_CONTEXT.abnormalConnectionUIMap[connection].Item2, item);
					DRAWING_CONTEXT.abnormalConnectionsRequiringIndependentDrawing.Remove(connection);
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
				topSize = TopControl.PreferredSize;
				int width2 = topSize.Width;
				int width3 = width1 > width2 ? width1 : width2;
				height += topSize.Height;
				PreferredSize = new Size(width3, height);
			}

			public virtual void Place(int xStart, int yStart)
			{
				if (placed)
				{
					throw new InvalidOperationException("Can't place a layout piece that has already been placed");
				}
				TopControl.Place(xStart + (PreferredSize.Width / 2) - (topSize.Width / 2), yStart);
				int currentX = xStart, y = yStart + topSize.Height;
				Dictionary<JSONItem, ILayoutControl> reverseIngredientControls = new Dictionary<JSONItem, ILayoutControl>();
				foreach (ILayoutControl control in _ingredientControls.Keys)
				{
					reverseIngredientControls.Add(_ingredientControls[control], control);
				}
				List<ILayoutControl> orderedIngredients = new List<ILayoutControl>();
				foreach (JSONItem item in TopControl.backingStep.recipe.ingredients.Keys)
				{
					if (reverseIngredientControls.ContainsKey(item))
					{
						orderedIngredients.Add(reverseIngredientControls[item]);
					}
				}
				if (PreferredSize.Width == topSize.Width)
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
				Dictionary<JSONItem, Point> ingredientIRCConnectionPoints = new Dictionary<JSONItem, Point>();
				Dictionary<JSONItem, Point> productIRCConnectionPoints = new Dictionary<JSONItem, Point>();
				foreach (ILayoutControl control in orderedIngredients)
				{
					JSONItem item = _ingredientControls[control];
					//ItemRateControl productIRC = (!(control is SplitAndMergeLayout)) ? control.TopControl.productRateControls[item] : (control as SplitAndMergeLayout).TopControl.outControls.Values.First();
					ItemRateControl productIRC = control.TopControl.productRateControls[item];
					Point productIRCLoc = productIRC.GetTotalLocation();
					productIRCConnectionPoints.Add(item, new Point(productIRCLoc.X + (productIRC.Size.Width / 2), productIRCLoc.Y));
					ItemRateControl ingredientIRC = TopControl.ingredientRateControls[item];
					Point ingredientIRCLoc = ingredientIRC.GetTotalLocation();
					ingredientIRCConnectionPoints.Add(item, new Point(ingredientIRCLoc.X + (ingredientIRC.Size.Width / 2), ingredientIRCLoc.Y + ingredientIRC.Size.Height));
				}
				Dictionary<JSONItem, Range> itemLineRanges = new Dictionary<JSONItem, Range>();
				foreach (ILayoutControl control in orderedIngredients)
				{
					JSONItem item = _ingredientControls[control];
					int left1 = ingredientIRCConnectionPoints[item].X, left2 = productIRCConnectionPoints[item].X;
					int left3 = left1 < left2 ? left1 : left2;
					int right1 = ingredientIRCConnectionPoints[item].X, right2 = productIRCConnectionPoints[item].X;
					int right3 = right1 > right2 ? right1 : right2;
					itemLineRanges.Add(item, new Range(left3, right3));
				}
				DRAWING_CONTEXT.StartNewIngredientColorGroup();
				foreach (ILayoutControl control in orderedIngredients)
				{
					JSONItem item = _ingredientControls[control];
					LineControl line1 = new LineControl(), line2 = new LineControl(), line3 = new LineControl();
					TopControl.Parent.Controls.AddRange(new Control[] { line1, line2, line3 });
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
				Dictionary<Step, JSONItem> ingredientSteps = new Dictionary<Step, JSONItem>();
				foreach (Connection ingredientConnection in TopControl.backingStep.normalIngredientConnections.Get())
				{
					foreach (Step step in ingredientConnection.ProducerSteps)
					{
						ingredientSteps.Add(step, ingredientConnection.item);
					}
				}
				Dictionary<Step, JSONItem> alternateConnectionIngredients = new Dictionary<Step, JSONItem>();
				foreach (Step ingredientStep in ingredientSteps.Keys)
				{
					foreach (ILayoutControl ingredientLayout in _ingredientControls.Keys)
					{
						if (ingredientLayout.TopControl.backingStep == ingredientStep)
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
					JSONItem item = alternateConnectionIngredients[alternateConnectionStep];
					ItemRateControl ingredientIRC = TopControl.ingredientRateControls[item];
					ItemRateControl productIRC = DRAWING_CONTEXT.stepUIMap[alternateConnectionStep].Item1.productRateControls[item];
					DRAWING_CONTEXT.scheduledAlternateConnections.Add(new Tuple<ItemRateControl, ItemRateControl>(ingredientIRC, productIRC));
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
