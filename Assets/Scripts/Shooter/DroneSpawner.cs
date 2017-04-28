using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class DroneSpawner : MonoBehaviour {
	[SerializeField]
	private float _spawnDelay;

	private int _queuedDrones = 0;
	private IEnumerator _spawnCoroutine;

	private void Awake() {
		_spawnCoroutine = spawnQueuedDrones();
	}

	public void QueueDroneSpawn(int pAmount) {
		_queuedDrones = pAmount;
		StartCoroutine(_spawnCoroutine);
		ShooterAlarmManager.Instance.OnAlarmChange += () => StopCoroutine(_spawnCoroutine);
	}

	private IEnumerator spawnQueuedDrones() {
		while (_queuedDrones > 0) {
			_queuedDrones--;
			Instantiate(ShooterReferenceManager.Instance.Drone, transform.position, transform.rotation);

			if (_queuedDrones > 0) {
				yield return new WaitForSeconds(_spawnDelay);
			}
		}

		ShooterAlarmManager.Instance.OnAlarmChange -= () => StopCoroutine(_spawnCoroutine);
	}
}
