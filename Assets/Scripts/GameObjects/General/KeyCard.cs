using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class KeyCard : MonoBehaviour, IInteractable {
	[SerializeField]
	private List<Door> _doors;

	private void Start() {
		foreach (Door d in _doors) {
			d.SetRequireKeyCard();
		}
	}

	public void Interact() {
		FindObjectOfType<Inventory>().AddKeyCard(_doors);
		Destroy(gameObject);
	}

	public void SetDoors(List<Door> pDoors) {
		_doors = pDoors;
	}
}
