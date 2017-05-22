using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;
using System.Linq;

public class DroneSpawnManager : Singleton<DroneSpawnManager> {
	[SerializeField]
	private int _minRandomDroneSpawnAmount;

	[SerializeField]
	private int _maxRandomDroneSpawnAmount;


	[SerializeField]
	private float _initialAlarmSpawnDelay;

	[SerializeField]
	private float _alarmWaveSpawnDelay;

	[SerializeField]
	private int _minAlarmDroneSpawnAmount;

	[SerializeField]
	private int _maxAlarmDroneSpawnAmount;

	private List<DroneSpawner> _droneSpawners = new List<DroneSpawner>();

	private List<DronePoint> _dronePoints = new List<DronePoint>();

	private GameObject _player;

	private Coroutine _spawnCoroutine;

	private List<DroneEnemy> _alarmDrones = new List<DroneEnemy>();

	





	private void Start() {
		ShooterAlarmManager.Instance.OnAlarmChange += onAlarmChange;
		_droneSpawners.AddRange(FindObjectsOfType<DroneSpawner>());
		_dronePoints.AddRange(FindObjectsOfType<DronePoint>());
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
		yield return new WaitForSeconds(_initialAlarmSpawnDelay);

		while (ShooterAlarmManager.Instance.AlarmIsOn) {
			Utilities.Vectors.GetClosestObject<DroneSpawner>(_player.transform, _droneSpawners).QueueAlarmDroneSpawn(Random.Range(_minAlarmDroneSpawnAmount, _maxAlarmDroneSpawnAmount + 1));
			yield return new WaitForSeconds(_alarmWaveSpawnDelay);
		}
	}



	public void SpawnReinforcements(int pAmount) {
		List<DronePoint> _availiblePoints = _dronePoints.FindAll(x => x.Occupied == false);

		pAmount = Mathf.Min(pAmount, _availiblePoints.Count);

		for (int i = 0; i < pAmount; ++i) {
			DronePoint chosenPoint = _availiblePoints[Random.Range(0, _availiblePoints.Count - 1)];
			_availiblePoints.RemoveAll(x => x.gameObject == chosenPoint.gameObject);
			DroneSpawner closestSpawner = GetClosestSpawner(chosenPoint.gameObject).GetComponent<DroneSpawner>();
			closestSpawner.QueueReinforcementDroneSpawn(chosenPoint);
		}
	}


	public GameObject GetClosestSpawner(GameObject pObject) {
		return Utilities.Vectors.GetClosestObject<DroneSpawner>(pObject.transform, _droneSpawners).gameObject;
	}


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
