using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public abstract class SaveableData
{
	public string saveableDataRootString = "parent-class-string";
	

	public bool Save(string path, string filename, string extension)
	{
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
				+ (shouldSaveAsRef  ? "   << SHOULD BE CONVERTED TO GUID >>" : "")
				+ "\n";
		}

		Debug.Log(log);

		// we don't save yet...
		return false;
	}

	public bool Load(string path, string filename, string extension)
	{
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
