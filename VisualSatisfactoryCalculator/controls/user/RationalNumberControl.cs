using System;
using System.Linq;
using System.Windows.Forms;

using VisualSatisfactoryCalculator.satisfactory.Numbers;

namespace VisualSatisfactoryCalculator.controls.user
{
	public partial class RationalNumberControl : UserControl
	{
		public delegate void NumberChanged(RationalNumber oldValue, RationalNumber newValue);

		private NumberChanged numberChanged;
		private string oldValue;

		public void AddNumberChangedListener(NumberChanged numberChanged)
		{
			this.numberChanged += numberChanged;
		}

		public RationalNumberControl()
		{
			InitializeComponent();
			oldValue = NumberTextBox.Text;
		}

		public RationalNumberControl(bool enabled) : this()
		{
			NumberTextBox.Enabled = enabled;
		}

		private void ValueChanged()
		{
			if (NumberTextBox.Text != oldValue)
			{
				oldValue = NumberTextBox.Text;
				numberChanged(GetNumber(oldValue), GetNumber());
			}
		}

		private void UpdateAlternateLabel()
		{
			if (NumberTextBox.Text.Contains('/'))
			{
				if (NumberTextBox.Text.Substring(NumberTextBox.Text.IndexOf('/') + 1).Length > 0)
				{
					if (NumberTextBox.Text.Substring(0, NumberTextBox.Text.IndexOf('/')).Length > 0)
					{
						RationalNumber number = GetNumber();
						if (number.GetDenominator() != 1)
						{
							AlternateNumberLabel.Text = number.ToDouble().ToString();
							return;
						}
					}
				}
			}
			AlternateNumberLabel.Text = "";
		}

		private RationalNumber cachedRationalNumber;
		private bool cachedRationalNumberIsValid = false;

		private RationalNumber GetNumber(string source)
		{
			if (source.Contains('/'))
			{
				string numerator, denominator;
				int divideIndex = source.IndexOf('/');
				numerator = source.Substring(0, divideIndex);
				denominator = source.Substring(divideIndex + 1);
				return new RationalNumber(int.Parse(numerator), int.Parse(denominator), true);
			}
			else
			{
				return new RationalNumber(int.Parse(source), 1, true);
			}
		}

		public RationalNumber GetNumber()
		{
			if (cachedRationalNumberIsValid)
			{
				return cachedRationalNumber;
			}
			cachedRationalNumber = GetNumber(NumberTextBox.Text);
			cachedRationalNumberIsValid = true;
			return cachedRationalNumber;
		}

		public void SetNumber(RationalNumber number)
		{
			NumberTextBox.Text = number.ToString().Replace(" ", "");
			oldValue = NumberTextBox.Text;
			cachedRationalNumber = number;
			cachedRationalNumberIsValid = true;
		}

		private void NumberTextBox_LostFocus(object sender, EventArgs e)
		{
			if (NumberTextBox.Text != oldValue)
			{
				ValueChanged();
			}
		}

		private void NumberTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.D0:
				case Keys.D1:
				case Keys.D2:
				case Keys.D3:
				case Keys.D4:
				case Keys.D5:
				case Keys.D6:
				case Keys.D7:
				case Keys.D8:
				case Keys.D9:
				case Keys.NumPad0:
				case Keys.NumPad1:
				case Keys.NumPad2:
				case Keys.NumPad3:
				case Keys.NumPad4:
				case Keys.NumPad5:
				case Keys.NumPad6:
				case Keys.NumPad7:
				case Keys.NumPad8:
				case Keys.NumPad9:
				case Keys.Left:
				case Keys.Right:
				case Keys.Back:
					e.SuppressKeyPress = false;
					e.Handled = false;
					break;
				case Keys.Divide:
				case Keys.Oem2:
					if (NumberTextBox.Text.Contains('/') || NumberTextBox.SelectionStart == 0)
					{
						goto default;
					}
					else
					{
						e.SuppressKeyPress = false;
						e.Handled = false;
					}
					break;
				case Keys.Enter:
				case Keys.Tab:
					ValueChanged();
					break;
				default:
					e.SuppressKeyPress = true;
					e.Handled = true;
					break;
			}
		}

		private void NumberTextBox_TextChanged(object sender, EventArgs e)
		{
			cachedRationalNumberIsValid = false;
			UpdateAlternateLabel();
		}
	}
}
