using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShooterFireWall : MonoBehaviour, IDamageable, INetworked {
	private static List<ShooterFireWall> _firewalls = new List<ShooterFireWall>();
	public static List<ShooterFireWall> GetFirewallList() { return _firewalls; }

	private int _id;
	public int Id { get { return _id; } }

	public void Initialize () {
		_id = IdManager.RequestId();
	}

	private bool destroyed;
	private List<ShooterDoor> _doors;

	//Graphical Stuff
	private ParticleSystem _particleSystem;

	private void Awake() {
		Initialize();

		_particleSystem = GetComponentInChildren<ParticleSystem>();
		ParticleSystem.EmissionModule em = _particleSystem.emission;
		em.enabled = false;

		_firewalls.Add(this);
	}

	private void OnDestroy() {
		_firewalls.Remove(this);
	}



	public void ReceiveDamage (Vector3 pDirection, Vector3 pPoint, float pDamage) {
		destroyed = true;

		ParticleSystem.EmissionModule em = _particleSystem.emission;
		em.enabled = true;

		ShooterPackageSender.GetInstance().SendFireWallUpdate(this);

		//Tell all doors im dead
		foreach (ShooterDoor d in _doors) {
			d.ChangeState(DoorState.Closed);
		}
	}

	public void AddDoor(ShooterDoor door) {
		if (_doors == null) {
			_doors = new List<ShooterDoor>();
		}

		_doors.Add(door);
	}

	public bool GetState() {
		return destroyed;
	}

	public List<ShooterDoor> GetDoorList() {
		return _doors;
	}
}
