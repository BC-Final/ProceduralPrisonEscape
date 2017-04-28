using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryObjective : AbstractObjective, IInteractable {
	private bool _canPickup = true;
	private Rigidbody _rigidbody;

	private void Start () {
		_rigidbody = GetComponent<Rigidbody>();
	}

	public bool IsInSlot () {
		return !_canPickup;
	}

	public void Disable () {
		_canPickup = false;
		_rigidbody.isKinematic = true;
		_rigidbody.useGravity = false;
		GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
	}

	public void Interact () {
		if (_canPickup) {
			_rigidbody.isKinematic = true;
			_rigidbody.useGravity = false;
			GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;


			FindObjectOfType<HeavyObjectHolder>().Pickup(this);
		}
	}

	public void Drop () {
		GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
		_rigidbody.isKinematic = false;
		_rigidbody.useGravity = true;
	}
}
