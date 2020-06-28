using Ionic.Zlib;
using SatisfactorySaveParser.Save;
using SatisfactorySaveParser.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

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

            using (var stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new BinaryReader(stream))
            {
                Header = FSaveHeader.Parse(reader);

                if (Header.SaveVersion < FSaveCustomVersion.SaveFileIsCompressed)
                {
                    LoadData(reader);
                }
                else
                {
                    using (var buffer = new MemoryStream())
                    {
                        var uncompressedSize = 0L;

                        while (stream.Position < stream.Length)
                        {
                            var header = reader.ReadChunkInfo();
                            Trace.Assert(header.CompressedSize == ChunkInfo.Magic);

                            var summary = reader.ReadChunkInfo();

                            var subChunk = reader.ReadChunkInfo();
                            Trace.Assert(subChunk.UncompressedSize == summary.UncompressedSize);

                            var startPosition = stream.Position;
                            using (var zStream = new ZlibStream(stream, CompressionMode.Decompress, true))
                            {
                                zStream.CopyTo(buffer);
                            }

                            // ZlibStream appears to read more bytes than it uses (because of buffering probably) so we need to manually fix the input stream position
                            stream.Position = startPosition + subChunk.CompressedSize;

                            uncompressedSize += subChunk.UncompressedSize;
                        }


                        buffer.Position = 0;

#if DEBUG
                        //File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + ".bin"), buffer.ToArray());
#endif


                        using (var bufferReader = new BinaryReader(buffer))
                        {
                            var dataLength = bufferReader.ReadInt32();
                            Trace.Assert(uncompressedSize == dataLength + 4);

                            LoadData(bufferReader);
                        }
                    }
                }
            }
        }

        private void LoadData(BinaryReader reader)
        {
            // Does not need to be a public property because it's equal to Entries.Count
            var totalSaveObjects = reader.ReadUInt32();
            // Saved entities loop
            for (int i = 0; i < totalSaveObjects; i++)
            {
                var type = reader.ReadInt32();
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

            var totalSaveObjectData = reader.ReadInt32();
            Trace.Assert(Entries.Count == totalSaveObjects);
            Trace.Assert(Entries.Count == totalSaveObjectData);

            for (int i = 0; i < Entries.Count; i++)
            {
                var len = reader.ReadInt32();
                var before = reader.BaseStream.Position;

#if DEBUG
                //log.Trace($"Reading {len} bytes @ {before} for {Entries[i].TypePath}");
#endif

                Entries[i].ParseData(len, reader);
                var after = reader.BaseStream.Position;

                if (before + len != after)
                {
                    throw new InvalidOperationException($"Expected {len} bytes read but got {after - before}");
                }
            }

            var collectedObjectsCount = reader.ReadInt32();
            for (int i = 0; i < collectedObjectsCount; i++)
            {
                CollectedObjects.Add(new ObjectReference(reader));
            }

            Trace.Assert(reader.BaseStream.Position == reader.BaseStream.Length);
        }
    }
}
