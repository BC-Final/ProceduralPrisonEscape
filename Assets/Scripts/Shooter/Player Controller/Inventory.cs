using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
	private List<ShooterDoor> _accessibleDoors;
	private HUDCardHolder _cardHolder;

	private void Awake() {
		_accessibleDoors = new List<ShooterDoor>();
	}

	public void AddKeyCard(List<ShooterDoor> pDoors, Color color) {
		_accessibleDoors.AddRange(pDoors);
		if (_cardHolder) { _cardHolder.AddCard(color); } else { _cardHolder = GameObject.FindObjectOfType<HUDCardHolder>(); _cardHolder.AddCard(color); }
		ShooterPackageSender.SendPackage(new NetworkPacket.Update.KeyCardCollected(color));
	}

	public bool Contains(ShooterDoor pDoor) {
		return _accessibleDoors.Find(x => x.gameObject == pDoor.gameObject) != null;
	}
}
