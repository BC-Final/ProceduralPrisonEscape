using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aiTESTSACRIPT : MonoBehaviour {

	// Use this for initialization
	void Update () {
		System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
		Utilities.AI.ObjectInRange(transform, transform.GetChild(0), 10);
		Debug.Log(watch.ElapsedTicks);
	}
}
