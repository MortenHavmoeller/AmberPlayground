using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using ProjectAmber.SavingData.DataSurrogates;

public class AmberBinaryFormatter
{
	private BinaryFormatter internalBinaryFormatter;
	
	public AmberBinaryFormatter()
	{
		// initialize
		internalBinaryFormatter = new BinaryFormatter();
		SurrogateSelector surrogateSelector = new SurrogateSelector();

		// add surrogates
		Vector3_SerializationSurrogate vector3SS = new Vector3_SerializationSurrogate();
		surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3SS);
		
		// apply to binary formatter
		internalBinaryFormatter.SurrogateSelector = surrogateSelector;
	}

	public void Serialize(System.IO.Stream serializationStream, object graph)
	{
		internalBinaryFormatter.Serialize(serializationStream, graph);
	}

	public object Deserialize(System.IO.Stream serializationStream)
	{
		return internalBinaryFormatter.Deserialize(serializationStream);
	}
}
