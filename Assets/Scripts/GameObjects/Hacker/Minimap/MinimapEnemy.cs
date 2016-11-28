using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinimapEnemy : MonoBehaviour {

	//static vars
	private static List<MinimapEnemy> _enemies = new List<MinimapEnemy>();

	private int _id;
	private Vector3 _oldPos;
	private Vector3 _newPos;

	private float _lerpTime = 0.5f;
	private float _timeSinceLastUpdate;
	private float _currentLerpTime;

	// Update is called once per frame
	void Update()
	{
		_timeSinceLastUpdate += Time.deltaTime;
		//increment timer once per frame
		_currentLerpTime += Time.deltaTime;
		if (_currentLerpTime > _lerpTime)
		{
			_currentLerpTime = _lerpTime;
		}

		//lerp!
		float perc = _currentLerpTime / _lerpTime;
		transform.position = Vector3.Lerp(_oldPos, _newPos, perc);
	}

	private void SetNewPos(Vector3 nPos)
	{
		_lerpTime = _timeSinceLastUpdate;
		_timeSinceLastUpdate = 0f;
		_currentLerpTime = 0f;
		_oldPos = _newPos;
		_newPos = nPos;
	}
	private void SetNewRotation(float rotation)
	{
		transform.rotation = Quaternion.Euler(0, rotation, 0);
	}
	private void SetHealth(int health)
	{

	}

	//STATIC METHODS
	private static MinimapEnemy GetEnemyByID(int id)
	{
		foreach (MinimapEnemy i in _enemies)
		{
			if (i._id == id)
			{
				return i;
			}
		}
		return null;
	}

	public static void UpdateEnemy(CustomCommands.Update.EnemyUpdate package)
	{
		Vector3 pos = new Vector3(package.x, 1, package.z);
		MinimapEnemy enemy = GetEnemyByID(package.id);
		enemy.SetNewPos(pos);
		enemy.SetNewRotation(package.rotation);
		enemy.SetHealth(package.hpPercent);
	}
}
