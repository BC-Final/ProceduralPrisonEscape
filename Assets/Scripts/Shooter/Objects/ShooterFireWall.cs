using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;

public class ShooterFireWall : MonoBehaviour, IDamageable, IShooterNetworked
{
	[SerializeField]
	private List<ShooterDoor> _doors;
	[SerializeField]
	private GameObject _particleSystem;
	private bool _active = true;

	//IShooterNetworked implementation

	private ShooterNetworkId _id = new ShooterNetworkId();
	public ShooterNetworkId Id { get { return _id; } }

	public void Initialize()
	{
		foreach(ShooterDoor door in _doors)
		{
			ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.Door.Other.DisableDoorOptions(door.Id));
			ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.Firewall.Creation(Id, this.transform.position.x, this.transform.position.z, _active));
		}
	}

	//IDamageable implementation

	private Faction _faction = Faction.Neutral;
	public Faction Faction { get { return _faction; } }

	public GameObject GameObject { get { return gameObject; } }

	float _currentHealth = 100f;

	public void ReceiveDamage(Transform pSource, Vector3 pHitPoint, float pDamage, float pForce)
	{
		if (_currentHealth > 0.0f)
		{
			_currentHealth -= pDamage;
			
			if (_currentHealth <= 0.0f)
			{
				OnDestoryed();
			}
		}
	}

	private void Awake()
	{
		ShooterPackageSender.RegisterNetworkObject(this);
	}

	private void OnDestoryed()
	{
		Debug.Log("BOOM");
		_active = false;
		ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.Firewall.sUpdate(_id, _active));
		_particleSystem.SetActive(true);
		foreach (ShooterDoor door in _doors)
		{
			ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.Door.Other.EnableDoorOptions(door.Id));
		}
	}
}

//	private int _id;
//	public int Id {
//		get {
//			if (_id == 0) {
//				_id = IdManager.RequestId();
//			}

//			return _id;
//		}
//	}


//	private bool _destroyed;

//	//Graphical Stuff
//	private ParticleSystem _particleSystem;

//	public void Initialize () {
//		//ShooterPackageSender.SendPackage(new CustomCommands.Creation.FireWallCreation(Id, transform.position.x, transform.position.z, _destroyed));
//	}

//	private void Awake() {
//		_particleSystem = GetComponentInChildren<ParticleSystem>();
//		ParticleSystem.EmissionModule em = _particleSystem.emission;
//		em.enabled = false;

//		_firewalls.Add(this);

//		ShooterPackageSender.RegisterNetworkObject(this);
//	}

//	private void OnDestroy() {
//		_firewalls.Remove(this);
//		ShooterPackageSender.UnregisterNetworkedObject(this);
//	}

//	public void ReceiveDamage (Vector3 pDirection, Vector3 pPoint, float pDamage) {
//		if(!_destroyed) {
//			FMOD.Studio.EventInstance startDest = FMODUnity.RuntimeManager.CreateInstance("event:/PE_envi/PE_envi_firewall_destroy_start");
//			FMODUnity.RuntimeManager.AttachInstanceToGameObject(startDest, transform, GetComponent<Rigidbody>());
//			startDest.start();

//			FMOD.Studio.EventInstance loopDest = FMODUnity.RuntimeManager.CreateInstance("event:/PE_envi/PE_envi_firewall_destroy_loop");
//			FMODUnity.RuntimeManager.AttachInstanceToGameObject(loopDest, transform, GetComponent<Rigidbody>());
//			loopDest.start();

//			_destroyed = true;

//			ParticleSystem.EmissionModule em = _particleSystem.emission;
//			em.enabled = true;

//			//ShooterPackageSender.SendPackage(new CustomCommands.Update.FireWallUpdate(Id, _destroyed));
//		}
//	}
//}
