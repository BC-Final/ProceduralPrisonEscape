using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Firewall : MonoBehaviour, IDamageable {

	private static List<Firewall> _firewalls;
	private static int _firewallIndex = 0;




	public int ID;
	private bool destroyed;
	private List<Door> _doors; 

	//Graphical Stuff
	protected float health = 10;
	private ParticleSystem _particleSystem;

	public void ChangeState(bool nDestroyed)
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

		ParticleSystem.EmissionModule em = _particleSystem.emission;
		em.enabled = false;
	}

	public void AddDoor(Door door)
	{
		if(_doors == null)
		{
			_doors = new List<Door>();
		}
		_doors.Add(door);
	}

	public bool GetState()
	{
		if (destroyed)
		{
			return true;
		}
		return false;
	}

	public List<Door> GetDoorList()
	{
		return _doors;
	}

	private void OnDeath()
	{
		destroyed = true;
		ParticleSystem.EmissionModule em = _particleSystem.emission;
		em.enabled = true;
		ShooterPackageSender.GetInstance().SendFireWallUpdate(this);
		//Tell all doors im dead
		foreach(Door d in _doors)
		{
			d.ChangeState(Door.DoorState.Closed);
		}
	}

	//Static methods

	private static Firewall GetFireWallByID(int ID)
	{
		foreach (Firewall f in _firewalls)
		{
			if (f.ID == ID)
			{
				return f;
			}
		}
		return null;
	}

	private static List<Firewall> InitGetAllFireWallsInLevel()
	{
		List<Firewall> allFireWalls = new List<Firewall>();
		Firewall[] fireWallArray = FindObjectsOfType<Firewall>();
		for (int i = 0; i < fireWallArray.Length; i++)
		{
			fireWallArray[i].ID = _firewallIndex;
			_firewallIndex++;
			allFireWalls.Add(fireWallArray[i]);
		}
		return allFireWalls;
	}

	public static List<Firewall> GetFirewallList()
	{
		if (_firewalls == null)
		{
			_firewalls = InitGetAllFireWallsInLevel();
		}
		return _firewalls;
	}

}
