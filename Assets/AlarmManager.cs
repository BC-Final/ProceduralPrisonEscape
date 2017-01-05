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



	private bool _alarmActive;


	private Timers.Timer _waveTimer;
	private Timers.Timer _spawnTimer;

	private void Start () {
		_waveTimer = Timers.CreateTimer().ResetOnFinish().SetCallback(() => spawnWave());
		_spawnTimer = Timers.CreateTimer().ResetOnFinish().SetCallback(() => spawnEnemy());
	}


	private void spawnWave () {
		_waveTimer.SetTime(Random.Range(_minWaveDelay, _maxWaveDelay)).Start();

		spawnEnemy();
	}


	private void spawnEnemy () {
		_spawnTimer.SetTime(Random.Range(_minEnemySpawnDelay, _maxEnemySpawnDelay)).Start();

		//TODO Spawn an enemy
	}




	public void ActivateAlarm () {
		_alarmActive = true;

		_waveTimer.SetTime(Random.Range(_minWaveDelay, _maxWaveDelay)).Start();

		//TODO Send Min Alarm Time
		//TODO Send Update
	}

	public void DeactivateAlarm () {
		_alarmActive = false;
		//TODO Send Update
	}

	public void SetAlarm (bool pState) {
		_alarmActive = pState;
	}
}
