using GalaSoft.MvvmLight;
using SatisfactorySaveEditor.Util;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveParser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SatisfactorySaveEditor.Model
{
	public class SaveObjectModel : ViewModelBase
	{
		private string title;
		private string rootObject;
		private string type;
		private bool isSelected;
		private bool isExpanded;

		public ObservableCollection<SaveObjectModel> Items { get; } = new ObservableCollection<SaveObjectModel>();

		public ObservableCollection<SerializedPropertyViewModel> Fields { get; }

		/// <summary>
		/// Recursively gets all the SaveObject children in the tree plus self
		/// </summary>
		public List<SaveObject> DescendantSelf
		{
			get
			{
				var list = new List<SaveObject>();
				if (Model != null) list.Add(Model);

				foreach (var item in Items) list.AddRange(item.DescendantSelf);

				return list;
			}
		}

		/// <summary>
		/// Recursively gets all the SaveObjectModel children in the tree plus self
		/// </summary>
		public List<SaveObjectModel> DescendantSelfViewModel
		{
			get
			{
				var list = new List<SaveObjectModel>();
				if (Model != null) list.Add(this);

				foreach (var item in Items) list.AddRange(item.DescendantSelfViewModel);

				return list;
			}
		}

		public string Title
		{
			get => title;
			set { Set(() => Title, ref title, value); }
		}

		public string RootObject
		{
			get => rootObject;
			set { Set(() => RootObject, ref rootObject, value); }
		}

		public string Type
		{
			get => type;
			set { Set(() => Type, ref type, value); }
		}

		public bool IsSelected
		{
			get => isSelected;
			set { Set(() => IsSelected, ref isSelected, value); }
		}

		public bool IsExpanded
		{
			get => isExpanded;
			set { Set(() => IsExpanded, ref isExpanded, value); }
		}

		public SaveObject Model { get; }

		public SaveObjectModel(SaveObject model)
		{
			Model = model;
			Title = model.InstanceName;
			RootObject = model.RootObject;
			Type = model.TypePath.Split('/').Last();

			Fields = new ObservableCollection<SerializedPropertyViewModel>(Model.DataFields.Select(PropertyViewModelMapper.Convert));
		}

		public SaveObjectModel(string title)
		{
			Title = title;
			Type = title;
		}

		/// <summary>
		/// Recursively walks through the save tree and finds a child node with 'name' title
		/// Expand also opens all parent nodes on return
		/// </summary>
		/// <param name="name">Name of the searched node</param>
		/// <param name="expand">Whether the parents of searched node should be expanded</param>
		/// <returns></returns>
		public SaveObjectModel FindChild(string name, bool expand)
		{
			if (title == name)
			{
				if (expand) IsSelected = true;
				return this;
			}

			foreach (var item in Items)
			{
				var foundChild = item.FindChild(name, expand);
				if (foundChild != null)
				{
					if (expand) IsExpanded = true;
					return foundChild;
				}
			}

			return null;
		}

		public override string ToString()
		{
			return $"{Title} ({Items.Count})";
		}

		public T FindField<T>(string fieldName, Action<T> edit = null) where T : SerializedPropertyViewModel
		{
			var field = Fields.FirstOrDefault(f => f.PropertyName == fieldName);

			if (field == null)
			{
				return null;
			}

			if (field is T vm)
			{
				edit?.Invoke(vm);
				return vm;
			}

			throw new InvalidOperationException($"A field with the name {fieldName} was found but with a different type ({field.GetType()} != {typeof(T)})");
		}

		public T FindOrCreateField<T>(string fieldName, Action<T> edit = null) where T : SerializedPropertyViewModel
		{
			var field = Fields.FirstOrDefault(f => f.PropertyName == fieldName);

			if (field == null)
			{
				var newVM = (T)PropertyViewModelMapper.Create<T>(fieldName);
				Fields.Add(newVM);

				edit?.Invoke(newVM);

				return newVM;
			}

			if (field is T vm)
			{
				edit?.Invoke(vm);
				return vm;
			}

			throw new InvalidOperationException($"A field with the name {fieldName} already exists but with a different type ({field.GetType()} != {typeof(T)})");
		}
	}
}
