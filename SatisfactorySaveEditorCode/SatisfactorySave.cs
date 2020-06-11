using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Ionic.Zlib;

using SatisfactorySaveParser.Save;
using SatisfactorySaveParser.Structures;

namespace SatisfactorySaveParser
{
	/// <summary>
	///     SatisfactorySave is the main class for parsing a savegame
	/// </summary>
	public class SatisfactorySave
	{
		/// <summary>
		///     Path to save on disk
		/// </summary>
		public string FileName { get; private set; }

		/// <summary>
		///     Header part of the save containing things like the version and metadata
		/// </summary>
		public FSaveHeader Header { get; private set; }

		/// <summary>
		///     Main content of the save game
		/// </summary>
		public List<SaveObject> Entries { get; set; } = new List<SaveObject>();

		/// <summary>
		///     List of object references of all collected objects in the world (Nut/berry bushes, slugs, etc)
		/// </summary>
		public List<ObjectReference> CollectedObjects { get; set; } = new List<ObjectReference>();

		/// <summary>
		///     Open a savefile from disk
		/// </summary>
		/// <param name="file">Full path to the .sav file, usually found in %localappdata%/FactoryGame/Saved/SaveGames</param>
		public SatisfactorySave(string file)
		{
			FileName = Environment.ExpandEnvironmentVariables(file);

			using (FileStream stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			using (BinaryReader reader = new BinaryReader(stream))
			{
				Header = FSaveHeader.Parse(reader);

				if (Header.SaveVersion < FSaveCustomVersion.SaveFileIsCompressed)
				{
					LoadData(reader);
				}
				else
				{
					using (MemoryStream buffer = new MemoryStream())
					{
						long uncompressedSize = 0L;

						while (stream.Position < stream.Length)
						{
							ChunkInfo header = reader.ReadChunkInfo();
							Trace.Assert(header.CompressedSize == ChunkInfo.Magic);

							ChunkInfo summary = reader.ReadChunkInfo();

							ChunkInfo subChunk = reader.ReadChunkInfo();
							Trace.Assert(subChunk.UncompressedSize == summary.UncompressedSize);

							long startPosition = stream.Position;
							using (ZlibStream zStream = new ZlibStream(stream, CompressionMode.Decompress, true))
							{
								zStream.CopyTo(buffer);
							}

							stream.Position = startPosition + subChunk.CompressedSize;

							uncompressedSize += subChunk.UncompressedSize;
						}


						buffer.Position = 0;

						using (BinaryReader bufferReader = new BinaryReader(buffer))
						{
							int dataLength = bufferReader.ReadInt32();
							Trace.Assert(uncompressedSize == dataLength + 4);

							LoadData(bufferReader);
						}
					}
				}
			}
		}

		private void LoadData(BinaryReader reader)
		{
			uint totalSaveObjects = reader.ReadUInt32();

			for (int i = 0; i < totalSaveObjects; i++)
			{
				int type = reader.ReadInt32();
				switch (type)
				{
					case SaveEntity.TypeID:
						Entries.Add(new SaveEntity(reader));
						break;
					case SaveComponent.TypeID:
						Entries.Add(new SaveComponent(reader));
						break;
					default:
						throw new InvalidOperationException($"Unexpected type {type}");
				}
			}

			int totalSaveObjectData = reader.ReadInt32();
			Trace.Assert(Entries.Count == totalSaveObjects);
			Trace.Assert(Entries.Count == totalSaveObjectData);

			for (int i = 0; i < Entries.Count; i++)
			{
				int len = reader.ReadInt32();
				long before = reader.BaseStream.Position;

				Entries[i].ParseData(len, reader);
				long after = reader.BaseStream.Position;

				if (before + len != after)
				{
					throw new InvalidOperationException($"Expected {len} bytes read but got {after - before}");
				}
			}

			int collectedObjectsCount = reader.ReadInt32();
			for (int i = 0; i < collectedObjectsCount; i++)
			{
				CollectedObjects.Add(new ObjectReference(reader));
			}

			Trace.Assert(reader.BaseStream.Position == reader.BaseStream.Length);
		}
	}
}
