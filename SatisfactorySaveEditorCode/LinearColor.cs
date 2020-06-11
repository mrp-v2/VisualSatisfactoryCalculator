﻿using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
	public class LinearColor : IStructData
	{
		public float R { get; set; }
		public float G { get; set; }
		public float B { get; set; }
		public float A { get; set; }

		public int SerializedLength
		{
			get
			{
				return 16;
			}
		}

		public string Type
		{
			get
			{
				return "LinearColor";
			}
		}

		public LinearColor(float r, float g, float b, float a)
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}

		public LinearColor(BinaryReader reader)
		{
			R = reader.ReadSingle();
			G = reader.ReadSingle();
			B = reader.ReadSingle();
			A = reader.ReadSingle();
		}
	}
}
