using Microsoft.VisualStudio.TestTools.UnitTesting;
using VisualSatisfactoryCalculator.code.Numbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code.Numbers.Tests
{
	[TestClass]
	public class RationalNumberTests
	{
		[TestMethod]
		public void AddTest()
		{
			// Whole Numbers
			RationalNumber zero = 0, one = 1, two = 2, three = 3, four = 4, nOne = -1, nTwo = -2, nThree = -3, nFour = -4;
			// Adding Positives
			Assert.AreEqual(zero + zero, zero);
			Assert.AreEqual(zero + one, one);
			Assert.AreEqual(zero + one, one + zero);
			Assert.AreEqual(zero + two, two);
			Assert.AreEqual(two + zero, zero + two);
			Assert.AreEqual(one + one, two);
			Assert.AreEqual(one + two, three);
			Assert.AreEqual(two + one, one + two);
			Assert.AreEqual(two + two, four);
			// Adding Negatives
			Assert.AreEqual(zero + nOne, nOne);
			Assert.AreEqual(nOne + zero, nOne);
			Assert.AreEqual(zero + nTwo, nTwo);
			Assert.AreEqual(zero + nTwo, nTwo + zero);
			Assert.AreEqual(nOne + nOne, nTwo);
			Assert.AreEqual(nOne + nTwo, nThree);
			Assert.AreEqual(nTwo + nOne, nOne + nTwo);
			Assert.AreEqual(nTwo + nTwo, nFour);
			// Added Positives and Negatives
			Assert.AreEqual(nTwo + one, nOne);
			Assert.AreEqual(one + nTwo, nOne);
			Assert.AreEqual(nTwo + two, zero);
			Assert.AreEqual(two + nTwo, zero);
			Assert.AreEqual(nOne + one, zero);
			Assert.AreEqual(one + nOne, zero);
			Assert.AreEqual(nOne + two, one);
			Assert.AreEqual(two + nOne, one);
			// Fractions
			RationalNumber a = new RationalNumber(5, 3), b = new RationalNumber(5, 3, false), c = new RationalNumber(10, 3), d = new RationalNumber(10, 3, false);
			Assert.AreEqual(a + b, zero);
			Assert.AreEqual(b + a, zero);
			Assert.AreEqual(a + a, c);
			Assert.AreEqual(b + b, d);
			Assert.AreEqual(c + b, a);
			Assert.AreEqual(b + c, a);
			Assert.AreEqual(d + a, b);
			Assert.AreEqual(a + d, b);
		}

		[TestMethod]
		public void CeilingTest()
		{
			Assert.AreEqual(new RationalNumber(0.1234).Ceiling(3), new RationalNumber(0.124));
		}
	}
}