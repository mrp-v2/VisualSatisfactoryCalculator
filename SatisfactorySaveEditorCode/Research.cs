﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SatisfactorySaveParser.Data
{
	public class Research
	{
		public string Path { get; set; }

		public Research(XElement element)
		{
			Path = element.Attribute("value").Value;
		}

		public static IEnumerable<Research> GetResearches()
		{
			XDocument doc = XDocument.Load("Data/Research.xml");
			XElement node = doc.Element("ResearchData");

			return node.Elements("Research").Select(c => new Research(c));
		}
	}
}
