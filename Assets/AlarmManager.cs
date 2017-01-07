using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Gamelogic.Extensions;
using DG.Tweening;

public class AlarmManager : Singleton<ShooterGamestateManager> {
	[SerializeField]
	private float _minAlarmTime;



	[SerializeField]
	private float _minEnemySpawnDelay;

	[SerializeField]
	private float _maxEnemySpawnDelay;



	[SerializeField]
	private float _minWaveDelay;

	[SerializeField]
	private float _maxWaveDelay;



	[SerializeField]
	private int _minWaveEnemyCount;

	[SerializeField]
	private int _maxWaveEnemyCount;

	[SerializeField]
	private GameObject _enemyPrefab;



	private bool _alarmActive;


	private Timers.Timer _waveTimer;
	private Timers.Timer _spawnTimer;

	private void Start () {
		_waveTimer = Timers.CreateTimer().SetCallback(() => spawnWave()).SetMinMaxTime(_minWaveDelay, _maxWaveDelay);
		_spawnTimer = Timers.CreateTimer().ResetOnFinish().SetCallback(() => spawnEnemy()).SetMinMaxTime(_minEnemySpawnDelay, _maxEnemySpawnDelay);
	}


	private void spawnWave () {
		_waveTimer.SetLoop(-1).Start();

		spawnEnemy();
	}


	private void spawnEnemy () {
		_spawnTimer.SetLoop(Random.Range(_minWaveEnemyCount, _maxWaveEnemyCount+1)).Start();

		Vector3 playerPos = FindObjectOfType<PlayerMotor>().transform.position;

		List<DroneSpawner> droneSpawners = DroneSpawner.DroneSpawners;

		droneSpawners.Sort((x, y) => Vector3.Distance(x.transform.position, playerPos).CompareTo(Vector3.Distance(y.transform.position, playerPos)));

		//TODO Make the 3 modifiable
		int index = Random.Range(0, Mathf.Min(3+1, droneSpawners.Count));

		GameObject.Instantiate(_enemyPrefab, droneSpawners[index].transform.position, droneSpawners[index].transform.rotation);
	}




	public void ActivateAlarm () {
		_alarmActive = true;

		_waveTimer.SetTime(Random.Range(_minWaveDelay, _maxWaveDelay)).Start();

		//TODO Send Min Alarm Time
		//TODO Send Update
	}

	public void DeactivateAlarm () {
		_alarmActive = false;
		_waveTimer.Stop();
		_spawnTimer.Stop();
		//TODO Send Update
	}

	public void SetAlarm (bool pState) {
		_alarmActive = pState;
	}
}
