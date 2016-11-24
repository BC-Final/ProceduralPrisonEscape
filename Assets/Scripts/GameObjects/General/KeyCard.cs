using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class KeyCard : MonoBehaviour, IInteractable {
	private static List<KeyCard> _keyCards = new List<KeyCard>();
	public static List<KeyCard> GetKeyCards() { return _keyCards; }
	public static int _availibleId = 0;

	[SerializeField]
	private List<Door> _doors;

	public int _id;

	private void Awake() {
		_id = _availibleId;
		_availibleId++;

		ShooterPackageSender.SendPackage(new CustomCommands.Update.Items.ItemUpdate(_id, false));

		_keyCards.Add(this);
	}

	private void OnDestroy() {
		ShooterPackageSender.SendPackage(new CustomCommands.Update.Items.ItemUpdate(_id, true));
		_keyCards.Remove(this);
	}

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
