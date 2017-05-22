using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforcementTrigger : MonoBehaviour {
	[SerializeField]
	private int _minReinforcementAmount;

	[SerializeField]
	private int _maxReinforcementAmount;

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("RayTrigger")) {
			if (other.GetComponentInParent<PlayerHealth>() != null) {
				DroneSpawnManager.Instance.SpawnReinforcements(Random.Range(_minReinforcementAmount, _maxReinforcementAmount));
			}
		}
	}
}
