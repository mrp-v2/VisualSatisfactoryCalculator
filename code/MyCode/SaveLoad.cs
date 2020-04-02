using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VisualSatisfactoryCalculator.code
{
	public static class SaveLoad
	{
		private static string GetDirectory()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		}

		private static string FullPath(string fileName)
		{
			return GetDirectory() + "\\" + fileName;
		}

		private static bool CheckForFile(string dataPath)
		{
			if (File.Exists(FullPath(dataPath)))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static void Save<T>(string fileName, T obj)
		{
			if (CheckForFile(fileName))
			{
				File.Delete(FullPath(fileName));
			}
			BinaryFormatter bf = new BinaryFormatter();
			FileStream f = File.Create(FullPath(fileName));
			bf.Serialize(f, obj);
			f.Close();
		}

		public static T Load<T>(string fileName)
		{
			if (CheckForFile(fileName))
			{
				BinaryFormatter bf = new BinaryFormatter();
				FileStream f = File.Open(FullPath(fileName), FileMode.Open);
				T obj = default;
				try
				{
					obj = (T)bf.Deserialize(f);
				}
				catch
				{

				}
				f.Close();
				return obj;
			}
			else
			{
				return default;
			}
		}
	}
}
