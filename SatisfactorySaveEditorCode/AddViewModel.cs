using System;
using System.Windows;

using GalaSoft.MvvmLight;

using SatisfactorySaveEditor.Model;

using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel
{
	public class AddViewModel : ViewModelBase
	{
		public enum AddTypeEnum
		{
			Array,
			Bool,
			Byte,
			Enum,
			Float,
			Int,
			Map,
			Name,
			Object,
			String,
			Struct,
			Text,
			Interface,
			Int64
		}

		private AddTypeEnum type = AddTypeEnum.Array;
		private AddTypeEnum arrayType = AddTypeEnum.Bool;
		private string name;

		public SaveObjectModel ObjectModel { get; set; }

		public string Name
		{
			get
			{
				return name;
			}

			set
			{
				Set(() => Name, ref name, value);
				RaisePropertyChanged(() => CanConfirm);
			}
		}
		public AddTypeEnum Type
		{
			get
			{
				return type;
			}

			set
			{
				Set(() => Type, ref type, value);
				RaisePropertyChanged(() => IsArray);
				RaisePropertyChanged(() => CanConfirm);
			}
		}
		public AddTypeEnum ArrayType
		{
			get
			{
				return arrayType;
			}

			set
			{
				Set(() => ArrayType, ref arrayType, value);
				RaisePropertyChanged(() => CanConfirm);
			}
		}

		public Visibility IsArray
		{
			get
			{
				return type == AddTypeEnum.Array ? Visibility.Visible : Visibility.Collapsed;
			}
		}

		public bool CanConfirm
		{
			get
			{
				if (type != AddTypeEnum.Array)
				{
					return !string.IsNullOrWhiteSpace(Name);
				}

				return arrayType != AddTypeEnum.Array && !string.IsNullOrWhiteSpace(Name);
			}
		}

		public static SerializedProperty CreateProperty(AddTypeEnum type, string name)
		{
			switch (type)
			{
				case AddTypeEnum.Array:
					return new ArrayProperty(name);
				case AddTypeEnum.Bool:
					return new BoolProperty(name);
				case AddTypeEnum.Byte:
					return new ByteProperty(name);
				case AddTypeEnum.Enum:
					return new EnumProperty(name);
				case AddTypeEnum.Float:
					return new FloatProperty(name);
				case AddTypeEnum.Int:
					return new IntProperty(name);
				case AddTypeEnum.Map:
					return new MapProperty(name);
				case AddTypeEnum.Name:
					return new NameProperty(name);
				case AddTypeEnum.Object:
					return new ObjectProperty(name);
				case AddTypeEnum.String:
					return new StrProperty(name);
				case AddTypeEnum.Struct:
					return new StructProperty(name);
				case AddTypeEnum.Text:
					return new TextProperty(name);
				case AddTypeEnum.Interface:
					return new InterfaceProperty(name);
				case AddTypeEnum.Int64:
					return new Int64Property(name);
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}

		public static AddTypeEnum FromStringType(string stringType)
		{
			switch (stringType)
			{
				case "ArrayProperty":
					return AddTypeEnum.Array;
				case "BoolProperty":
					return AddTypeEnum.Bool;
				case "ByteProperty":
					return AddTypeEnum.Byte;
				case "EnumProperty":
					return AddTypeEnum.Enum;
				case "FloatProperty":
					return AddTypeEnum.Float;
				case "IntProperty":
					return AddTypeEnum.Int;
				case "MapProperty":
					return AddTypeEnum.Map;
				case "NameProperty":
					return AddTypeEnum.Name;
				case "ObjectProperty":
					return AddTypeEnum.Object;
				case "StrProperty":
					return AddTypeEnum.String;
				case "StructProperty":
					return AddTypeEnum.Struct;
				case "TextProperty":
					return AddTypeEnum.Text;
				case "InterfaceProperty":
					return AddTypeEnum.Interface;
				case "Int64Property":
					return AddTypeEnum.Int64;
				default:
					throw new ArgumentOutOfRangeException(nameof(stringType), stringType, null);
			}
		}
	}
}
