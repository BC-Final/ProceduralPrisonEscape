using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSpawner : MonoBehaviour {
	public static List<DroneSpawner> DroneSpawners = new List<DroneSpawner>();

	private void OnEnable () {
		DroneSpawners.Add(this);
	}

	private void OnDestroy () {
		DroneSpawners.Remove(this);
	}
}
