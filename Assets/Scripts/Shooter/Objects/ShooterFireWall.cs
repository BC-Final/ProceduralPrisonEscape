using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;

public class ShooterFireWall : MonoBehaviour, IDamageable, IShooterNetworked {
	private static List<ShooterFireWall> _firewalls = new List<ShooterFireWall>();
	public static List<ShooterFireWall> GetFirewallList() { return _firewalls; }

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

	public void Initialize (TcpClient pClient) {
		ShooterPackageSender.SendPackage(new CustomCommands.Creation.FireWallCreation(Id, transform.position.x, transform.position.z, _destroyed), pClient);
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
		_destroyed = true;

		ParticleSystem.EmissionModule em = _particleSystem.emission;
		em.enabled = true;

		ShooterPackageSender.SendPackage(new CustomCommands.Update.FireWallUpdate(Id, _destroyed));
	}
}
