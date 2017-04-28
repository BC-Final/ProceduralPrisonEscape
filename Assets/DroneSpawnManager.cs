using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class DroneSpawnManager : Singleton<DroneSpawnManager> {
	[SerializeField]
	private float _initialSpawnDelay;

	[SerializeField]
	private int _minDroneSpawnAmount;

	[SerializeField]
	private int _maxDroneSpawnAmount;

	//TODO Add Waves
	//TODO Set Max Spawned Drones
	//TODO Track spawned drones
	//TODO Respawn destroyed drones

	private List<DroneSpawner> _droneSpawners = new List<DroneSpawner>();

	private GameObject _player;

	private IEnumerator _spawnCoroutine;

	private void Awake() {
		_spawnCoroutine = spawnDrones();
	}

	private void Start() {
		ShooterAlarmManager.Instance.OnAlarmChange += onAlarmChange;
		_droneSpawners.AddRange(FindObjectsOfType<DroneSpawner>());
		_player = FindObjectOfType<PlayerHealth>().gameObject;
	}

	private void onAlarmChange() {
		if (ShooterAlarmManager.Instance.AlarmIsOn) {
			StartCoroutine(_spawnCoroutine);
		} else {
			StopCoroutine(_spawnCoroutine);
		}
	}

	private IEnumerator spawnDrones() {
		yield return new WaitForSeconds(_initialSpawnDelay);
		Utilities.Vectors.GetClosestObject<DroneSpawner>(_player.transform, _droneSpawners).QueueDroneSpawn(Random.Range(_minDroneSpawnAmount, _minDroneSpawnAmount + 1));
	}
}
