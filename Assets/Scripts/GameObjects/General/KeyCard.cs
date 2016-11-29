using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class KeyCard : MonoBehaviour, IInteractable, INetworked {
	private static List<KeyCard> _keyCards = new List<KeyCard>();
	public static List<KeyCard> GetKeyCardList() { return _keyCards; }

	[SerializeField]
	private List<ShooterDoor> _doors;

	private int _id;
	public int Id { get { return _id; } }

	public void Initialize () {
		_id = IdManager.RequestId();
	}

	private void Awake() {
		Initialize();
		_keyCards.Add(this);
	}

	private void OnDestroy() {
		ShooterPackageSender.SendPackage(new CustomCommands.Update.Items.ItemUpdate(_id, true));
		
		_keyCards.Remove(this);
	}

	private void Start() {
		foreach (ShooterDoor d in _doors) {
			d.SetRequireKeyCard();
		}
	}

	public void Interact() {
		FindObjectOfType<Inventory>().AddKeyCard(_doors);
		Destroy(gameObject);
	}

	public void SetDoors(List<ShooterDoor> pDoors) {
		_doors = pDoors;
	}
}
