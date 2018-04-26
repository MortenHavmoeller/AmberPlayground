using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularUint64
{
	// Uint64 which wraps around at Max and Min values.
	// will judge a value of 0 to be greater than any value higher than Uint64.Max/2
	// will judge a value of 1 to be greater than any value higher than Uint64.Max/2 + 1
	// will judge a value of 2 to be greater than any value higher than Uint64.Max/2 + 2
	// etc.



}
