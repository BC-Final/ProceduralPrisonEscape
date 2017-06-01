using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushTrigger : MonoBehaviour {
	[SerializeField]
	private DronePoint[] _targets;

	public void TriggerAmbush() {
		foreach (DronePoint dp in _targets) {
			if (dp.Occupied) {
				dp.Occupant.StartAmbush();
			}
		} 
	}
}
