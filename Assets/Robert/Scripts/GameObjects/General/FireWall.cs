using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireWall : MonoBehaviour, IDamageable {

	public bool destroyed;
	public int ID;

	public List<Door> doors;
	protected float health = 10;
	protected FireWallManager _manager;
	private ParticleSystem _particleSystem;


	public virtual void ChangeState(bool nDestroyed)
	{
		destroyed = nDestroyed;
	}

	public void ReceiveDamage(Vector3 pDirection, Vector3 pPoint, float pDamage)
	{
		health -= pDamage;
		if(health <= 0)
		{
			OnDeath();
		}
	}

	void Awake()
	{
		_particleSystem = GetComponentInChildren<ParticleSystem>();
		_particleSystem.enableEmission = false;
		if(doors == null)
		{
			doors = new List<Door>();
		}
	}

	public void AddDoor(Door door)
	{
		if(doors == null)
		{
			Debug.Log("list is null");
			doors = new List<Door>();
		}
		doors.Add(door);
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

	public void SetManager(FireWallManager manager)
	{
		_manager = manager;
	}

	private void OnDeath()
	{
		destroyed = true;
		_particleSystem.enableEmission = true;
		_manager.SendFireWallUpdate(this);
		//Tell all doors im dead
		foreach(Door d in doors)
		{
			d.ChangeState(Door.DoorStatus.Closed);
		}
	}

}
