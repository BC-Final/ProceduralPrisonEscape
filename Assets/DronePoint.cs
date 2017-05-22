using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePoint : MonoBehaviour {
	[SerializeField]
	private bool _spawnDroneOnStart;

	private PatrolRoute _patrolRoute;
	public PatrolRoute Route { get { return _patrolRoute; } }

	private DroneEnemy _occupant;

	public bool Occupied { get { return _occupant != null; } }

	private void Start() {
		_patrolRoute = GetComponentInParent<PatrolRoute>();

		if (_spawnDroneOnStart) {
			SetOccupant(Instantiate(ShooterReferenceManager.Instance.Drone, transform.position, transform.rotation).GetComponent<DroneEnemy>());
		}
	}

	public void SetOccupant(DroneEnemy pOccupant) {
		if (_occupant == null) {
			_occupant = pOccupant;
			_occupant.StartPoint = this;

			if (_patrolRoute != null) {
				_occupant.Route = _patrolRoute;
			}

			_occupant.AddToDestroyEvent((g) => _occupant = null);
		}
	}

	private void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position, Vector3.one * 0.25f);

		if (!Occupied) {
			if (_spawnDroneOnStart) {
				Gizmos.color = Color.blue;
			} else {
				Gizmos.color = Color.grey;
			}
		} else {
			Gizmos.color = Color.green;
		}

		Gizmos.DrawSphere(transform.position + Vector3.up * 1, 0.5f);	
	}
}
