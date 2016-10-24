using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireWall : MonoBehaviour {

	private List<Door> _doors;

	private int health = 10;

	public bool destroyed;

	void Start()
	{
		_doors = new List<Door>();
	}

	public bool GetPermission()
	{
		if (destroyed)
		{
			return true;
		}
		return false;
	}

	public void GetDmg(int dmg = 10)
	{
		destroyed = true;
		OnDeath();
	}

	private void OnDeath()
	{
		//Tell all doors im dead
		foreach(Door d in _doors)
		{
			d.ChangeState(Door.DoorStatus.Closed);
		}
	}

}
