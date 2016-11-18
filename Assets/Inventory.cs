using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
	private List<Door> _accessibleDoors;

	private void Awake() {
		_accessibleDoors = new List<Door>();
	}

	public void AddKeyCard(List<Door> pDoors) {
		_accessibleDoors.AddRange(pDoors);
	}

	public bool Contains(Door pDoor) {
		return _accessibleDoors.Find(x => x.gameObject == pDoor.gameObject) != null;
	}
}
