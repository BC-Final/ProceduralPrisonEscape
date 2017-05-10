using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;
using System.Linq;

public class DroneSpawnManager : Singleton<DroneSpawnManager> {
	[SerializeField]
	private float _initialSpawnDelay;

	[SerializeField]
	private int _minDroneSpawnAmount;

	[SerializeField]
	private int _maxDroneSpawnAmount;

	//TODO Add Waves
	//TODO Set Max Spawned Drones
	//TODO Respawn destroyed drones

	private List<DroneSpawner> _droneSpawners = new List<DroneSpawner>();

	private GameObject _player;

	private Coroutine _spawnCoroutine;

	private void Start() {
		ShooterAlarmManager.Instance.OnAlarmChange += onAlarmChange;
		_droneSpawners.AddRange(FindObjectsOfType<DroneSpawner>());
		_player = FindObjectOfType<PlayerHealth>().gameObject;
	}

	private void onAlarmChange() {
		if (ShooterAlarmManager.Instance.AlarmIsOn) {
			_spawnCoroutine = StartCoroutine(spawnDrones());
		} else {
			if (_spawnCoroutine != null) {
				StopCoroutine(_spawnCoroutine);
				_spawnCoroutine = null;
			}
		}
	}

	private IEnumerator spawnDrones() {
		yield return new WaitForSeconds(_initialSpawnDelay);
		Utilities.Vectors.GetClosestObject<DroneSpawner>(_player.transform, _droneSpawners).QueueDroneSpawn(Random.Range(_minDroneSpawnAmount, _maxDroneSpawnAmount + 1));
	}

	public GameObject GetClosestSpawner(GameObject pObject) {
		return Utilities.Vectors.GetClosestObject<DroneSpawner>(pObject.transform, _droneSpawners).gameObject;
	}


	private List<DroneEnemy> _alarmDrones = new List<DroneEnemy>();

	public void RegisterAlarmDrone(GameObject pDrone) {
		_alarmDrones.Add(pDrone.GetComponent<DroneEnemy>());
		_alarmDrones.Last().GetComponent<DroneEnemy>().AddToDestroyEvent(g => removeAlarmDrone(g));
	}

	private void removeAlarmDrone(GameObject pDrone) {
		_alarmDrones.RemoveAll(x => x.gameObject == pDrone);
	}



	public void NotifySeesPlayer() {
		_alarmDrones.ForEach(x => x.SetFollowPlayer());
	}

	public void NotifyLostPlayer() {
		bool lost = true;

		foreach (DroneEnemy d in _alarmDrones) {
			lost = lost && !d.SeesTarget;
		}

		if (lost) {
			_alarmDrones.ForEach(x => x.SetSearchPlayer());
		}
	}
}
