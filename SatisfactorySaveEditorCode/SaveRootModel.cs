using SatisfactorySaveParser.Save;

namespace SatisfactorySaveEditor.Model
{
	public class SaveRootModel : SaveObjectModel
	{
		private readonly FSaveHeader model;

		private string mapName;
		private string mapOptions;
		private string sessionName;
		private int playDuration;
		private long saveDateTime;
		private ESessionVisibility sessionVisibility;

		public SaveHeaderVersion HeaderVersion
		{
			get
			{
				return model.HeaderVersion;
			}
		}

		public FSaveCustomVersion SaveVersion
		{
			get
			{
				return model.SaveVersion;
			}
		}

		public string MapName
		{
			get
			{
				return mapName;
			}

			set { Set(() => MapName, ref mapName, value); }
		}

		public string MapOptions
		{
			get
			{
				return mapOptions;
			}

			set { Set(() => MapOptions, ref mapOptions, value); }
		}

		public string SessionName
		{
			get
			{
				return sessionName;
			}

			set { Set(() => SessionName, ref sessionName, value); }
		}

		public int PlayDuration
		{
			get
			{
				return playDuration;
			}

			set { Set(() => PlayDuration, ref playDuration, value); }
		}

		public bool HasSessionVisibility
		{
			get
			{
				return HeaderVersion >= SaveHeaderVersion.AddedSessionVisibility;
			}
		}

		public ESessionVisibility SessionVisibility
		{
			get
			{
				return sessionVisibility;
			}

			set { Set(() => SessionVisibility, ref sessionVisibility, value); }
		}

		public long SaveDateTime
		{
			get
			{
				return saveDateTime;
			}

			set { Set(() => SaveDateTime, ref saveDateTime, value); }
		}

		public SaveRootModel(FSaveHeader header) : base(header.SessionName)
		{
			model = header;
			Type = "Root";

			mapName = model.MapName;
			mapOptions = model.MapOptions;
			sessionName = model.SessionName;
			playDuration = model.PlayDuration;
			sessionVisibility = model.SessionVisibility;
			saveDateTime = model.SaveDateTime;
		}
	}
}
