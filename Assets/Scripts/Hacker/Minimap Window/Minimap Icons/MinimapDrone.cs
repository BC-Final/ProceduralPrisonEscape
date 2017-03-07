using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinimapDrone : MonoBehaviour {
	private HackerDrone _associatedDrone;

	public HackerDrone AssociatedDrone {
		set { _associatedDrone = value; }
	}

	private Vector3 _oldPos;
	private Vector3 _newPos;

	private Quaternion _oldRot;
	private Quaternion _newRot;

	private float _lastUpdateTime = 0.0f;

	private float _currentLerpTime = 0.0f;
	private float _lerpTime = 0.5f;


	private void Update () {
		_currentLerpTime += Time.deltaTime;
		if (_currentLerpTime > _lerpTime) {
			_currentLerpTime = _lerpTime;
		}

		if (_lerpTime == 0.0f) {
			_lerpTime = 0.5f;
		}

		float perc = _currentLerpTime / _lerpTime;

		transform.position = Vector3.Lerp(_oldPos, _newPos, perc);
		transform.rotation = Quaternion.Slerp(_oldRot, _newRot, perc);
	}

	public void InitialTransform (Vector3 pPos, float pRot) {
		_lastUpdateTime = Time.time;

		_oldPos = pPos;
		_newPos = pPos;

		_oldRot = Quaternion.Euler(0, pRot, 0);
		_newRot = Quaternion.Euler(0, pRot, 0);
	}

	public void UpdateTransform (Vector3 pPos, float pRot) {
		_lerpTime = Time.time - _lastUpdateTime;
		_lastUpdateTime = Time.time;
		_currentLerpTime = 0f;

		_oldPos = transform.position;
		_newPos = pPos;

		_oldRot = transform.rotation;
		_newRot = Quaternion.Euler(0, pRot, 0);
	}

	public void HealthChanged (float pHealth) {
		if (pHealth <= 0) {
			GetComponent<Renderer>().material.SetColor("_Color", Color.gray);
		}
	}
	/*
	private void SetHealth (int nHealth) {
		health = nHealth;
		if (health <= 0) {
			GetComponent<Renderer>().material.SetColor("_Color", Color.gray);
		}
	}
	*/


	//STATIC METHODS
	/*
	private static MinimapEnemy GetEnemyByID (int id) {
		foreach (MinimapEnemy i in _enemies) {
			if (i._id == id) {
				return i;
			}
		}
		return null;
	}

	public static void UpdateEnemy (CustomCommands.Update.EnemyUpdate package) {
		Vector3 pos = new Vector3(package.x, 1, package.z);

		if (GetEnemyByID(package.id) != null) {
			MinimapEnemy enemy = GetEnemyByID(package.id);
			enemy.UpdateTransform(pos / MinimapManager.GetInstance().scale, package.rotation);
			enemy.SetHealth(package.hpPercent);
		} else {
			MinimapEnemy enemy = MinimapManager.GetInstance().CreateMinimapEnemy(pos);
			enemy._id = package.id;
			enemy.InitialTransform(pos / MinimapManager.GetInstance().scale, package.rotation);
			enemy.SetHealth(package.hpPercent);
			_enemies.Add(enemy);
		}
	}
	*/
}
