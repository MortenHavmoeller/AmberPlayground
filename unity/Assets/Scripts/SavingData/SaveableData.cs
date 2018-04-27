using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[Serializable]
public abstract class SaveableData
{
	public string saveableDataRootString = "parent-class-string";
	

	public bool Save(string path, string filename, string extension)
	{
		// check if class type is marked as serializable
		Type type = GetType();
		if (!type.IsDefined(typeof(SerializableAttribute), false))
			throw new InvalidOperationException("Tried saving class of type " + type.ToString() + ", but it was not marked as Serializable");


		var bindingFlags =
			BindingFlags.Instance |
			BindingFlags.NonPublic |
			BindingFlags.Public;

		List<object> fieldValues = GetType()
			.GetFields(bindingFlags)
			.Select(field => field.GetValue(this))
			.ToList();

		List<string> fieldNames = GetType()
			.GetFields(bindingFlags)
			.Select(field => field.Name)
			.ToList();

		string log = "SAVING TYPE '" + GetType() + "' - it has " + fieldValues.Count + " fields\n";

		for (int i= 0; i < fieldValues.Count; i++)
		{
			Type fieldType = fieldValues[i].GetType();
			bool shouldSaveAsRef = ShouldSaveAsGuidRef(fieldType);

			log += "field " + (i + 1) + ": " + fieldNames[i] 
				+ " [" + fieldType.ToString() +"] => '" 
				+ fieldValues[i].ToString()  + "'"
				+ (shouldSaveAsRef  ? "   << WILL BE CONVERTED TO GUID REFERENCE >>" : "")
				+ "\n";
		}

		Debug.Log(log);

		// TODO: Find out if current class is Serializable!

		// BINARY FORMATTING TO FILE
		try
		{
			AmberBinaryFormatter binaryFormatter = new AmberBinaryFormatter();
			string fullPath = Application.persistentDataPath + filename + extension;

			using (FileStream fileStream = File.Open(fullPath, FileMode.OpenOrCreate))
			{
				binaryFormatter.Serialize(fileStream, this);
				fileStream.Close();
			}

			Debug.Log("Saved file to " + fullPath);
			return true;
		}
		catch (Exception ex)
		{
			Debug.LogError("FAILED SAVING FILE\n" + ex);
			return false;
		}
	}

	public bool Load(string path, string filename, string extension)
	{

		string fullPath = Application.persistentDataPath + filename + extension;

		// READ BINARY FORMATTING
		if (File.Exists(fullPath))
		{
			AmberBinaryFormatter binaryFormatter = new AmberBinaryFormatter();
			using (FileStream fileStream = File.Open(fullPath, FileMode.Open))
			{
				object data = binaryFormatter.Deserialize(fileStream);
				fileStream.Close();

				Debug.Log("Got a file from disk. Type was: " + data.GetType());

				return true;
			}
		}



		// we don't load yet...
		return false;
	}

	private static bool ShouldSaveAsGuidRef(Type type)
	{
		if (type == typeof(string))
			return false;

		return type.IsClass;
	}
}
