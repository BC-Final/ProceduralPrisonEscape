using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropBeacon : MonoBehaviour {
	[SerializeField]
	public List<ShooterDoor> _doors;

	private void Start() {
		foreach (ShooterDoor d in _doors) {
			d.SetRequireKeyCard();
		}
	}

	public void Drop() {
		if (_doors != null && _doors.Count > 0) {
			GameObject go = Instantiate(Resources.Load("prefabs/shooter/pickups/pfb_dronebeacon"), transform.position, Quaternion.identity) as GameObject;
			go.GetComponent<KeyCard>().SetDoors(_doors);
		}
	}
}
