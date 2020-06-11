using SatisfactorySaveParser;
using SatisfactorySaveParser.Structures;

namespace SatisfactorySaveEditor.Model
{
	public class SaveEntityModel : SaveObjectModel
	{
		private bool needTransform;
		private bool wasPlacedInLevel;
		private Vector4 rotation;
		private Vector3 position;
		private Vector3 scale;
		private string parentObjectRoot;
		private string parentObjectName;

		public bool NeedTransform
		{
			get
			{
				return needTransform;
			}

			set { Set(() => NeedTransform, ref needTransform, value); }
		}

		public bool WasPlacedInLevel
		{
			get
			{
				return wasPlacedInLevel;
			}

			set { Set(() => WasPlacedInLevel, ref wasPlacedInLevel, value); }
		}

		public Vector4 Rotation
		{
			get
			{
				return rotation;
			}

			set { Set(() => Rotation, ref rotation, value); }
		}
		public Vector3 Position
		{
			get
			{
				return position;
			}

			set { Set(() => Position, ref position, value); }
		}

		public Vector3 Scale
		{
			get
			{
				return scale;
			}

			set { Set(() => Scale, ref scale, value); }
		}

		public string ParentObjectRoot
		{
			get
			{
				return parentObjectRoot;
			}

			set { Set(() => ParentObjectRoot, ref parentObjectRoot, value); }
		}

		public string ParentObjectName
		{
			get
			{
				return parentObjectName;
			}

			set { Set(() => ParentObjectName, ref parentObjectName, value); }
		}

		public SaveEntityModel(SaveEntity ent) : base(ent)
		{
			NeedTransform = ent.NeedTransform;
			WasPlacedInLevel = ent.WasPlacedInLevel;
			ParentObjectRoot = ent.ParentObjectRoot;
			ParentObjectName = ent.ParentObjectName;

			Rotation = ent.Rotation;
			Position = ent.Position;
			Scale = ent.Scale;
		}
	}
}
