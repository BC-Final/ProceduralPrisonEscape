using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[SelectionBase]
public class DroneSpawner : MonoBehaviour {
	[SerializeField]
	private float _alarmSpawnDelay;

	[SerializeField]
	private float _reinforcementSpawnDelay;

	private int _queuedAlarmDrones = 0;
	private Coroutine _alarmSpawnCoroutine;

	private List<DronePoint> _queuedReinforcementDrones = new List<DronePoint>();
	private Coroutine _reinforcementSpawnCoroutine;

	private void Start() {
		ShooterAlarmManager.Instance.OnAlarmChange += abortAlarmSpawns;
	}

	public void QueueAlarmDroneSpawn(int pAmount) {
		if (_alarmSpawnCoroutine == null) {
			_queuedAlarmDrones = pAmount;
			_alarmSpawnCoroutine = StartCoroutine(spawnQueuedAlarmDrones());
		} else {
			_queuedAlarmDrones += pAmount;
		}
	}

	public void QueueReinforcementDroneSpawn(DronePoint pPoint) {
		_queuedReinforcementDrones.Add(pPoint);

		if (_reinforcementSpawnCoroutine == null) {
			_reinforcementSpawnCoroutine = StartCoroutine(spawnQueuedReinforcementDrones());
		}
	}

	private void abortAlarmSpawns() {
		_queuedAlarmDrones = 0;

		if (_alarmSpawnCoroutine != null) {
			StopCoroutine(_alarmSpawnCoroutine);
			_alarmSpawnCoroutine = null;
		}
	}

	private IEnumerator spawnQueuedAlarmDrones() {
		while (_queuedAlarmDrones > 0) {
			_queuedAlarmDrones--;

			GameObject drone = Instantiate(ShooterReferenceManager.Instance.Drone, transform.position, transform.rotation);
			drone.GetComponent<DroneEnemy>().AlarmResponder = true;
			DroneSpawnManager.Instance.RegisterAlarmDrone(drone);
			yield return new WaitForSeconds(_alarmSpawnDelay);
		}

		_alarmSpawnCoroutine = null;
	}

	private IEnumerator spawnQueuedReinforcementDrones() {
		while (_queuedReinforcementDrones.Count > 0) {
			GameObject drone = Instantiate(ShooterReferenceManager.Instance.Drone, transform.position, transform.rotation);
			_queuedReinforcementDrones.First().SetOccupant(drone.GetComponent<DroneEnemy>());
			_queuedReinforcementDrones.RemoveAt(0);
			yield return new WaitForSeconds(_reinforcementSpawnDelay);
		}

		_reinforcementSpawnCoroutine = null;
	}
}
