using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidRef<T>
{
	public string guid;
	public T reference;

	public static GuidRef<T>Create(T obj)
	{
		GuidRef<T> result = new GuidRef<T>();
		result.guid = Guid.NewGuid().ToString();
		result.reference = obj;
		return result;
	}
}
