using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public abstract class SaveableData
{
	public string saveableDataRootString = "";


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

		string log = "Saving type " + GetType() + " - it has " + fieldValues.Count + " fields\n";

		for (int i= 0; i < fieldValues.Count; i++)
		{
			log += "field " + i + ": " + fieldNames[i] + " [" + fieldValues[i].GetType().ToString() +"] => " + fieldValues[i].ToString() + "\n";
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
}
