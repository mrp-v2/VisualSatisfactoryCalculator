using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code.Utility
{
	class BitmapWriter
	{
		private int xPosition;
		private int yPosition;

		private readonly Bitmap map;

		public BitmapWriter(Bitmap map)
		{
			this.map = map;
		}

		public void WriteBytes(byte[] bytes)
		{
			for (int i = 0; i < bytes.Length; i += 3)
			{
				byte[] colorBytes = new byte[24];
				for (int j = 0; j < 24; j++)
				{
					if (i + j / 8 >= bytes.Length)
					{
						colorBytes[j] = 0;
					}
					else
					{
						colorBytes[j] = IsolateBit(bytes[i + j / 8], j % 8);
					}
					if ((j + 1) % 3 == 0)
					{
						map.SetPixel(xPosition, yPosition, Color.FromArgb(0xFF, 0xFE | colorBytes[j - 2], 0xFE | colorBytes[j - 1], 0xFE | colorBytes[j]));
						AdvancePosition();
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="colors">The length should always be a multiple of 8</param>
		/// <returns></returns>
		private byte[] ReadFromColors(Color[] colors)
		{
			if (colors.Length % 8 != 0)
			{
				throw new ArgumentException("The length of the given array has an ivalid length! It should be a multiple of 8.");
			}
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="colors">There should be 8 colors for every 3 bytes</param>
		/// <param name="bytes">There should be 3 bytes for every 8 colors</param>
		/// <returns></returns>
		private Color[] WriteToColors(Color[] colors, byte[] bytes)
		{
			if (colors.Length * 8 != bytes.Length * 3)
			{
				throw new ArgumentException("The lengths of the given arrays don't have the correct ratio! colors:bytes should be 8:3.");
			}
			throw new NotImplementedException();
		}

		private byte IsolateBit(byte b, int bitIndex)
		{
			//shift right to delete bits after position
			b = (byte)(b >> (bitIndex));
			//subtract bits before position
			b -= (byte)(b & 0b11111110);
			//shift back to correct position
			b = (byte)(b << (bitIndex));
			return b;
		}

		public byte[] ReadBytes(int count)
		{
			byte[] bytes = new byte[count];
			for (int i = 0; i < count; i += 3)
			{
				byte[] colorBytes = new byte[24];
				for (int j = 0; j < 24; j += 3)
				{
					colorBytes[j] = (byte)(map.GetPixel(xPosition, yPosition).R & 0b1);
					colorBytes[j + 1] = (byte)(map.GetPixel(xPosition, yPosition).G & 0b1);
					colorBytes[j + 2] = (byte)(map.GetPixel(xPosition, yPosition).B & 0b1);
					if ((j + 1) % 3 == 0)
					{
						AdvancePosition();
					}
				}
				bytes[i] = MergeBytes(colorBytes, 0);
				if (i + 2 < count)
				{
					bytes[i + 1] = MergeBytes(colorBytes, 8);
					bytes[i + 2] = MergeBytes(colorBytes, 16);
				}
				else if (i + 1 < count)
				{
					bytes[i + 1] = MergeBytes(colorBytes, 8);
				}
			}
			return bytes;
		}

		private void AdvancePosition()
		{
			xPosition++;
			if (xPosition >= map.Width)
			{
				xPosition -= map.Width;
				yPosition++;
			}
		}

		/// <summary>
		/// Merges 8 single bit bytes into 1 8 bit byte
		/// </summary>
		/// <param name="bytes"></param>
		/// <param name="startIndex"></param>
		/// <returns></returns>
		private byte MergeBytes(byte[] bytes, int startIndex)
		{
			byte b = 0;
			for (int i = 0; i < 8; i++)
			{
				b |= (byte)(bytes[startIndex + i] << (7 - i));
			}
			return b;
		}
	}
}
