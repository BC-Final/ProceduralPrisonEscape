using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class DroneSpawner : MonoBehaviour {
	[SerializeField]
	private float _spawnDelay;

	private int _queuedDrones = 0;
	private Coroutine _spawnCoroutine;


	public void QueueDroneSpawn(int pAmount) {
		if (_spawnCoroutine == null) {
			_queuedDrones = pAmount;
			_spawnCoroutine = StartCoroutine(spawnQueuedDrones());
			ShooterAlarmManager.Instance.OnAlarmChange += abortSpawns;
		} else {
			_queuedDrones += pAmount;
		}
	}

	private void abortSpawns() {
		_queuedDrones = 0;
		StopCoroutine(_spawnCoroutine);
		_spawnCoroutine = null;
		ShooterAlarmManager.Instance.OnAlarmChange -= abortSpawns;
	}

	private IEnumerator spawnQueuedDrones() {
		while (_queuedDrones > 0) {
			_queuedDrones--;

			GameObject drone = Instantiate(ShooterReferenceManager.Instance.Drone, transform.position, transform.rotation);
			drone.GetComponent<DroneEnemy>().AlarmResponder = true;
			DroneSpawnManager.Instance.RegisterAlarmDrone(drone);

			if (_queuedDrones > 0) {
				yield return new WaitForSeconds(_spawnDelay);
			}
		}

		ShooterAlarmManager.Instance.OnAlarmChange -= abortSpawns;
	}
}
