﻿using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
	public class RailroadTrackPosition : IStructData
	{
		public string Root { get; set; }
		public string InstanceName { get; set; }
		public float Offset { get; set; }
		public float Forward { get; set; }


		public int SerializedLength
		{
			get
			{
				return Root.GetSerializedLength() + InstanceName.GetSerializedLength() + 8;
			}
		}

		public string Type
		{
			get
			{
				return "RailroadTrackPosition";
			}
		}

		public RailroadTrackPosition(BinaryReader reader)
		{
			Root = reader.ReadLengthPrefixedString();
			InstanceName = reader.ReadLengthPrefixedString();
			Offset = reader.ReadSingle();
			Forward = reader.ReadSingle();
		}
	}
}
