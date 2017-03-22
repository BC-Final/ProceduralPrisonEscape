﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net.Sockets;

public class KeyCard : MonoBehaviour, IInteractable {
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

    [SerializeField]
    public Color keyColor;

	public void Initialize () {
        int[] intArray;
        List<int> tempArray = new List<int>();
        foreach (ShooterDoor door in _doors)
        {
            tempArray.Add(door.Id);
        }
        intArray = tempArray.ToArray();
        ShooterPackageSender.SendPackage(new NetworkPacket.Create.KeyCard(Id, intArray, transform.position, keyColor.r, keyColor.g, keyColor.b));
	}

	private void Awake() {
		_keyCards.Add(this);
        gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", keyColor);
	}

	private void OnDestroy() {
		ShooterPackageSender.SendPackage(new NetworkPacket.Update.Icon(_id, true));
		_keyCards.Remove(this);
		//ShooterPackageSender.UnregisterNetworkedObject(this);
	}

	private void Start() {
		foreach (ShooterDoor d in _doors) {
			//d.SetRequireKeyCard();
		}
	}

	public void Interact() {
		//FindObjectOfType<Inventory>().AddKeyCard(_doors);

		FMOD.Studio.EventInstance ins = FMODUnity.RuntimeManager.CreateInstance("event:/PE_shooter/PE_shooter_pickkeycard");
		FMODUnity.RuntimeManager.AttachInstanceToGameObject(ins, transform, GetComponent<Rigidbody>());
		ins.start();

		Destroy(gameObject);
	}

	//public void SetDoors(List<ShooterDoor> pDoors) {
	//	_doors = pDoors;
	//}
}
