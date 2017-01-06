using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinimapEnemy : MonoBehaviour {

	//static vars
	private static List<MinimapEnemy> _enemies = new List<MinimapEnemy>();

	public int health;
	private int _id;
	private Vector3 _oldPos;
	private Vector3 _newPos;

	private float _lerpTime = 0.1f;
	private float _timeSinceLastUpdate = 0.1f;
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

	public void InitialPosition (Vector3 pPosition) {
		_oldPos = pPosition;
		_newPos = pPosition;
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

	private void SetHealth(int nHealth)
	{
		health = nHealth;
        if(health <= 0)
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.gray);
        }
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
		if (GetEnemyByID(package.id) != null)
		{	
			MinimapEnemy enemy = GetEnemyByID(package.id);
			enemy.SetNewPos(pos / MinimapManager.GetInstance().scale);
			enemy.SetNewRotation(package.rotation);
			enemy.SetHealth(package.hpPercent);
		}
		else
		{
			MinimapEnemy enemy = MinimapManager.GetInstance().CreateMinimapEnemy(pos);
			enemy._id = package.id;
            enemy.SetNewPos(pos / MinimapManager.GetInstance().scale);
			enemy.SetNewRotation(package.rotation);
			enemy.SetHealth(package.hpPercent);
			_enemies.Add(enemy);
			enemy.InitialPosition(pos / MinimapManager.GetInstance().scale);
		}
	}

    //public static void UpdateEnemy(CustomCommands.Update.CameraUpdate package)
    //{
    //    Vector3 pos = new Vector3(package.x, 1, package.z);
    //    if (GetEnemyByID(package.id) != null)
    //    {
    //        MinimapEnemy enemy = GetEnemyByID(package.id);
    //        enemy.SetNewPos(pos / MinimapManager.GetInstance().scale);
    //        enemy.SetNewRotation(package.rotation);
    //        enemy.SetHealth(package.hpPercent);
    //    }
    //    else
    //    {
    //        GameObject gameObject = (GameObject)Instantiate(HackerReferenceManager.Instance.CameraIcon, pos / MinimapManager.GetInstance().scale, Quaternion.Euler(0, 0, 0));
    //        MinimapEnemy enemy = gameObject.GetComponent<MinimapEnemy>();
    //        enemy._id = package.id;
    //        enemy.SetNewPos(pos / MinimapManager.GetInstance().scale);
    //        enemy.SetNewRotation(package.rotation);
    //        enemy.SetHealth(package.hpPercent);
    //        _enemies.Add(enemy);
    //        enemy.InitialPosition(pos / MinimapManager.GetInstance().scale);
    //    }
    //}
    //
    //public static void UpdateEnemy(CustomCommands.Update.TurretUpdate package)
    //{
    //    Vector3 pos = new Vector3(package.x, 1, package.z);
    //    if (GetEnemyByID(package.id) != null)
    //    {
    //        MinimapEnemy enemy = GetEnemyByID(package.id);
    //        enemy.SetNewPos(pos / MinimapManager.GetInstance().scale);
    //        enemy.SetNewRotation(package.rotation);
    //        enemy.SetHealth(package.hpPercent);
    //    }
    //    else
    //    {
    //        GameObject gameObject = (GameObject)Instantiate(HackerReferenceManager.Instance.TurretIcon, pos / MinimapManager.GetInstance().scale, Quaternion.Euler(0, 0, 0));
    //        MinimapEnemy enemy = gameObject.GetComponent<MinimapEnemy>();
    //        enemy._id = package.id;
    //        enemy.SetNewPos(pos / MinimapManager.GetInstance().scale);
    //        enemy.SetNewRotation(package.rotation);
    //        enemy.SetHealth(package.hpPercent);
    //        _enemies.Add(enemy);
    //        enemy.InitialPosition(pos / MinimapManager.GetInstance().scale);
    //    }
    //}
}
