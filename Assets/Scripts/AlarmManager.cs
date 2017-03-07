using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Gamelogic.Extensions;
using DG.Tweening;

public class AlarmManager : Singleton<AlarmManager> {
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

	private FMOD.Studio.EventInstance _alarmSound;

	private void Start () {
		_waveTimer = Timers.CreateTimer().SetCallback(() => spawnWave()).SetMinMaxTime(_minWaveDelay, _maxWaveDelay);
		_spawnTimer = Timers.CreateTimer().ResetOnFinish().SetCallback(() => spawnEnemy()).SetMinMaxTime(_minEnemySpawnDelay, _maxEnemySpawnDelay);

		_alarmSound = FMODUnity.RuntimeManager.CreateInstance("event:/PE_shooter/PE_shooter_alarm_loop");
	}


	private void spawnWave () {
		_waveTimer.SetLoop(-1).Start();
		//Debug.Log("Started Wave");
		_spawnTimer.SetLoop(Random.Range(_minWaveEnemyCount, _maxWaveEnemyCount + 1)).Start();
	}


	private void spawnEnemy () {

		Vector3 playerPos = FindObjectOfType<PlayerMotor>().transform.position;

		List<DroneSpawner> droneSpawners = DroneSpawner.DroneSpawners;

		droneSpawners.Sort((x, y) => Vector3.Distance(x.transform.position, playerPos).CompareTo(Vector3.Distance(y.transform.position, playerPos)));

		//TODO Make the 3 modifiable
		int index = Random.Range(0, Mathf.Min(3+1, droneSpawners.Count));

		GameObject drone = GameObject.Instantiate(_enemyPrefab, droneSpawners[index].transform.position, droneSpawners[index].transform.rotation);
		drone.GetComponent<DroneEnemy>().SetTarget();

		//Debug.Log("Spawned Enemy");
	}




	public void ActivateAlarm () {
		_alarmActive = true;

		GetComponent<ShooterGamestateManager>().TriggerAlarm();

		spawnWave();

		_alarmSound.start();

		//TODO Send Min Alarm Time
		//TODO Send Update
	}

	public void DeactivateAlarm () {
		Debug.Log("Disabled Alarm");

		_alarmActive = false;

		GetComponent<ShooterGamestateManager>().DisableAlarm();

		_waveTimer.Stop();
		_spawnTimer.Stop();

		_alarmSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		//TODO Send Update
	}

	/*
	public void SetAlarm (bool pState) {
		_alarmActive = pState;
	}
	*/
}
