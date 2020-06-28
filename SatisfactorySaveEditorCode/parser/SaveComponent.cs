using System.IO;

namespace SatisfactorySaveParser
{
    /// <summary>
    ///     Engine class: FObjectSaveHeader
    /// </summary>
    public class SaveComponent : SaveObject
    {
        public const int TypeID = 0;

        /// <summary>
        ///     Instance name of the parent entity object
        /// </summary>
        public string ParentEntityName { get; set; }

        public SaveComponent(string typePath, string rootObject, string instanceName) : base(typePath, rootObject, instanceName)
        {
        }

        public SaveComponent(BinaryReader reader) : base(reader)
        {
            ParentEntityName = reader.ReadLengthPrefixedString();
        }

        public override string ToString()
        {
            return TypePath;
        }
    }
}
