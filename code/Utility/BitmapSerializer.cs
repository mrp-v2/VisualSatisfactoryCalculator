using System;
using System.Drawing;
using VisualSatisfactoryCalculator.code.Extensions;

namespace VisualSatisfactoryCalculator.code.Utility
{
	class BitmapSerializer
	{
		private int xPosition;
		private int yPosition;

		private readonly Bitmap map;

		public static readonly byte removeFinalBitMask = 0xFE;
		public static readonly byte onlyFinalBitMask = 0b1;

		public BitmapSerializer(Bitmap map)
		{
			xPosition = 0;
			yPosition = 0;
			this.map = map;
		}

		public void WriteBytes(byte[] bytes)
		{
			while (bytes.Length % 3 != 0)
			{
				bytes = bytes.AddElement<byte>(0);
			}
			byte[] seperatedBytes = SeperateBytes(bytes);
			for (int i = 0; i < seperatedBytes.Length; i += 3)
			{
				Color color = OverwriteColor(map.GetPixel(xPosition, yPosition), seperatedBytes.SubArray(i, 3));
				map.SetPixel(xPosition, yPosition, color);
				AdvancePosition();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="colors">The length should always be a multiple of 8</param>
		/// <returns></returns>
		private byte[] ReadFromColor(Color color)
		{
			return new byte[] {
				(byte)(color.R & onlyFinalBitMask),
				(byte)(color.G & onlyFinalBitMask),
				(byte)(color.B & onlyFinalBitMask)
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="original"></param>
		/// <param name="bytes">Length of 3</param>
		/// <returns></returns>
		private Color OverwriteColor(Color original, byte[] bytes)
		{
			if (bytes.Length != 3)
			{
				throw new ArgumentException("bytes should have a length of 3.");
			}
			return Color.FromArgb(original.A, (original.R & removeFinalBitMask) | bytes[0], (original.G & removeFinalBitMask) | bytes[1], (original.B & removeFinalBitMask) | bytes[2]);
		}

		private byte IsolateBit(byte b, int bitIndex)
		{
			//shift right to delete bits after position
			b = (byte)(b >> (bitIndex));
			//mask to only last bit
			b = (byte)(b & onlyFinalBitMask);
			return b;
		}

		public byte[] ReadBytes(int count)
		{
			int adjustedCount = count;
			while (adjustedCount % 3 != 0)
			{
				adjustedCount++;
			}
			byte[] bytes = new byte[adjustedCount];
			for (int i = 0; i < adjustedCount; i += 3)
			{
				ReadBytes().CopyTo(bytes, i);
			}
			if (count != bytes.Length)
			{
				return bytes.SubArray(0, count);
			}
			return bytes;
		}

		private byte[] ReadBytes()
		{
			byte[] bytes = new byte[24];
			for (int j = 0; j < 8; j++)
			{
				ReadFromColor(map.GetPixel(xPosition, yPosition)).CopyTo(bytes, j * 3);
				AdvancePosition();
			}
			return MergeBytes(bytes);
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
		/// Merges 8 single bit bytes into a byte
		/// </summary>
		/// <param name="bytes"></param>
		/// <param name="startIndex"></param>
		/// <returns></returns>
		private byte MergeBytes(byte[] bytes, int startIndex)
		{
			byte b = 0;
			for (int i = 0; i < 8; i++)
			{
				if (bytes[i] > 1) throw new ArgumentException("A byte in the array was greater than 1 bit!");
				b |= (byte)(bytes[startIndex + i] << (7 - i));
			}
			return b;
		}

		/// <summary>
		/// Merges single bit bytes into a byte
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		private byte[] MergeBytes(byte[] bytes)
		{
			if (bytes.Length % 8 != 0) throw new ArgumentException("The length of the given array is not divisible by 8!");
			byte[] array = new byte[bytes.Length / 8];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = MergeBytes(bytes, i * 8);
			}
			return array;
		}

		/// <summary>
		/// Seperates a byte into 8 single bit bytes
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		private byte[] SeperateBytes(byte[] bytes)
		{
			byte[] seperated = new byte[bytes.Length * 8];
			for (int i = 0; i < bytes.Length; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					seperated[i * 8 + j] = IsolateBit(bytes[i], 7 - j);
				}
			}
			return seperated;
		}
	}
}
