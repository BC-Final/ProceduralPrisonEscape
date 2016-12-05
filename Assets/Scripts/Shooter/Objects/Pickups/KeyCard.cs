using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net.Sockets;

public class KeyCard : MonoBehaviour, IInteractable, INetworked {
	private static List<KeyCard> _keyCards = new List<KeyCard>();
	public static List<KeyCard> GetKeyCardList() { return _keyCards; }

	[SerializeField]
	private List<ShooterDoor> _doors;

	private int _id;
	public int Id {
		get {
			if (_id == 0) {
				_id = IdManager.RequestId();
			}

			return _id;
		}
	}

	public void Initialize () {
		ShooterPackageSender.SendPackage(new CustomCommands.Creation.Items.KeyCardCreation(Id, transform.position.x, transform.position.z));
	}

	private void Awake() {
		_keyCards.Add(this);
		ShooterPackageSender.RegisterNetworkObject(this);
	}

	private void OnDestroy() {
		ShooterPackageSender.SendPackage(new CustomCommands.Update.Items.ItemUpdate(_id, true, this.transform.position));
		_keyCards.Remove(this);
		ShooterPackageSender.UnregisterNetworkedObject(this);
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
