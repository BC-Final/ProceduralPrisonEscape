using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;

public class ShooterFirewall : MonoBehaviour, IDamageable, INetworked {
	private static List<ShooterFirewall> _firewalls = new List<ShooterFirewall>();
	public static List<ShooterFirewall> GetFirewallList() { return _firewalls; }

	private int _id;
	public int Id {
		get {
			if (_id == 0) {
				_id = IdManager.RequestId();
			}

			return _id;
		}
	}


	private bool _destroyed;

	//Graphical Stuff
	private ParticleSystem _particleSystem;

	public void Initialize () {
		//ShooterPackageSender.SendPackage(new CustomCommands.Creation.FireWallCreation(Id, transform.position.x, transform.position.z, _destroyed));
	}

	private void Awake() {
		_particleSystem = GetComponentInChildren<ParticleSystem>();
		ParticleSystem.EmissionModule em = _particleSystem.emission;
		em.enabled = false;

		_firewalls.Add(this);

		ShooterPackageSender.RegisterNetworkObject(this);
	}

	private void OnDestroy() {
		_firewalls.Remove(this);
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}

	public void ReceiveDamage (Vector3 pDirection, Vector3 pPoint, float pDamage) {
		if(!_destroyed) {
			FMOD.Studio.EventInstance startDest = FMODUnity.RuntimeManager.CreateInstance("event:/PE_envi/PE_envi_firewall_destroy_start");
			FMODUnity.RuntimeManager.AttachInstanceToGameObject(startDest, transform, GetComponent<Rigidbody>());
			startDest.start();

			FMOD.Studio.EventInstance loopDest = FMODUnity.RuntimeManager.CreateInstance("event:/PE_envi/PE_envi_firewall_destroy_loop");
			FMODUnity.RuntimeManager.AttachInstanceToGameObject(loopDest, transform, GetComponent<Rigidbody>());
			loopDest.start();

			_destroyed = true;

			ParticleSystem.EmissionModule em = _particleSystem.emission;
			em.enabled = true;

			//ShooterPackageSender.SendPackage(new CustomCommands.Update.FireWallUpdate(Id, _destroyed));
		}
	}
}
